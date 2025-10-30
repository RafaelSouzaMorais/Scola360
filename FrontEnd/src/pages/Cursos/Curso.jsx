import React, { useEffect, useState } from "react";
import { Table, Button, Space, Popconfirm, message, Form, Input } from "antd";
import PageHeader from "../../components/PageHeader";
import SearchFilters from "../../components/SearchFilters";
import FormModal from "../../components/FormModal";
import {
  listarCursos,
  criarCurso,
  atualizarCurso,
  deletarCurso,
  buscarCursoPorId,
} from "../../services/Curso/cursoService";

export default function Cursos() {
  const [cursos, setCursos] = useState([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState("create");
  const [editingId, setEditingId] = useState(null);
  const [form] = Form.useForm();
  const [saveLoading, setSaveLoading] = useState(false);
  const [filtroNome, setFiltroNome] = useState("");

  const fetchCursos = async () => {
    setLoading(true);
    try {
      const data = await listarCursos();
      setCursos(data);
    } catch (err) {
      message.error("Erro ao carregar cursos");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCursos();
  }, []);

  // Filtro de cursos
  const cursosFiltrados = cursos.filter((curso) => {
    return curso.nome?.toLowerCase().includes(filtroNome.toLowerCase());
  });

  // Filtros
  const handleSearch = () => {
    // Filtro já é reativo
  };
  const handleClearFilters = () => {
    setFiltroNome("");
  };
  const handleResetFilters = () => {
    setFiltroNome("");
    fetchCursos();
  };

  // CRUD
  const handleNovoCurso = () => {
    setModalMode("create");
    setEditingId(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEditarCurso = async (id) => {
    setModalMode("edit");
    setEditingId(id);
    try {
      const data = await buscarCursoPorId(id);
      form.setFieldsValue({
        nome: data.nome,
        descricao: data.descricao,
      });
      setModalOpen(true);
    } catch (err) {
      message.error("Erro ao carregar curso");
    }
  };

  const handleCloseModal = () => {
    setModalOpen(false);
    setEditingId(null);
    form.resetFields();
  };

  const handleSaveCurso = async () => {
    try {
      setSaveLoading(true);
      const values = await form.validateFields();
      if (modalMode === "create") {
        if (await criarCurso(values)) {
          message.success("Curso criado com sucesso!");
          fetchCursos();
          handleCloseModal();
        }
      } else {
        if (await atualizarCurso({ id: editingId, ...values })) {
          message.success("Curso atualizado com sucesso!");
          const updatedCursos = cursos.map((curso) =>
            curso.id === editingId ? { ...curso, ...values } : curso
          );
          setCursos(updatedCursos);
          handleCloseModal();
        }
      }
    } catch (err) {
      if (err?.response?.data?.error) {
        message.error(err.response.data.error);
      } else {
        message.error("Erro ao salvar curso");
      }
    } finally {
      setSaveLoading(false);
    }
  };

  const handleDeleteCurso = async (id) => {
    try {
      await deletarCurso(id);
      message.success("Curso deletado com sucesso!");
      fetchCursos();
    } catch (err) {
      if (err?.response?.data?.error) {
        message.error(err.response.data.error);
      } else {
        message.error("Erro ao deletar curso");
      }
    }
  };

  const columns = [
    {
      title: "Nome",
      dataIndex: "nome",
      key: "nome",
    },
    {
      title: "Descrição",
      dataIndex: "descricao",
      key: "descricao",
      ellipsis: true,
    },
    {
      title: "Ações",
      key: "acoes",
      render: (_, record) => (
        <Space>
          <Button type="link" onClick={() => handleEditarCurso(record.id)}>
            Editar
          </Button>
          <Popconfirm
            title="Tem certeza que deseja excluir?"
            onConfirm={() => handleDeleteCurso(record.id)}
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
    <div className="max-w-2xl w-full mx-auto bg-white p-2 sm:p-4 md:p-6 rounded shadow-md mt-4 md:mt-6">
      <PageHeader
        title="Cursos"
        buttonText="Novo Curso"
        onButtonClick={handleNovoCurso}
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
      </SearchFilters>

      <Table
        columns={columns}
        dataSource={cursosFiltrados}
        rowKey="id"
        loading={loading}
        pagination={{ pageSize: 10 }}
        scroll={{ x: 600 }}
      />

      <FormModal
        open={modalOpen}
        onClose={handleCloseModal}
        onSave={handleSaveCurso}
        mode={modalMode}
        loading={saveLoading}
        title={modalMode === "edit" ? "Editar Curso" : "Novo Curso"}
        width={500}
      >
        <Form form={form} layout="vertical" requiredMark={false}>
          <Form.Item
            name="nome"
            label="Nome do Curso"
            rules={[{ required: true, message: "Informe o nome do curso" }]}
          >
            <Input placeholder="Ex: Informática" maxLength={100} />
          </Form.Item>
          <Form.Item
            name="descricao"
            label="Descrição"
            rules={[{ required: true, message: "Informe a descrição" }]}
          >
            <Input.TextArea
              placeholder="Descreva o curso..."
              rows={3}
              maxLength={500}
              showCount
            />
          </Form.Item>
        </Form>
      </FormModal>
    </div>
  );
}
