import api from "../api";

export async function listarPeriodos() {
  const { data } = await api.get("/api/periodos");
  return data;
}

export async function buscarPeriodoPorId(id) {
  const { data } = await api.get(`/api/periodos/${id}`);
  return data;
}

export async function criarPeriodo(payload) {
  const { data } = await api.post("/api/periodos", payload);
  return data;
}

export async function atualizarPeriodo(payload) {
  const { data } = await api.put(`/api/periodos`, payload);
  return data;
}

export async function deletarPeriodo(id) {
  await api.delete(`/api/periodos/${id}`);
}
