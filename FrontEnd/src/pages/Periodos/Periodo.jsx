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
  DatePicker,
  Row,
  Col,
} from "antd";
import PageHeader from "../../components/PageHeader";
import SearchFilters from "../../components/SearchFilters";
import FormModal from "../../components/FormModal";
import {
  listarPeriodos,
  criarPeriodo,
  atualizarPeriodo,
  deletarPeriodo,
  buscarPeriodoPorId,
} from "../../services/Periodo/periodoService";
import dayjs from "dayjs";

export default function Periodos() {
  const [periodos, setPeriodos] = useState([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState("create");
  const [editingId, setEditingId] = useState(null);
  const [form] = Form.useForm();
  const [saveLoading, setSaveLoading] = useState(false);
  const [filtroNome, setFiltroNome] = useState("");
  const [filtroAno, setFiltroAno] = useState("");

  const fetchPeriodos = async () => {
    setLoading(true);
    try {
      const data = await listarPeriodos();
      setPeriodos(data);
    } catch (err) {
      message.error("Erro ao carregar períodos");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPeriodos();
  }, []);

  // Filtro de períodos
  const periodosFiltrados = periodos.filter((periodo) => {
    const nomeMatch = periodo.nome
      ?.toLowerCase()
      .includes(filtroNome.toLowerCase());
    const anoMatch = filtroAno
      ? periodo.ano?.toString().includes(filtroAno)
      : true;
    return nomeMatch && anoMatch;
  });

  // Filtros
  const handleSearch = () => {
    // Filtro já é reativo
  };
  const handleClearFilters = () => {
    setFiltroNome("");
    setFiltroAno("");
  };
  const handleResetFilters = () => {
    setFiltroNome("");
    setFiltroAno("");
    fetchPeriodos();
  };

  // CRUD
  const handleNovoPeriodo = () => {
    setModalMode("create");
    setEditingId(null);
    form.resetFields();
    form.setFieldsValue({ ano: new Date().getFullYear() });
    setModalOpen(true);
  };

  const handleEditarPeriodo = async (id) => {
    setModalMode("edit");
    setEditingId(id);
    try {
      const data = await buscarPeriodoPorId(id);
      form.setFieldsValue({
        nome: data.nome,
        ano: data.ano,
        dataInicio: data.dataInicio ? dayjs(data.dataInicio) : null,
        dataFim: data.dataFim ? dayjs(data.dataFim) : null,
      });
      setModalOpen(true);
    } catch (err) {
      message.error("Erro ao carregar período");
    }
  };

  const handleCloseModal = () => {
    setModalOpen(false);
    setEditingId(null);
    form.resetFields();
  };

  const handleSavePeriodo = async () => {
    try {
      setSaveLoading(true);
      const values = await form.validateFields();

      // Validação customizada: data fim >= data início
      if (values.dataFim && values.dataInicio) {
        if (dayjs(values.dataFim).isBefore(dayjs(values.dataInicio))) {
          message.error("A data fim não pode ser menor que a data início.");
          return;
        }
      }

      const payload = {
        nome: values.nome,
        ano: values.ano,
        dataInicio: values.dataInicio
          ? dayjs(values.dataInicio).format("YYYY-MM-DDTHH:mm:ss")
          : undefined,
        dataFim: values.dataFim
          ? dayjs(values.dataFim).format("YYYY-MM-DDTHH:mm:ss")
          : undefined,
      };

      if (modalMode === "create") {
        await criarPeriodo(payload);
        message.success("Período criado com sucesso!");
        fetchPeriodos();
        handleCloseModal();
      } else {
        await atualizarPeriodo({ id: editingId, ...payload });
        message.success("Período atualizado com sucesso!");
        fetchPeriodos();
        handleCloseModal();
      }
    } catch (err) {
      if (err?.errorFields) {
        return; // Erro de validação do form
      }
      if (err?.response?.data?.error) {
        message.error(err.response.data.error);
      } else {
        message.error("Erro ao salvar período");
      }
    } finally {
      setSaveLoading(false);
    }
  };

  const handleDeletePeriodo = async (id) => {
    try {
      await deletarPeriodo(id);
      message.success("Período deletado com sucesso!");
      fetchPeriodos();
    } catch (err) {
      if (err?.response?.data?.error) {
        message.error(err.response.data.error);
      } else {
        message.error("Erro ao deletar período");
      }
    }
  };

  const columns = [
    {
      title: "Nome",
      dataIndex: "nome",
      key: "nome",
      sorter: (a, b) => a.nome.localeCompare(b.nome),
    },
    {
      title: "Ano",
      dataIndex: "ano",
      key: "ano",
      width: 100,
      sorter: (a, b) => a.ano - b.ano,
    },
    {
      title: "Data Início",
      dataIndex: "dataInicio",
      key: "dataInicio",
      width: 120,
      render: (date) => (date ? dayjs(date).format("DD/MM/YYYY") : "-"),
    },
    {
      title: "Data Fim",
      dataIndex: "dataFim",
      key: "dataFim",
      width: 120,
      render: (date) => (date ? dayjs(date).format("DD/MM/YYYY") : "-"),
    },
    {
      title: "Ações",
      key: "acoes",
      width: 150,
      render: (_, record) => (
        <Space>
          <Button type="link" onClick={() => handleEditarPeriodo(record.id)}>
            Editar
          </Button>
          <Popconfirm
            title="Tem certeza que deseja excluir?"
            onConfirm={() => handleDeletePeriodo(record.id)}
            okText="Sim"
            cancelText="Não"
          >
            <Button type="link" danger>
              Excluir
            </Button>
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div className="max-w-4xl w-full mx-auto bg-white p-2 sm:p-4 md:p-6 rounded shadow-md mt-4 md:mt-6">
      <PageHeader
        title="Períodos"
        buttonText="Novo Período"
        onButtonClick={handleNovoPeriodo}
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
        <Input
          placeholder="Pesquisar por ano"
          value={filtroAno}
          onChange={(e) => setFiltroAno(e.target.value)}
          allowClear
        />
      </SearchFilters>

      <Table
        columns={columns}
        dataSource={periodosFiltrados}
        rowKey="id"
        loading={loading}
        pagination={{ pageSize: 10 }}
        scroll={{ x: 800 }}
      />

      <FormModal
        open={modalOpen}
        onClose={handleCloseModal}
        onSave={handleSavePeriodo}
        mode={modalMode}
        loading={saveLoading}
        title={modalMode === "edit" ? "Editar Período" : "Novo Período"}
        width={600}
      >
        <Form form={form} layout="vertical" requiredMark={false}>
          <Form.Item
            name="nome"
            label="Nome do Período"
            rules={[
              { required: true, message: "Informe o nome do período" },
              { min: 2, message: "Nome deve ter pelo menos 2 caracteres" },
            ]}
          >
            <Input placeholder="Ex: 1º Semestre" maxLength={100} />
          </Form.Item>

          <Form.Item
            name="ano"
            label="Ano"
            rules={[
              { required: true, message: "Informe o ano" },
              {
                type: "number",
                min: 1900,
                max: 3000,
                message: "Ano deve estar entre 1900 e 3000",
              },
            ]}
          >
            <InputNumber
              placeholder="Ex: 2025"
              style={{ width: "100%" }}
              min={1900}
              max={3000}
            />
          </Form.Item>

          <Row gutter={16}>
            <Col xs={24} md={12}>
              <Form.Item
                name="dataInicio"
                label="Data Início"
                rules={[
                  { required: true, message: "Informe a data de início" },
                ]}
              >
                <DatePicker
                  format="DD/MM/YYYY"
                  style={{ width: "100%" }}
                  placeholder="Selecione a data"
                />
              </Form.Item>
            </Col>

            <Col xs={24} md={12}>
              <Form.Item
                name="dataFim"
                label="Data Fim"
                rules={[{ required: true, message: "Informe a data fim" }]}
              >
                <DatePicker
                  format="DD/MM/YYYY"
                  style={{ width: "100%" }}
                  placeholder="Selecione a data"
                />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </FormModal>
    </div>
  );
}
