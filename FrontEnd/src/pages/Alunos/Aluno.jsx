import React, { useEffect, useState } from "react";
import {
  Table,
  Button,
  Space,
  Popconfirm,
  message,
  Tabs,
  Form,
  Input,
  DatePicker,
  Select,
  Switch,
  Row,
  Col,
} from "antd";
import {
  listarAlunos,
  deletarAluno,
  cadastrarAluno,
} from "../../services/Aluno/alunoService";
import {
  cadastrarEnderecoPessoa,
  atualizarEnderecoPessoa,
} from "../../services/Aluno/enderecoService";
import {
  cadastrarPessoa,
  atualizarPessoa,
  associarResponsavelAluno,
} from "../../services/Aluno/pessoaService";
import PageHeader from "../../components/PageHeader";
import SearchFilters from "../../components/SearchFilters";
import FormModal from "../../components/FormModal";
import api from "../../services/api";
import dayjs from "dayjs";
import { useEnums } from "../../contexts/EnumContext";

export default function ListaAlunos() {
  const [alunos, setAlunos] = useState([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState("create"); // "create" ou "edit"
  const [tabKey, setTabKey] = useState("dados");
  const [responsavelId, setResponsavelId] = useState(null);
  const [alunoId, setAlunoId] = useState(null); // ID do aluno (GUID)
  const [pessoaId, setPessoaId] = useState(null); // ID da pessoa do aluno (GUID)
  const [form] = Form.useForm();
  const [formEndereco] = Form.useForm();
  const [formResponsavel] = Form.useForm();
  const [filtroNome, setFiltroNome] = useState("");
  const [filtroStatus, setFiltroStatus] = useState(null);
  const [saveLoading, setSaveLoading] = useState(false);
  const {
    sexo: opcoesSexo,
    corRaca: opcoesCorRaca,
    tipoCertidao: opcoesTipoCertidao,
    loading: enumsLoading,
  } = useEnums();

  const fetchAlunos = async () => {
    setLoading(true);
    try {
      const data = await listarAlunos();
      setAlunos(data);
    } catch (err) {
      message.error("Erro ao carregar alunos");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAlunos();
  }, []);

  // Funções para controle dos filtros
  const handleSearch = () => {
    // A filtragem já acontece automaticamente via alunosFiltrados
    // Esta função pode ser usada para analytics ou outras ações
    console.log("Busca realizada:", { filtroNome, filtroStatus });
  };

  const handleClearFilters = () => {
    setFiltroNome("");
    setFiltroStatus(null);
  };

  const handleResetFilters = () => {
    setFiltroNome("");
    setFiltroStatus(null);
    fetchAlunos(); // Recarrega os dados
  };

  // Filtro de alunos
  const alunosFiltrados = alunos.filter((aluno) => {
    const nomeMatch = aluno.nomeCompleto
      ?.toLowerCase()
      .includes(filtroNome.toLowerCase());
    const statusMatch =
      filtroStatus === null ? true : aluno.ativo === filtroStatus;
    return nomeMatch && statusMatch;
  });

  const handleDelete = async (id) => {
    try {
      await deletarAluno(id);
      message.success("Aluno deletado com sucesso!");
      fetchAlunos();
    } catch (err) {
      message.error("Erro ao deletar aluno");
    }
  };

  const handleNovoAluno = () => {
    setModalMode("create");
    setModalOpen(true);
    setTabKey("dados");
    setAlunoId(null); // Reset ID ao criar novo aluno
    setPessoaId(null);
    form.resetFields();
    formEndereco.resetFields();
    formResponsavel.resetFields();
    form.setFieldsValue({
      ativo: true,
    });
    // Defaults do endereço
    formEndereco.setFieldsValue({
      pais: "Brasil",
      tipo: "Residencial",
      principal: true,
    });
  };

  const handleEditarAluno = async (id) => {
    setModalMode("edit");
    setModalOpen(true);
    setTabKey("dados");
    setAlunoId(id);
    try {
      const { data } = await api.get(`/api/alunos/${id}`);
      // Captura pessoaId vindo do aluno
      setPessoaId(data.pessoaId ?? data.pessoa?.id ?? null);
      // Endereço principal (primeiro da lista)
      const enderecoPrincipal = data.enderecos?.[0];
      form.setFieldsValue({
        ...data,
        dataNascimento: data.dataNascimento ? dayjs(data.dataNascimento) : null,
      });
      formEndereco.setFieldsValue({
        logradouro:
          enderecoPrincipal?.Logradouro || enderecoPrincipal?.logradouro || "",
        numero: enderecoPrincipal?.Numero || enderecoPrincipal?.numero || "",
        complemento:
          enderecoPrincipal?.Complemento ||
          enderecoPrincipal?.complemento ||
          "",
        bairro: enderecoPrincipal?.Bairro || enderecoPrincipal?.bairro || "",
        cidade: enderecoPrincipal?.Cidade || enderecoPrincipal?.cidade || "",
        uf:
          enderecoPrincipal?.Estado ||
          enderecoPrincipal?.estado ||
          enderecoPrincipal?.uf ||
          "",
        cep: enderecoPrincipal?.CEP || enderecoPrincipal?.cep || "",
        pais: enderecoPrincipal?.Pais || enderecoPrincipal?.pais || "Brasil",
        tipo:
          enderecoPrincipal?.Tipo || enderecoPrincipal?.tipo || "Residencial",
        principal:
          enderecoPrincipal?.Principal ?? enderecoPrincipal?.principal ?? true,
        enderecoId: enderecoPrincipal?.Id || enderecoPrincipal?.id || null,
      });
      formResponsavel.setFieldsValue({
        nomeCompletoResponsavel: data.responsavel?.nomeCompleto || "",
        cpfResponsavel: data.responsavel?.cpf || "",
        dataNascimentoResponsavel: data.responsavel?.dataNascimento
          ? dayjs(data.responsavel.dataNascimento)
          : null,
        emailResponsavel: data.responsavel?.email || "",
        telefoneResponsavel: data.responsavel?.telefone || "",
        corRacaResponsavel: data.responsavel?.corRaca,
        rgResponsavel: data.responsavel?.rg || "",
        sexoResponsavel: data.responsavel?.sexo,
        nacionalidadeResponsavel: data.responsavel?.nacionalidade || "",
        naturalidadeResponsavel: data.responsavel?.naturalidade || "",
      });
      setResponsavelId(data.responsavel?.id || null);
    } catch (err) {
      message.error("Erro ao carregar dados do aluno");
    }
  };

  const handleCloseModal = () => {
    setModalOpen(false);
    setAlunoId(null);
    setPessoaId(null);
    form.resetFields();
    formEndereco.resetFields();
    formResponsavel.resetFields();
  };
  // Função dedicada para salvar responsável
  const handleSaveAluno = async () => {
    try {
      setSaveLoading(true);
      const values = await form.validateFields();

      if (modalMode === "create") {
        // Cadastro novo aluno
        const payload = {
          nomeCompleto: values.nomeCompleto,
          cpf: values.cpf,
          dataNascimento: values.dataNascimento
            ? values.dataNascimento.format("YYYY-MM-DD")
            : undefined,
          email: values.email,
          corRaca: values.corRaca,
          rg: values.rg,
          sexo: values.sexo,
          nacionalidade: values.nacionalidade,
          naturalidade: values.naturalidade,
          certidaoNumero: values.certidaoNumero,
          certidaoTipo: values.certidaoTipo,
          ativo: values.ativo ?? true,
        };
        const response = await cadastrarAluno(payload);
        const novoAlunoId = response.id || response.data?.id;
        let novoPessoaId = response.pessoaId || response.data?.pessoaId;
        if (novoAlunoId) {
          setAlunoId(novoAlunoId);
          if (!novoPessoaId) {
            // Fallback para obter pessoaId após cadastro
            try {
              const { data: alunoCarregado } = await api.get(
                `/api/alunos/${novoAlunoId}`
              );
              novoPessoaId =
                alunoCarregado.pessoaId ?? alunoCarregado.pessoa?.id ?? null;
              setPessoaId(novoPessoaId);
            } catch (e) {
              console.warn(
                "Não foi possível obter pessoaId após cadastro do aluno."
              );
            }
          } else {
            setPessoaId(novoPessoaId);
          }
          message.success("Aluno cadastrado com sucesso!");
          fetchAlunos();
          handleCloseModal();
        } else {
          message.warning(
            "Aluno cadastrado, mas ID não foi retornado. Feche e reabra para editar."
          );
          handleCloseModal();
          fetchAlunos();
        }
      } else if (modalMode === "edit") {
        // Atualizar dados do aluno (PUT)
        const payload = {
          nomeCompleto: values.nomeCompleto,
          cpf: values.cpf,
          dataNascimento: values.dataNascimento
            ? values.dataNascimento.format("YYYY-MM-DD")
            : undefined,
          email: values.email,
          corRaca: values.corRaca,
          rg: values.rg,
          sexo: values.sexo,
          nacionalidade: values.nacionalidade,
          naturalidade: values.naturalidade,
          certidaoNumero: values.certidaoNumero,
          certidaoTipo: values.certidaoTipo,
          ativo: values.ativo ?? true,
        };
        await api.put(`/api/alunos/${alunoId}`, payload);
        message.success("Dados do aluno atualizados com sucesso!");
        fetchAlunos();
        handleCloseModal();
      }
    } catch (err) {
      if (err?.errorFields) return;
      if (err?.response?.data?.message) {
        message.error(err.response.data.message);
      } else {
        message.error("Erro ao salvar dados.");
      }
    } finally {
      setSaveLoading(false);
    }
  };

  const columns = [
    {
      title: "Nome",
      dataIndex: "nomeCompleto",
      key: "nomeCompleto",
    },
    {
      title: "Responsável",
      key: "responsavel",
      render: (_, record) => record.responsavel?.nomeCompleto || "-",
    },
    {
      title: "Telefone do Responsável",
      key: "telefoneResponsavel",
      render: (_, record) => record.responsavel?.telefone || "-",
    },
    {
      title: "Status",
      key: "ativo",
      render: (_, record) => (record.ativo ? "Ativo" : "Inativo"),
    },
    {
      title: "Ações",
      key: "acoes",
      render: (_, record) => (
        <Space>
          <Button type="link" onClick={() => handleEditarAluno(record.id)}>
            Visualizar/Editar
          </Button>
          <Popconfirm
            title="Tem certeza que deseja deletar?"
            onConfirm={() => handleDelete(record.id)}
            okText="Sim"
            cancelText="Não"
          >
            <Button type="link" danger>
              Deletar
            </Button>
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div className="max-w-4xl w-full mx-auto bg-white p-2 sm:p-4 md:p-6 rounded shadow-md mt-4 md:mt-6">
      <PageHeader
        title="Alunos"
        buttonText="Novo Aluno"
        onButtonClick={handleNovoAluno}
      />

      <SearchFilters
        onSearch={handleSearch}
        onClear={handleClearFilters}
        onReset={handleResetFilters}
        showResetButton={true}
        loading={loading}
      >
        <Input
          placeholder="Pesquisar por nome"
          value={filtroNome}
          onChange={(e) => setFiltroNome(e.target.value)}
          allowClear
        />
        <Select
          placeholder="Status"
          allowClear
          value={filtroStatus}
          onChange={setFiltroStatus}
          options={[
            { label: "Ativo", value: true },
            { label: "Inativo", value: false },
          ]}
        />
      </SearchFilters>
      <div style={{ width: "100%", overflowX: "auto" }}>
        <Table
          columns={columns}
          dataSource={alunosFiltrados}
          rowKey="id"
          loading={loading}
          pagination={{ pageSize: 10 }}
          scroll={{ x: 600 }}
        />
      </div>

      <FormModal
        open={modalOpen}
        onClose={handleCloseModal}
        onSave={handleSaveAluno}
        mode={modalMode}
        loading={saveLoading}
        title={modalMode === "edit" ? "Editar Aluno" : "Novo Aluno"}
        width={800}
      >
        <Tabs
          activeKey={tabKey}
          onChange={setTabKey}
          items={[
            {
              key: "dados",
              label: "Dados do Aluno",
              children: (
                <Form
                  layout="vertical"
                  form={form}
                  requiredMark={false}
                  initialValues={{ ativo: true }}
                >
                  <Form.Item
                    name="nomeCompleto"
                    label="Nome completo"
                    rules={[
                      { required: true, message: "Informe o nome completo" },
                    ]}
                  >
                    <Input placeholder="Nome completo" />
                  </Form.Item>
                  <Form.Item
                    name="cpf"
                    label="CPF"
                    rules={[
                      { required: true, message: "Informe o CPF" },
                      {
                        pattern: /^\d{11}$/,
                        message: "CPF deve ter 11 dígitos",
                      },
                    ]}
                  >
                    <Input placeholder="Somente números" maxLength={11} />
                  </Form.Item>
                  <Form.Item
                    name="dataNascimento"
                    label="Data de nascimento"
                    rules={[
                      {
                        required: true,
                        message: "Informe a data de nascimento",
                      },
                    ]}
                  >
                    <DatePicker
                      format="DD/MM/YYYY"
                      style={{ width: "100%" }}
                      placeholder="Selecione a data"
                      disabledDate={(current) => current && current > dayjs()}
                    />
                  </Form.Item>
                  <Form.Item
                    name="email"
                    label="E-mail"
                    rules={[
                      {
                        required: true,
                        type: "email",
                        message: "Informe um e-mail válido",
                      },
                    ]}
                  >
                    <Input placeholder="E-mail" />
                  </Form.Item>
                  <Form.Item name="rg" label="RG">
                    <Input placeholder="RG" />
                  </Form.Item>
                  <Form.Item
                    name="sexo"
                    label="Sexo"
                    rules={[{ required: true, message: "Selecione o sexo" }]}
                  >
                    <Select
                      options={opcoesSexo}
                      placeholder="Selecione"
                      loading={opcoesSexo.length === 0}
                    />
                  </Form.Item>
                  <Form.Item name="corRaca" label="Cor/Raça">
                    <Select
                      options={opcoesCorRaca}
                      placeholder="Selecione"
                      loading={opcoesCorRaca.length === 0}
                    />
                  </Form.Item>
                  <Form.Item name="nacionalidade" label="Nacionalidade">
                    <Input placeholder="Nacionalidade" />
                  </Form.Item>
                  <Form.Item name="naturalidade" label="Naturalidade">
                    <Input placeholder="Naturalidade" />
                  </Form.Item>
                  <Form.Item name="certidaoNumero" label="Número da Certidão">
                    <Input placeholder="Número da certidão" />
                  </Form.Item>
                  <Form.Item name="certidaoTipo" label="Tipo da Certidão">
                    <Select
                      options={opcoesTipoCertidao}
                      placeholder="Selecione"
                    />
                  </Form.Item>
                  <Form.Item name="ativo" label="Ativo" valuePropName="checked">
                    <Switch
                      checkedChildren="Ativo"
                      unCheckedChildren="Inativo"
                      defaultChecked
                    />
                  </Form.Item>
                </Form>
              ),
            },
            {
              key: "endereco",
              label: "Endereço",
              disabled: !alunoId, // Desabilita até que o aluno seja criado
              children: (
                <Form
                  layout="vertical"
                  form={formEndereco}
                  style={{ padding: 24 }}
                >
                  <Form.Item name="logradouro" label="Logradouro">
                    <Input placeholder="Logradouro" />
                  </Form.Item>
                  <Form.Item name="numero" label="Número">
                    <Input placeholder="Número" />
                  </Form.Item>
                  <Form.Item name="complemento" label="Complemento">
                    <Input placeholder="Complemento" />
                  </Form.Item>
                  <Form.Item name="bairro" label="Bairro">
                    <Input placeholder="Bairro" />
                  </Form.Item>
                  <Form.Item name="cidade" label="Cidade">
                    <Input placeholder="Cidade" />
                  </Form.Item>
                  <Form.Item name="uf" label="UF">
                    <Input placeholder="UF" maxLength={2} />
                  </Form.Item>
                  <Form.Item name="cep" label="CEP">
                    <Input placeholder="CEP" maxLength={8} />
                  </Form.Item>
                  <Form.Item name="pais" label="País">
                    <Input placeholder="País" defaultValue="Brasil" />
                  </Form.Item>
                  <Form.Item name="tipo" label="Tipo de Endereço">
                    <Select
                      placeholder="Selecione o tipo"
                      options={[
                        { label: "Residencial", value: "Residencial" },
                        { label: "Comercial", value: "Comercial" },
                        { label: "Outro", value: "Outro" },
                      ]}
                    />
                  </Form.Item>
                  <Form.Item
                    name="principal"
                    label="Endereço Principal"
                    valuePropName="checked"
                  >
                    <Switch checkedChildren="Sim" unCheckedChildren="Não" />
                  </Form.Item>
                  <Form.Item name="enderecoId" style={{ display: "none" }}>
                    <Input type="hidden" />
                  </Form.Item>
                </Form>
              ),
            },
            {
              key: "responsavel",
              label: "Responsável",
              disabled: !alunoId, // Desabilita até que o aluno seja criado
              children: (
                <Form
                  layout="vertical"
                  form={formResponsavel}
                  style={{ padding: 24 }}
                >
                  <Form.Item
                    name="nomeCompletoResponsavel"
                    label="Nome completo"
                  >
                    <Input placeholder="Nome do responsável" />
                  </Form.Item>
                  <Row gutter={16}>
                    <Col xs={24} md={12}>
                      <Form.Item name="cpfResponsavel" label="CPF">
                        <Input placeholder="CPF" maxLength={11} />
                      </Form.Item>
                    </Col>
                    <Col xs={24} md={12}>
                      <Form.Item
                        name="dataNascimentoResponsavel"
                        label="Data de nascimento"
                      >
                        <DatePicker
                          format="DD/MM/YYYY"
                          style={{ width: "100%" }}
                          placeholder="Selecione a data"
                        />
                      </Form.Item>
                    </Col>
                  </Row>
                  <Form.Item name="emailResponsavel" label="E-mail">
                    <Input placeholder="E-mail" />
                  </Form.Item>
                  <Row gutter={16}>
                    <Col xs={24} md={12}>
                      <Form.Item name="telefoneResponsavel" label="Telefone">
                        <Input placeholder="Telefone" />
                      </Form.Item>
                    </Col>
                    <Col xs={24} md={12}>
                      <Form.Item name="corRacaResponsavel" label="Cor/Raça">
                        <Select
                          options={opcoesCorRaca}
                          placeholder="Selecione"
                        />
                      </Form.Item>
                    </Col>
                  </Row>
                  <Row gutter={16}>
                    <Col xs={24} md={12}>
                      <Form.Item name="rgResponsavel" label="RG">
                        <Input placeholder="RG" />
                      </Form.Item>
                    </Col>
                    <Col xs={24} md={12}>
                      <Form.Item name="sexoResponsavel" label="Sexo">
                        <Select options={opcoesSexo} placeholder="Selecione" />
                      </Form.Item>
                    </Col>
                  </Row>
                  <Row gutter={16}>
                    <Col xs={24} md={12}>
                      <Form.Item
                        name="nacionalidadeResponsavel"
                        label="Nacionalidade"
                      >
                        <Input placeholder="Nacionalidade" />
                      </Form.Item>
                    </Col>
                    <Col xs={24} md={12}>
                      <Form.Item
                        name="naturalidadeResponsavel"
                        label="Naturalidade"
                      >
                        <Input placeholder="Naturalidade" />
                      </Form.Item>
                    </Col>
                  </Row>
                </Form>
              ),
            },
          ]}
        />
      </FormModal>
    </div>
  );
}
