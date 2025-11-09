import React, { useState, useEffect } from "react";
import {
  Table,
  Button,
  Space,
  message,
  Form,
  Input,
  Select,
  Popconfirm,
  InputNumber,
  App,
} from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import PageHeader from "../../components/PageHeader";
import SearchFilters from "../../components/SearchFilters";
import FormModal from "../../components/FormModal";
import {
  useApiOperation,
  useFetchOperation,
} from "../../hooks/useApiOperation";
import {
  listarTurmas,
  criarTurma,
  atualizarTurma,
  deletarTurma,
  listarDisciplinasTurmaByIdTurma,
  deletarTurmaDisciplina,
} from "../../services/Turma/turmaService";
import { listarCursos } from "../../services/Curso/cursoService";
import { listarCurriculosPorCurso } from "../../services/Curriculo/curriculoService";
import { listarPeriodos } from "../../services/Periodo/periodoService";
import { buscarProfessoresDropdown } from "../../services/Funcionario/funcionarioService";
import { buscarDisciplinasPorCurriculoDropdown } from "../../services/Disciplina/disciplinaService";
import { useEnums } from "../../contexts/EnumContext";

export default function Turmas() {
  const { modal } = App.useApp();
  const { turno } = useEnums();
  const { executeOperation, loading: saveLoading } = useApiOperation();
  const { fetchData: fetchListData, loading: loadingLists } =
    useFetchOperation();
  const { fetchData: fetchTurmasData, loading: loadingTurmas } =
    useFetchOperation();

  // Debug: monitora mudanças no loading
  useEffect(() => {
    console.log("🔄 saveLoading mudou:", saveLoading);
  }, [saveLoading]);

  // Listas e filtros
  const [turmas, setTurmas] = useState([]);
  const [cursos, setCursos] = useState([]);
  const [curriculos, setCurriculos] = useState([]);
  const [periodos, setPeriodos] = useState([]);
  const [professores, setProfessores] = useState([]);
  const [disciplinas, setDisciplinas] = useState([]);

  // Filtros
  const [filtroCursoId, setFiltroCursoId] = useState(null);
  const [filtroCurriculoId, setFiltroCurriculoId] = useState(null);

  // Modal
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState("create");
  const [editingId, setEditingId] = useState(null);
  const [form] = Form.useForm();

  // Disciplinas da turma (tabela interna)
  const [turmasDisciplinas, setTurmasDisciplinas] = useState([]);

  // Carrega dados iniciais
  useEffect(() => {
    fetchCursos();
    fetchPeriodos();
    fetchProfessores();
  }, []);

  const fetchCursos = async () => {
    await fetchListData({
      operation: listarCursos,
      errorMessage: "Erro ao carregar cursos",
      onSuccess: (data) => setCursos(Array.isArray(data) ? data : []),
    });
  };

  const fetchPeriodos = async () => {
    await fetchListData({
      operation: listarPeriodos,
      errorMessage: "Erro ao carregar períodos",
      onSuccess: (data) => setPeriodos(Array.isArray(data) ? data : []),
    });
  };

  const fetchProfessores = async () => {
    await fetchListData({
      operation: buscarProfessoresDropdown,
      errorMessage: "Erro ao carregar professores",
      onSuccess: (data) => setProfessores(Array.isArray(data) ? data : []),
    });
  };

  const fetchCurriculosPorCurso = async (cursoId) => {
    if (!cursoId) {
      setCurriculos([]);
      return;
    }
    await fetchListData({
      operation: () => listarCurriculosPorCurso(cursoId),
      errorMessage: "Erro ao carregar currículos",
      onSuccess: (data) => setCurriculos(Array.isArray(data) ? data : []),
    });
  };

  const fetchDisciplinasPorCurriculo = async (curriculoId) => {
    if (!curriculoId) {
      setDisciplinas([]);
      return;
    }
    await fetchListData({
      operation: () => buscarDisciplinasPorCurriculoDropdown(curriculoId),
      errorMessage: "Erro ao carregar disciplinas",
      onSuccess: (data) => setDisciplinas(Array.isArray(data) ? data : []),
    });
  };

  const fetchTurmas = async () => {
    await fetchTurmasData({
      operation: () => listarTurmas(filtroCurriculoId),
      errorMessage: "Erro ao carregar turmas",
      onSuccess: (data) => {
        // Filtra turmas pelo curso e currículo selecionados
        let filtered = Array.isArray(data) ? data : [];
        if (filtroCursoId && filtroCurriculoId) {
          filtered = filtered.filter(
            (t) => t.curriculoId === filtroCurriculoId
          );
        }
        setTurmas(filtered);
      },
    });
  };

  const handleCursoChange = (cursoId) => {
    setFiltroCursoId(cursoId);
    setFiltroCurriculoId(null);
    setTurmas([]); // Limpa a tabela
    if (cursoId) {
      fetchCurriculosPorCurso(cursoId);
    } else {
      setCurriculos([]);
    }
  };

  const handleCurriculoChange = (curriculoId) => {
    setFiltroCurriculoId(curriculoId);
    setTurmas([]); // Limpa a tabela
  };

  const handleSearch = () => {
    if (!filtroCursoId || !filtroCurriculoId) {
      modal.warning({
        title: "Filtros Obrigatórios",
        content: "Selecione o curso e o currículo para buscar",
        okText: "Ok",
      });
      return;
    }
    fetchTurmas();
  };

  const handleClearFilters = () => {
    setFiltroCursoId(null);
    setFiltroCurriculoId(null);
    setCurriculos([]);
    setTurmas([]);
  };

  const handleNovo = async () => {
    if (!filtroCursoId || !filtroCurriculoId) {
      modal.warning({
        title: "Filtros Obrigatórios",
        content: "Selecione o curso e o currículo antes de criar uma turma",
        okText: "Ok",
      });
      return;
    }
    setModalMode("create");
    setEditingId(null);
    setTurmasDisciplinas([]);
    form.resetFields();

    // Tenta carregar disciplinas, mas abre o modal mesmo se falhar
    try {
      await fetchDisciplinasPorCurriculo(filtroCurriculoId);
    } catch (error) {
      console.warn(
        "Erro ao carregar disciplinas, mas abrindo modal mesmo assim:",
        error
      );
      setDisciplinas([]); // Garante que a lista fique vazia
    }

    setModalOpen(true);
  };

  const handleEditar = async (record) => {
    setModalMode("edit");
    setEditingId(record.id);

    // Carrega currículos e disciplinas do curso da turma
    const curriculo = curriculos.find((c) => c.id === record.curriculoId);
    if (curriculo) {
      try {
        await fetchCurriculosPorCurso(curriculo.cursoId);
        await fetchDisciplinasPorCurriculo(record.curriculoId);
      } catch (error) {
        console.warn(
          "Erro ao carregar currículos/disciplinas para edição, continuando mesmo assim:",
          error
        );
        setDisciplinas([]); // Garante que a lista fique vazia
      }
    }

    form.setFieldsValue({
      periodoId: record.periodoId || null,
      turno: record.turno ?? null,
      codigoTurma: record.codigoTurma || "",
      capacidadeMaxima: record.capacidadeMaxima || 30,
    });

    console.log("Record para editar:", record);

    // Carrega disciplinas da turma (já tem tratamento de erro na função)
    const disciplinas = await listarDisciplinasTurmaByIdTurma(record.id);

    setTurmasDisciplinas(disciplinas);
    setModalOpen(true);
  };

  const handleDelete = async (id) => {
    await executeOperation({
      operation: () => deletarTurma(id),
      successMessage: "Turma excluída com sucesso",
      errorMessage: "Erro ao excluir turma",
      onSuccess: fetchTurmas,
    });
  };

  const handleSave = async () => {
    try {
      const values = await form.validateFields();

      if (turmasDisciplinas.filter((td) => !td.deleted).length === 0) {
        modal.warning({
          title: "Disciplinas Obrigatórias",
          content: "Adicione pelo menos uma disciplina à turma",
          okText: "Ok",
        });
        return;
      }

      // Primeiro, exclui as disciplinas marcadas para exclusão
      const disciplinasParaExcluir = turmasDisciplinas.filter(
        (td) => td.deleted && td.id
      );
      for (const disciplina of disciplinasParaExcluir) {
        try {
          await executeOperation({
            operation: () => deletarTurmaDisciplina(disciplina.id),
            successMessage: `Disciplina ${disciplina.disciplinaId} excluída com sucesso`,
            errorMessage: `Erro ao excluir disciplina ${disciplina.disciplinaId}`,
            showSuccessMessage: false, // Não mostra mensagem individual para não poluir
            showErrorMessage: true,
          });
        } catch (error) {
          console.error("Erro ao excluir disciplina:", error);
          // Continua com as outras exclusões mesmo se uma falhar
        }
      }

      const payload = {
        periodoId: values.periodoId,
        curriculoId: filtroCurriculoId,
        turno: values.turno,
        codigoTurma: values.codigoTurma,
        capacidadeMaxima: values.capacidadeMaxima,
        turmasDisciplinas: turmasDisciplinas
          .filter((td) => !td.deleted) // Inclui apenas as não marcadas para exclusão
          .map((td) => ({
            id: td.id || undefined, // Inclui o id se existir (ao editar)
            turmaId: editingId ? editingId : undefined,
            disciplinaId: td.disciplinaId,
            funcionarioId: td.funcionarioId,
          })),
      };

      if (modalMode === "edit") {
        payload.id = editingId;

        await executeOperation({
          operation: () => atualizarTurma(payload),
          successMessage: "Turma atualizada com sucesso",
          errorMessage: "Erro ao atualizar turma",
          onSuccess: () => {
            setModalOpen(false);
            form.resetFields();
            setTurmasDisciplinas([]);
            fetchTurmas();
          },
        });
      } else {
        await executeOperation({
          operation: () => criarTurma(payload),
          successMessage: "Turma criada com sucesso",
          errorMessage: "Erro ao criar turma",
          onSuccess: () => {
            setModalOpen(false);
            form.resetFields();
            setTurmasDisciplinas([]);
            fetchTurmas();
          },
        });
      }
    } catch (err) {
      // Erros de validação do formulário são silenciosos
      if (err?.errorFields) return;
    }
  };

  // Funções para gerenciar disciplinas da turma
  const handleAddDisciplina = () => {
    setTurmasDisciplinas([
      ...turmasDisciplinas,
      {
        key: Date.now(),
        disciplinaId: null,
        funcionarioId: null,
      },
    ]);
  };

  const handleRemoveTurmaDisciplina = async (turmaDisciplina) => {
    console.log("Removendo turmaDisciplina:", turmaDisciplina);
    await executeOperation({
      operation: () => deletarTurmaDisciplina(turmaDisciplina.id),
      successMessage: "Disciplina removida com sucesso",
      errorMessage: "Erro ao remover disciplina",
      onSuccess: () => {
        setTurmasDisciplinas(
          turmasDisciplinas.filter((td) => td.id !== turmaDisciplina.id)
        );
      },
    });
  };

  const handleDisciplinaChange = (key, field, value) => {
    setTurmasDisciplinas(
      turmasDisciplinas.map((td) =>
        td.key === key ? { ...td, [field]: value } : td
      )
    );
  };

  const columnsTurmas = [
    { title: "Código", dataIndex: "codigoTurma", key: "codigoTurma" },
    {
      title: "Turno",
      dataIndex: "turno",
      key: "turno",
      width: 120,
      render: (turnoValue) => {
        const turnoOption = turno.find((t) => t.value === turnoValue);
        return turnoOption ? turnoOption.label : "-";
      },
    },
    {
      title: "Período",
      dataIndex: "periodoId",
      key: "periodo",
      render: (periodoId) => {
        const periodo = periodos.find((p) => p.id === periodoId);
        return periodo ? periodo.nome : "-";
      },
    },
    {
      title: "Capacidade",
      dataIndex: "capacidadeMaxima",
      key: "capacidadeMaxima",
      width: 120,
    },
    {
      title: "Ações",
      key: "acoes",
      width: 150,
      render: (_, record) => (
        <Space>
          <Button type="link" size="small" onClick={() => handleEditar(record)}>
            Editar
          </Button>
          <Popconfirm
            title="Excluir turma?"
            onConfirm={() => handleDelete(record.id)}
            okText="Sim"
            cancelText="Não"
          >
            <Button type="link" size="small" danger>
              Excluir
            </Button>
          </Popconfirm>
        </Space>
      ),
    },
  ];

  const columnsDisciplinas = [
    {
      title: "Disciplina",
      dataIndex: "disciplinaId",
      key: "disciplina",
      render: (disciplinaId, record) => (
        <Select
          value={disciplinaId}
          onChange={(value) =>
            handleDisciplinaChange(record.key, "disciplinaId", value)
          }
          style={{ width: "100%" }}
          placeholder="Selecione a disciplina"
          options={disciplinas.map((d) => ({
            value: d.id,
            label: `${d.codigo} - ${d.nome}`,
          }))}
        />
      ),
    },
    {
      title: "Professor",
      dataIndex: "funcionarioId",
      key: "professor",
      render: (funcionarioId, record) => (
        <Select
          value={funcionarioId}
          onChange={(value) =>
            handleDisciplinaChange(record.key, "funcionarioId", value)
          }
          style={{ width: "100%" }}
          placeholder="Selecione o professor"
          options={professores.map((p) => ({
            value: p.id,
            label: p.nomeCompleto,
          }))}
        />
      ),
    },
    {
      title: "Ações",
      key: "acoes",
      width: 80,
      render: (_, record) => (
        <Popconfirm
          title="Excluir disciplina?"
          onConfirm={async () => await handleRemoveTurmaDisciplina(record)}
          okText="Sim"
          cancelText="Não"
        >
          <Button type="text" danger icon={<DeleteOutlined />} />
        </Popconfirm>
      ),
    },
  ];

  const cursoNome = cursos.find((c) => c.id === filtroCursoId)?.nome || "";
  const curriculoNome =
    curriculos.find((c) => c.id === filtroCurriculoId)?.nome || "";

  return (
    <div className="max-w-6xl w-full mx-auto bg-white p-2 sm:p-4 md:p-6 rounded shadow-md mt-4 md:mt-6">
      <PageHeader
        title="Turmas"
        buttonText="Nova Turma"
        onButtonClick={handleNovo}
      />

      <SearchFilters
        onSearch={handleSearch}
        onClear={handleClearFilters}
        loading={loadingLists}
      >
        <Select
          placeholder="Selecione o curso"
          value={filtroCursoId}
          onChange={handleCursoChange}
          allowClear
          style={{ width: "100%" }}
          options={cursos.map((c) => ({ value: c.id, label: c.nome }))}
        />
        <Select
          placeholder="Selecione o currículo"
          value={filtroCurriculoId}
          onChange={handleCurriculoChange}
          allowClear
          disabled={!filtroCursoId}
          style={{ width: "100%" }}
          options={curriculos.map((c) => ({ value: c.id, label: c.nome }))}
        />
      </SearchFilters>

      <Table
        columns={columnsTurmas}
        dataSource={turmas}
        rowKey="id"
        loading={loadingTurmas}
        pagination={{ pageSize: 10 }}
      />

      <FormModal
        open={modalOpen}
        onClose={() => {
          setModalOpen(false);
          form.resetFields();
          setTurmasDisciplinas([]);
        }}
        onSave={handleSave}
        mode={modalMode}
        loading={saveLoading}
        title={modalMode === "edit" ? "Editar Turma" : "Nova Turma"}
        width={900}
      >
        <Form form={form} layout="vertical" requiredMark={false}>
          <div style={{ marginBottom: 16 }}>
            <strong>Curso:</strong> {cursoNome}
            <br />
            <strong>Currículo:</strong> {curriculoNome}
          </div>

          <Form.Item
            name="periodoId"
            label="Período"
            rules={[{ required: true, message: "Selecione o período" }]}
          >
            <Select
              placeholder="Selecione o período"
              options={periodos.map((p) => ({
                value: p.id,
                label: p.nome,
              }))}
            />
          </Form.Item>

          <Form.Item
            name="turno"
            label="Turno"
            rules={[{ required: true, message: "Selecione o turno" }]}
          >
            <Select placeholder="Selecione o turno" options={turno} />
          </Form.Item>

          <Form.Item
            name="codigoTurma"
            label="Código da Turma"
            rules={[{ required: true, message: "Informe o código da turma" }]}
          >
            <Input maxLength={50} placeholder="Ex: TURMA-2024-1A" />
          </Form.Item>

          <Form.Item
            name="capacidadeMaxima"
            label="Capacidade Máxima"
            rules={[{ required: true, message: "Informe a capacidade" }]}
          >
            <InputNumber min={1} style={{ width: "100%" }} />
          </Form.Item>

          <div style={{ marginTop: 24 }}>
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                marginBottom: 8,
              }}
            >
              <strong>Disciplinas da Turma</strong>
              <Button type="primary" size="small" onClick={handleAddDisciplina}>
                Adicionar Disciplina
              </Button>
            </div>
            <Table
              columns={columnsDisciplinas}
              dataSource={turmasDisciplinas.filter((td) => !td.deleted)}
              rowKey="key"
              pagination={false}
              size="small"
              locale={{ emptyText: "Nenhuma disciplina adicionada" }}
            />
          </div>
        </Form>
      </FormModal>
    </div>
  );
}
