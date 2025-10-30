import React, { useEffect, useState } from "react";
import {
  Table,
  Button,
  Space,
  Popconfirm,
  message,
  Form,
  Input,
  InputNumber,
  Select,
  Row,
  Col,
} from "antd";
import {
  listarDisciplinas,
  criarDisciplina,
  atualizarDisciplina,
  excluirDisciplina,
} from "../../services/Disciplina/disciplinaService";
import PageHeader from "../../components/PageHeader";
import SearchFilters from "../../components/SearchFilters";
import FormModal from "../../components/FormModal";
import { useEnums } from "../../contexts/EnumContext";
import "./Disciplina.css";

const { TextArea } = Input;

export default function Disciplinas() {
  const [disciplinas, setDisciplinas] = useState([]);
  const [loading, setLoading] = useState(false);
  const [filtroNome, setFiltroNome] = useState("");
  const [filtroCodigo, setFiltroCodigo] = useState("");

  // Estados do modal
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState("create"); // "create" ou "edit"
  const [editingId, setEditingId] = useState(null);
  const [form] = Form.useForm();
  const [saveLoading, setSaveLoading] = useState(false);

  // Hooks para acessar enums
  const { tipoCalculoNota, loading: enumsLoading } = useEnums();

  const fetchDisciplinas = async () => {
    setLoading(true);
    try {
      const response = await listarDisciplinas();
      console.log("Disciplinas carregadas:", response);
      setDisciplinas(response);
    } catch (error) {
      message.error("Erro ao carregar disciplinas");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDisciplinas();
  }, []);

  // Filtro de disciplinas
  const disciplinasFiltradas = disciplinas.filter((disciplina) => {
    const nomeMatch = disciplina.nome
      ?.toLowerCase()
      .includes(filtroNome.toLowerCase());
    const codigoMatch = disciplina.codigo
      ?.toLowerCase()
      .includes(filtroCodigo.toLowerCase());
    return nomeMatch && codigoMatch;
  });

  // Funções para controle dos filtros
  const handleSearch = () => {
    console.log("Busca realizada:", { filtroNome, filtroCodigo });
  };

  const handleClearFilters = () => {
    setFiltroNome("");
    setFiltroCodigo("");
  };

  const handleResetFilters = () => {
    setFiltroNome("");
    setFiltroCodigo("");
    fetchDisciplinas();
  };

  const handleNovaDisciplina = () => {
    setModalMode("create");
    setEditingId(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEditarDisciplina = (disciplina) => {
    setModalMode("edit");
    setEditingId(disciplina.id);
    form.setFieldsValue({
      nome: disciplina.nome,
      codigo: disciplina.codigo,
      cargaHoraria: disciplina.cargaHoraria,
      descricao: disciplina.descricao,
      tipoCalculoUnidade: disciplina.tipoCalculoUnidade,
      tipoCalculoFinal: disciplina.tipoCalculoFinal,
    });
    setModalOpen(true);
  };

  const handleCloseModal = () => {
    setModalOpen(false);
    setEditingId(null);
    form.resetFields();
  };

  const handleSaveDisciplina = async () => {
    try {
      setSaveLoading(true);
      const values = await form.validateFields();

      console.log("Dados da disciplina:", values);

      if (modalMode === "create") {
        if (await criarDisciplina(values)) {
          message.success("Disciplina criada com sucesso!");
          fetchDisciplinas();
          handleCloseModal(); // Recarrega a lista
        }else{
          message.error("Erro ao criar disciplina");
          console.error("Erro:", error);
        }
      } else {
        // Se retornar sucesso, mostra mensagem e atualiza lista
        const disciplina = { id: editingId, ...values };
        if (await atualizarDisciplina(disciplina)) {
          message.success("Disciplina atualizada com sucesso!");
          //atualza lista sem ir buscar tudo de novo
          const updatedDisciplinas = disciplinas.map((disciplina) =>
            disciplina.id === editingId
              ? { id: editingId, ...values }
              : disciplina
          );
          setDisciplinas(updatedDisciplinas);
        }else{
          message.error("Erro ao atualizar disciplina");
          console.error("Erro:", error);
        }
      }

      
    } catch (error) {
      if (error?.errorFields) {
        return; // Erro de validação do form
      }
      message.error("Erro ao salvar disciplina");
      console.error("Erro:", error);
    } finally {
      setSaveLoading(false);
    }
  };

  const handleDeleteDisciplina = async (id) => {
    try {
      // Simula exclusão - aqui você chamaria o service de exclusão
      if (await excluirDisciplina(id)) {
        message.success("Disciplina excluída com sucesso!");
        fetchDisciplinas();
      }
    } catch (error) {
      message.error("Erro ao excluir disciplina");
    }
  };
  const columns = [
    {
      title: "Nome",
      dataIndex: "nome",
      key: "nome",
      width: 200,
      sorter: (a, b) => a.nome.localeCompare(b.nome),
    },
    {
      title: "Código",
      dataIndex: "codigo",
      key: "codigo",
      width: 100,
      sorter: (a, b) => a.codigo.localeCompare(b.codigo),
    },
    {
      title: "Carga Horária",
      dataIndex: "cargaHoraria",
      key: "cargaHoraria",
      width: 120,
      sorter: (a, b) => a.cargaHoraria - b.cargaHoraria,
      render: (value) => `${value}h`,
    },
    {
      title: "Descrição",
      dataIndex: "descricao",
      key: "descricao",
      width: 200,
      ellipsis: true,
      render: (text) => text || "-",
    },
    {
      title: "Cálculo Unidade",
      dataIndex: "tipoCalculoUnidade",
      key: "tipoCalculoUnidade",
      width: 140,
      render: (value) => {
        const tipo = tipoCalculoNota.find((t) => t.value === value);
        return tipo ? tipo.label : "-";
      },
    },
    {
      title: "Cálculo Final",
      dataIndex: "tipoCalculoFinal",
      key: "tipoCalculoFinal",
      width: 140,
      render: (value) => {
        const tipo = tipoCalculoNota.find((t) => t.value === value);
        return tipo ? tipo.label : "-";
      },
    },
    {
      title: "Ações",
      key: "acoes",
      width: 150,
      fixed: "right",
      render: (_, record) => (
        <Space>
          <Button
            type="link"
            onClick={() => handleEditarDisciplina(record)}
            size="small"
          >
            Editar
          </Button>
          <Popconfirm
            title="Tem certeza que deseja excluir?"
            onConfirm={() => handleDeleteDisciplina(record.id)}
            okText="Sim"
            cancelText="Não"
          >
            <Button type="link" danger size="small">
              Excluir
            </Button>
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <PageHeader
        title="Disciplinas"
        buttonText="Nova Disciplina"
        onButtonClick={handleNovaDisciplina}
      />

      <SearchFilters
        onSearch={handleSearch}
        onClear={handleClearFilters}
        onReset={handleResetFilters}
        showResetButton={true}
        loading={loading}
      >
        <Input
          label="Nome"
          placeholder="Pesquisar por nome"
          value={filtroNome}
          onChange={(e) => setFiltroNome(e.target.value)}
          allowClear
        />
        <Input
          label="Código"
          placeholder="Pesquisar por código"
          value={filtroCodigo}
          onChange={(e) => setFiltroCodigo(e.target.value)}
          allowClear
        />
      </SearchFilters>

      <div style={{ width: "100%", overflowX: "auto" }}>
        <Table
          columns={columns}
          dataSource={disciplinasFiltradas}
          rowKey="id"
          loading={loading}
          pagination={{ pageSize: 10 }}
          scroll={{ x: 1000 }}
        />
      </div>

      <FormModal
        open={modalOpen}
        onClose={handleCloseModal}
        onSave={handleSaveDisciplina}
        mode={modalMode}
        loading={saveLoading}
        title={modalMode === "edit" ? "Editar Disciplina" : "Nova Disciplina"}
        width={700}
      >
        <Form form={form} layout="vertical" requiredMark={false}>
          <Row gutter={16}>
            <Col xs={24} md={12}>
              <Form.Item
                name="nome"
                label="Nome da Disciplina"
                rules={[
                  {
                    required: true,
                    message: "Por favor, informe o nome da disciplina",
                  },
                  { min: 2, message: "Nome deve ter pelo menos 2 caracteres" },
                ]}
              >
                <Input placeholder="Ex: Matemática Básica" />
              </Form.Item>
            </Col>

            <Col xs={24} md={12}>
              <Form.Item
                name="codigo"
                label="Código"
                rules={[
                  { required: true, message: "Por favor, informe o código" },
                  {
                    min: 2,
                    message: "Código deve ter pelo menos 2 caracteres",
                  },
                ]}
              >
                <Input placeholder="Ex: MAT001" />
              </Form.Item>
            </Col>
          </Row>

          <Form.Item
            name="descricao"
            label="Descrição"
            rules={[
              { required: true, message: "Por favor, informe a descrição" },
              {
                min: 10,
                message: "Descrição deve ter pelo menos 10 caracteres",
              },
            ]}
          >
            <TextArea
              placeholder="Descreva os objetivos e conteúdo da disciplina..."
              rows={3}
              maxLength={500}
              showCount
            />
          </Form.Item>

          <Row gutter={16}>
            <Col xs={24} md={8}>
              <Form.Item
                name="cargaHoraria"
                label="Carga Horária (horas)"
                rules={[
                  {
                    required: true,
                    message: "Por favor, informe a carga horária",
                  },
                  {
                    type: "number",
                    min: 1,
                    message: "Carga horária deve ser maior que 0",
                  },
                ]}
              >
                <InputNumber
                  placeholder="Ex: 60"
                  style={{ width: "100%" }}
                  min={1}
                  max={500}
                />
              </Form.Item>
            </Col>

            <Col xs={24} md={8}>
              <Form.Item
                name="tipoCalculoUnidade"
                label="Cálculo da Nota da Unidade"
                rules={[
                  {
                    required: true,
                    message: "Selecione o tipo de cálculo da unidade",
                  },
                ]}
              >
                <Select
                  placeholder="Selecione..."
                  options={tipoCalculoNota}
                  loading={enumsLoading}
                />
              </Form.Item>
            </Col>

            <Col xs={24} md={8}>
              <Form.Item
                name="tipoCalculoFinal"
                label="Cálculo da Nota Final"
                rules={[
                  {
                    required: true,
                    message: "Selecione o tipo de cálculo final",
                  },
                ]}
              >
                <Select
                  placeholder="Selecione..."
                  options={tipoCalculoNota}
                  loading={enumsLoading}
                />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </FormModal>
    </div>
  );
}
