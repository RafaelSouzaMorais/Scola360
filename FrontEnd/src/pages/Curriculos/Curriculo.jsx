import React, { useEffect, useMemo, useState } from "react";
import {
  Table,
  Button,
  Space,
  Popconfirm,
  message,
  Form,
  Input,
  Select,
  Row,
  Col,
  Card,
} from "antd";
import PageHeader from "../../components/PageHeader";
import SearchFilters from "../../components/SearchFilters";
import FormModal from "../../components/FormModal";
import RowStatusLegend from "../../components/RowStatusLegend";
import {
  listarCurriculos,
  buscarCurriculoPorId,
  criarCurriculo,
  atualizarCurriculo,
  deletarCurriculo,
  listarGrade,
  adicionarGradeEmLote,
  substituirGradeEmLote,
} from "../../services/Curriculo/curriculoService";
import { listarDisciplinas } from "../../services/Disciplina/disciplinaService";
import { listarCursos } from "../../services/Curso/cursoService";

export default function Curriculo() {
  // Lista de currículos
  const [curriculos, setCurriculos] = useState([]);
  const [loading, setLoading] = useState(false);
  const [filtroNome, setFiltroNome] = useState("");

  // Detalhes - grade dentro do modal
  const [disciplinas, setDisciplinas] = useState([]);
  const [cursos, setCursos] = useState([]);
  const [grade, setGrade] = useState([]); // [{id?, disciplinaId, isNew, isEdited}]

  // Modal de criar/editar currículo (nome + grade)
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState("create");
  const [editingId, setEditingId] = useState(null);
  const [formCurriculo] = Form.useForm();
  const [saveModalLoading, setSaveModalLoading] = useState(false);

  const fetchCurriculos = async () => {
    setLoading(true);
    try {
      const data = await listarCurriculos();
      setCurriculos(Array.isArray(data) ? data : []);
    } catch (err) {
      message.error("Erro ao carregar currículos");
    } finally {
      setLoading(false);
    }
  };

  const fetchDisciplinas = async () => {
    try {
      const data = await listarDisciplinas();
      const opts = (Array.isArray(data) ? data : []).map((d) => ({
        label: `${d.codigo ? d.codigo + " - " : ""}${d.nome}`,
        value: d.id,
      }));
      setDisciplinas(opts);
    } catch (_) {}
  };

  const fetchCursos = async () => {
    try {
      const data = await listarCursos();
      const opts = (Array.isArray(data) ? data : []).map((c) => ({
        label: c.nome,
        value: c.id,
      }));
      setCursos(opts);
    } catch (_) {}
  };

  useEffect(() => {
    fetchCurriculos();
    fetchDisciplinas();
    fetchCursos();
  }, []);

  const curriculosFiltrados = useMemo(() => {
    return curriculos.filter((c) =>
      c.nome?.toLowerCase().includes(filtroNome.toLowerCase())
    );
  }, [curriculos, filtroNome]);

  const handleNovo = () => {
    // Abre popup para criar novo currículo (como em Períodos)
    setModalMode("create");
    setEditingId(null);
    formCurriculo.resetFields();
    setGrade([]);
    setModalOpen(true);
  };

  const handleEditarCurriculo = async (record) => {
    setModalMode("edit");
    setEditingId(record.id);

    try {
      const data = await buscarCurriculoPorId(record.id);
      formCurriculo.setFieldsValue({
        nome: data.nome,
        cursoId: data.cursoId,
        descricao: data.descricao,
      });

      // Carrega a grade
      try {
        const gradeData = await listarGrade(record.id);
        const linhas = (Array.isArray(gradeData) ? gradeData : []).map((g) => ({
          id: g.id,
          disciplinaId: g.disciplinaId || g.idDisciplina || g.disciplina?.id,
          isNew: false,
          isEdited: false,
        }));
        setGrade(linhas);
      } catch (_) {
        const linhas = (data.grade || data.disciplinas || []).map((g) => ({
          id: g.id,
          disciplinaId: g.disciplinaId || g.idDisciplina || g.disciplina?.id,
          isNew: false,
          isEdited: false,
        }));
        setGrade(linhas);
      }
    } catch (err) {
      message.error("Erro ao carregar currículo");
    }

    setModalOpen(true);
  };

  const carregarDetalhes = async (id) => {
    try {
      const data = await buscarCurriculoPorId(id);
      setEditingId(id);
      form.setFieldsValue({ nome: data.nome || "" });
      try {
        const gradeData = await listarGrade(id);
        const linhas = (Array.isArray(gradeData) ? gradeData : []).map((g) => ({
          id: g.id,
          disciplinaId: g.disciplinaId || g.idDisciplina || g.disciplina?.id,
          isNew: false,
          isEdited: false,
        }));
        setGrade(linhas);
      } catch (_) {
        const linhas = (data.grade || data.disciplinas || []).map((g) => ({
          id: g.id,
          disciplinaId: g.disciplinaId || g.idDisciplina || g.disciplina?.id,
          isNew: false,
          isEdited: false,
        }));
        setGrade(linhas);
      }
    } catch (err) {
      message.error("Erro ao carregar currículo");
    }
  };

  const addLinha = () => {
    setGrade((rows) => [
      ...rows,
      {
        tempKey: crypto.randomUUID?.() || Math.random().toString(36),
        disciplinaId: null,
        isNew: true,
        isEdited: false,
      },
    ]);
  };

  const removeLinha = (row, index) => {
    setGrade((rows) => rows.filter((_, i) => i !== index));
  };

  const onChangeDisciplina = (value, index) => {
    setGrade((rows) =>
      rows.map((r, i) =>
        i === index
          ? { ...r, disciplinaId: value, isEdited: r.isNew ? r.isNew : true }
          : r
      )
    );
  };

  const handleCloseModal = () => {
    setModalOpen(false);
    formCurriculo.resetFields();
    setGrade([]);
    setEditingId(null);
  };

  const handleSaveCurriculo = async () => {
    try {
      setSaveModalLoading(true);
      const values = await formCurriculo.validateFields();

      const disciplinaIds = grade
        .filter((g) => g.disciplinaId)
        .map((g) => g.disciplinaId);

      if (modalMode === "create") {
        const created = await criarCurriculo({
          nome: values.nome,
          cursoId: values.cursoId,
          descricao: values.descricao,
        });
        const novoId = created?.id;

        if (novoId && disciplinaIds.length) {
          await adicionarGradeEmLote(novoId, disciplinaIds);
        }

        message.success("Currículo criado com sucesso!");
        setModalOpen(false);
        fetchCurriculos();
        handleCloseModal();
      } else {
        await atualizarCurriculo({
          id: editingId,
          nome: values.nome,
          cursoId: values.cursoId,
          descricao: values.descricao,
        });

        await substituirGradeEmLote(editingId, disciplinaIds);

        message.success("Currículo atualizado com sucesso!");
        setModalOpen(false);
        fetchCurriculos();
        handleCloseModal();
      }
    } catch (err) {
      if (err?.errorFields) return;
      const apiMsg = err?.response?.data?.error || err?.response?.data?.message;
      message.error(apiMsg || "Erro ao salvar currículo");
    } finally {
      setSaveModalLoading(false);
    }
  };

  // Colunas da tabela de currículos
  const columns = [
    { title: "Nome", dataIndex: "nome", key: "nome" },
    {
      title: "Ações",
      key: "acoes",
      width: 140,
      render: (_, record) => (
        <Space>
          <Button
            size="small"
            type="link"
            onClick={(e) => {
              e.stopPropagation();
              handleEditarCurriculo(record);
            }}
          >
            Editar
          </Button>
          <Popconfirm
            title="Excluir currículo?"
            onConfirm={async (e) => {
              e?.stopPropagation?.();
              await deletarCurriculo(record.id);
              message.success("Excluído");
              if (editingId === record.id) {
                setEditingId(null);
                form.resetFields();
                setGrade([]);
              }
              fetchCurriculos();
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

  // Colunas da tabela de grade curricular
  const gradeColumns = [
    {
      title: "Disciplina",
      dataIndex: "disciplinaId",
      key: "disciplinaId",
      render: (_, record, index) => (
        <Select
          showSearch
          placeholder="Selecione a disciplina"
          options={disciplinas}
          value={grade[index].disciplinaId}
          onChange={(v) => onChangeDisciplina(v, index)}
          style={{ width: "100%" }}
          optionFilterProp="label"
          filterOption={(input, option) =>
            (option?.label ?? "").toLowerCase().includes(input.toLowerCase())
          }
        />
      ),
    },
    {
      title: "Ações",
      key: "acoes",
      width: 100,
      render: (_, record, index) => (
        <Button
          danger
          type="link"
          size="small"
          onClick={() => removeLinha(record, index)}
        >
          Remover
        </Button>
      ),
    },
  ];

  return (
    <div className="max-w-6xl w-full mx-auto bg-white p-2 sm:p-4 md:p-6 rounded shadow-md mt-4 md:mt-6">
      <PageHeader
        title="Currículos"
        buttonText="Novo Currículo"
        onButtonClick={handleNovo}
      />

      <SearchFilters
        onSearch={() => {}}
        onClear={() => setFiltroNome("")}
        onReset={() => {
          setFiltroNome("");
          fetchCurriculos();
        }}
        showResetButton
        loading={loading}
      >
        <Input
          placeholder="Pesquisar por nome"
          value={filtroNome}
          onChange={(e) => setFiltroNome(e.target.value)}
          allowClear
        />
      </SearchFilters>

      <Row gutter={[16, 16]}>
        <Col xs={24}>
          <Table
            columns={columns}
            dataSource={curriculosFiltrados}
            rowKey="id"
            loading={loading}
            pagination={{ pageSize: 10 }}
          />
        </Col>
      </Row>

      <FormModal
        open={modalOpen}
        onClose={handleCloseModal}
        onSave={handleSaveCurriculo}
        mode={modalMode}
        loading={saveModalLoading}
        title={modalMode === "edit" ? "Editar Currículo" : "Novo Currículo"}
        width={900}
      >
        <Form form={formCurriculo} layout="vertical" requiredMark={false}>
          <Row gutter={16}>
            <Col xs={24} md={12}>
              <Form.Item
                name="nome"
                label="Nome do Currículo"
                rules={[
                  { required: true, message: "Informe o nome do currículo" },
                ]}
              >
                <Input placeholder="Ex: Currículo 2025/1" maxLength={150} />
              </Form.Item>
            </Col>
            <Col xs={24} md={12}>
              <Form.Item
                name="cursoId"
                label="Curso"
                rules={[{ required: true, message: "Selecione o curso" }]}
              >
                <Select
                  showSearch
                  placeholder="Selecione o curso"
                  options={cursos}
                  optionFilterProp="label"
                  filterOption={(input, option) =>
                    (option?.label ?? "")
                      .toLowerCase()
                      .includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
          </Row>

          <Form.Item name="descricao" label="Descrição">
            <Input.TextArea
              placeholder="Descrição do currículo..."
              rows={2}
              maxLength={500}
              showCount
            />
          </Form.Item>
        </Form>

        <div style={{ marginTop: 24, marginBottom: 12 }}>
          <h4 style={{ marginBottom: 8 }}>Grade Curricular</h4>
          <RowStatusLegend />
        </div>

        <div style={{ marginBottom: 8 }}>
          <Button onClick={addLinha}>Adicionar Disciplina</Button>
        </div>

        <Table
          dataSource={grade.map((g, index) => ({
            ...g,
            key: g.id || g.tempKey || index,
          }))}
          pagination={{ pageSize: 10 }}
          size="small"
          rowClassName={(_, index) =>
            grade[index].isNew
              ? "row-new"
              : grade[index].isEdited
              ? "row-edited"
              : ""
          }
          columns={gradeColumns}
        />
      </FormModal>
    </div>
  );
}
