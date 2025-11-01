import api from "../api";

export async function listarCurriculos() {
  const { data } = await api.get("/api/curriculos");
  return data;
}

export async function buscarCurriculoPorId(id) {
  const { data } = await api.get(`/api/curriculos/${id}`);
  return data;
}

export async function criarCurriculo(payload) {
  const { data } = await api.post("/api/curriculos", payload);
  return data;
}

export async function atualizarCurriculo(payload) {
  const { data } = await api.put(`/api/curriculos`, payload);
  return data;
}

export async function deletarCurriculo(id) {
  await api.delete(`/api/curriculos/${id}`);
}

// ---- Grade Curricular ----
export async function listarGrade(curriculoId) {
  const { data } = await api.get(`/api/curriculos/${curriculoId}/grade`);
  return data;
}

export async function adicionarDisciplinaNoCurriculo(
  curriculoId,
  disciplinaId
) {
  const { data } = await api.post(`/api/curriculos/${curriculoId}/grade`, {
    disciplinaId,
  });
  return data;
}

export async function removerDisciplinaDoCurriculo(curriculoId, disciplinaId) {
  await api.delete(`/api/curriculos/${curriculoId}/grade/${disciplinaId}`);
}

export async function adicionarGradeEmLote(curriculoId, disciplinaIds) {
  const { data } = await api.post(`/api/curriculos/${curriculoId}/grade/lote`, {
    disciplinaIds,
  });
  return data;
}

export async function substituirGradeEmLote(curriculoId, disciplinaIds) {
  const { data } = await api.put(`/api/curriculos/${curriculoId}/grade/lote`, {
    disciplinaIds,
  });
  return data;
}
