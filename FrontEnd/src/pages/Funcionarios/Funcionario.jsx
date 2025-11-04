import React, { useEffect, useMemo, useState } from "react";
import {
  Table,
  Button,
  Space,
  message,
  Form,
  Input,
  Select,
  Row,
  Col,
  DatePicker,
  Popconfirm,
  Modal,
  App,
} from "antd";
import { ExclamationCircleOutlined } from "@ant-design/icons";
import PageHeader from "../../components/PageHeader";
import SearchFilters from "../../components/SearchFilters";
import FormModal from "../../components/FormModal";
import dayjs from "dayjs";
import { useEnums } from "../../contexts/EnumContext";
import {
  listarFuncionarios,
  buscarFuncionarioPorId,
  criarFuncionario,
  atualizarFuncionario,
  deletarFuncionario,
} from "../../services/Funcionario/funcionarioService";
import { buscarPessoaPorCpf } from "../../services/Aluno/pessoaService";

export default function Funcionarios() {
  const { modal } = App.useApp();
  const { sexo, corRaca, tipoFuncionario } = useEnums();

  // Lista e filtros
  const [funcionarios, setFuncionarios] = useState([]);
  const [loading, setLoading] = useState(false);
  const [filtroNome, setFiltroNome] = useState("");

  // Modal
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState("create");
  const [editingId, setEditingId] = useState(null);
  const [saveLoading, setSaveLoading] = useState(false);
  const [pessoaId, setPessoaId] = useState(null); // ID da pessoa quando CPF já existe
  const [form] = Form.useForm();

  const fetchFuncionarios = async (nome) => {
    setLoading(true);
    try {
      const data = await listarFuncionarios(nome);
      setFuncionarios(Array.isArray(data) ? data : []);
    } catch (err) {
      message.error("Erro ao carregar funcionários");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchFuncionarios();
  }, []);

  const funcionariosFiltrados = useMemo(() => {
    // Filtro adicional local por nome (além do backend)
    if (!filtroNome) return funcionarios;
    return funcionarios.filter((f) =>
      f.nomeCompleto?.toLowerCase().includes(filtroNome.toLowerCase())
    );
  }, [funcionarios, filtroNome]);

  const handleSearch = async () => {
    await fetchFuncionarios(filtroNome || undefined);
  };

  const handleClearFilters = () => {
    setFiltroNome("");
  };

  const handleNovo = () => {
    setModalMode("create");
    setEditingId(null);
    setPessoaId(null); // limpa pessoaId ao criar novo
    form.resetFields();
    setModalOpen(true);
  };

  const handleEditar = async (id) => {
    setModalMode("edit");
    setEditingId(id);
    try {
      const data = await buscarFuncionarioPorId(id);
      form.setFieldsValue({
        nomeCompleto: data.nomeCompleto || "",
        cpf: data.cpf || "",
        dataNascimento: data.dataNascimento ? dayjs(data.dataNascimento) : null,
        email: data.email || "",
        telefone: data.telefone || "",
        corRaca: data.corRaca ?? null,
        rg: data.rg || "",
        sexo: data.sexo ?? null,
        nacionalidade: data.nacionalidade || "",
        naturalidade: data.naturalidade || "",
        tipoFuncionario: data.tipoFuncionario ?? null,
        // username e password não vêm na edição
      });
      setPessoaId(data.pessoaId || null); // define pessoaId se existir
      setModalOpen(true);
    } catch (err) {
      message.error("Erro ao carregar funcionário");
    }
  };

  // Validação básica de CPF (algoritmo) - retorna true se válido
  function isValidCPF(cpf) {
    if (!cpf) return false;
    const s = String(cpf).replace(/\D/g, "");
    if (s.length !== 11) return false;
    if (/^(\d)\1+$/.test(s)) return false; // todos dígitos iguais

    const calc = (t) => {
      let sum = 0;
      for (let i = 0; i < t; i++) sum += parseInt(s.charAt(i)) * (t + 1 - i);
      const r = sum % 11;
      return r < 2 ? 0 : 11 - r;
    };
    return (
      calc(9) === parseInt(s.charAt(9)) && calc(10) === parseInt(s.charAt(10))
    );
  }

  // Aplica máscara de CPF (000.000.000-00)
  const formatCpf = (value) => {
    if (!value) return "";
    const raw = String(value).replace(/\D/g, "");
    if (raw.length <= 3) return raw;
    if (raw.length <= 6) return `${raw.slice(0, 3)}.${raw.slice(3)}`;
    if (raw.length <= 9)
      return `${raw.slice(0, 3)}.${raw.slice(3, 6)}.${raw.slice(6)}`;
    return `${raw.slice(0, 3)}.${raw.slice(3, 6)}.${raw.slice(
      6,
      9
    )}-${raw.slice(9, 11)}`;
  };

  const handleCpfChange = (e) => {
    const formatted = formatCpf(e.target.value);
    form.setFieldsValue({ cpf: formatted });
  };

  const handleCpfBlur = async (value) => {
    const raw = String(value || "").replace(/\D/g, "");
    if (!raw) return;
    if (raw.length < 11) {
      // não suficiente para validar
      return;
    }
    if (!isValidCPF(raw)) {
      message.error("CPF inválido");
      return;
    }

    // Busca pessoa pelo CPF e popula o formulário quando encontrada
    try {
      const pessoa = await buscarPessoaPorCpf(raw);
      if (!pessoa) {
        // não encontrou - não é erro, apenas informa
        return;
      }

      // Exibe modal de confirmação antes de preencher os dados
      modal.confirm({
        title: "Cadastro Existente",
        icon: <ExclamationCircleOutlined />,
        content: `Já existe um cadastro para o CPF ${formatCpf(
          raw
        )}. Deseja recuperar os dados desta pessoa?`,
        okText: "Sim, recuperar",
        cancelText: "Não",
        onOk() {
          // Popula campos do formulário com dados da pessoa
          form.setFieldsValue({
            nomeCompleto: pessoa.nomeCompleto || pessoa.nome || "",
            cpf: formatCpf(pessoa.cpf || raw), // aplica máscara
            dataNascimento: pessoa.dataNascimento
              ? dayjs(pessoa.dataNascimento)
              : null,
            email: pessoa.email || "",
            telefone: pessoa.telefone || pessoa.celular || "",
            corRaca: pessoa.corRaca ?? null,
            rg: pessoa.rg || "",
            sexo: pessoa.sexo ?? null,
            nacionalidade: pessoa.nacionalidade || "",
            naturalidade: pessoa.naturalidade || "",
          });
          // Define o pessoaId para associar ao funcionário
          setPessoaId(pessoa.id);
          message.success("Dados recuperados com sucesso");
        },
        onCancel() {
          form.setFieldsValue({
            cpf: "",
          });
          setPessoaId(null);
        },
      });
    } catch (err) {
      console.error("Erro ao buscar pessoa por CPF:", err);
      message.error("Erro ao buscar dados do CPF");
    }
  };

  const handleSave = async () => {
    try {
      setSaveLoading(true);
      const values = await form.validateFields();

      const payload = {
        nomeCompleto: values.nomeCompleto,
        cpf: String(values.cpf || "").replace(/\D/g, ""), // remove máscara antes de enviar
        dataNascimento: values.dataNascimento
          ? values.dataNascimento.format("YYYY-MM-DD")
          : null,
        email: values.email,
        telefone: values.telefone || null,
        corRaca: values.corRaca,
        rg: values.rg || null,
        sexo: values.sexo,
        nacionalidade: values.nacionalidade || null,
        naturalidade: values.naturalidade || null,
        tipoFuncionario: values.tipoFuncionario,
        ...(pessoaId && { pessoaId }), // inclui pessoaId se existir
      };

      if (modalMode === "create") {
        if (await criarFuncionario(payload)) {
          message.success("Funcionário criado com sucesso!");
        }
      } else {
        if (await atualizarFuncionario(editingId, payload)) {
          message.success("Funcionário atualizado com sucesso!");
        }
      }

      setModalOpen(false);
      form.resetFields();
      setEditingId(null);
      fetchFuncionarios(filtroNome || undefined);
    } catch (err) {
      if (err?.errorFields) return; // validação do form
      const apiMsg = err?.response?.data?.error || err?.response?.data?.message;
      message.error(apiMsg || "Erro ao salvar funcionário");
    } finally {
      setSaveLoading(false);
    }
  };

  const columns = [
    { title: "Nome", dataIndex: "nomeCompleto", key: "nomeCompleto" },
    { title: "CPF", dataIndex: "cpf", key: "cpf", width: 140 },
    { title: "Email", dataIndex: "email", key: "email" },
    {
      title: "Tipo",
      dataIndex: "tipoFuncionario",
      key: "tipoFuncionario",
      render: (v) => tipoFuncionario.find((t) => t.value === v)?.label || "-",
      width: 140,
    },
    {
      title: "Ações",
      key: "acoes",
      width: 120,
      render: (_, record) => (
        <Space>
          <Button
            type="link"
            size="small"
            onClick={(e) => {
              e?.stopPropagation?.();
              handleEditar(record.id);
            }}
          >
            Editar
          </Button>
          <Popconfirm
            title="Excluir funcionário?"
            onConfirm={async (e) => {
              e?.stopPropagation?.();
              try {
                await deletarFuncionario(record.id);
                message.success("Excluído");
                if (editingId === record.id) {
                  setEditingId(null);
                  form.resetFields();
                }
                fetchFuncionarios(filtroNome || undefined);
              } catch (err) {
                message.error("Erro ao excluir");
              }
            }}
            okText="Sim"
            cancelText="Não"
          >
            <Button
              size="small"
              type="link"
              danger
              onClick={(e) => e.stopPropagation()}
            >
              Excluir
            </Button>
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div className="max-w-6xl w-full mx-auto bg-white p-2 sm:p-4 md:p-6 rounded shadow-md mt-4 md:mt-6">
      <PageHeader
        title="Funcionários"
        buttonText="Novo Funcionário"
        onButtonClick={handleNovo}
      />

      <SearchFilters
        onSearch={handleSearch}
        onClear={handleClearFilters}
        loading={loading}
      >
        <Input
          placeholder="Pesquisar por nome"
          value={filtroNome}
          onChange={(e) => setFiltroNome(e.target.value)}
          allowClear
        />
      </SearchFilters>

      <Table
        columns={columns}
        dataSource={funcionariosFiltrados}
        rowKey="id"
        loading={loading}
        pagination={{ pageSize: 10 }}
      />

      <FormModal
        open={modalOpen}
        onClose={() => {
          setModalOpen(false);
          form.resetFields();
          setEditingId(null);
          setPessoaId(null); // limpa pessoaId ao fechar modal
        }}
        onSave={handleSave}
        mode={modalMode}
        loading={saveLoading}
        title={modalMode === "edit" ? "Editar Funcionário" : "Novo Funcionário"}
        width={900}
      >
        <Form form={form} layout="vertical" requiredMark={false} width="100%">
          <Row gutter={16}>
            <Col xs={24} md={12}>
              <Form.Item
                name="nomeCompleto"
                label="Nome Completo"
                rules={[{ required: true, message: "Informe o nome completo" }]}
              >
                <Input maxLength={200} />
              </Form.Item>
            </Col>
            <Col xs={24} md={6}>
              <Form.Item
                name="cpf"
                label="CPF"
                rules={[{ required: true, message: "Informe o CPF" }]}
              >
                <Input
                  maxLength={14}
                  onChange={handleCpfChange}
                  onBlur={(e) => handleCpfBlur(e.target.value)}
                  placeholder="000.000.000-00"
                />
              </Form.Item>
            </Col>
            <Col xs={24} md={6}>
              <Form.Item name="dataNascimento" label="Data de Nascimento">
                <DatePicker style={{ width: "100%" }} format="YYYY-MM-DD" />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col xs={24} md={8}>
              <Form.Item
                name="email"
                label="Email"
                rules={[{ type: "email", message: "Email inválido" }]}
              >
                <Input maxLength={150} />
              </Form.Item>
            </Col>
            <Col xs={24} md={8}>
              <Form.Item name="telefone" label="Telefone">
                <Input maxLength={20} />
              </Form.Item>
            </Col>
            <Col xs={24} md={8}>
              <Form.Item name="rg" label="RG">
                <Input maxLength={20} />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col xs={24} md={8}>
              <Form.Item name="sexo" label="Sexo">
                <Select options={sexo} allowClear />
              </Form.Item>
            </Col>
            <Col xs={24} md={8}>
              <Form.Item name="corRaca" label="Cor/Raça">
                <Select options={corRaca} allowClear />
              </Form.Item>
            </Col>
            <Col xs={24} md={8}>
              <Form.Item
                name="tipoFuncionario"
                label="Tipo de Funcionário"
                rules={[{ required: true, message: "Selecione o tipo" }]}
              >
                <Select options={tipoFuncionario} allowClear />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col xs={24} md={12}>
              <Form.Item name="nacionalidade" label="Nacionalidade">
                <Input maxLength={100} />
              </Form.Item>
            </Col>
            <Col xs={24} md={12}>
              <Form.Item name="naturalidade" label="Naturalidade">
                <Input maxLength={100} />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </FormModal>
    </div>
  );
}
