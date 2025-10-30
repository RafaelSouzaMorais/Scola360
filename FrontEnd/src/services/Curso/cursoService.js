import api from "../api";

export async function listarCursos() {
  const { data } = await api.get("/api/cursos");
  return data;
}

export async function buscarCursoPorId(id) {
  const { data } = await api.get(`/api/cursos/${id}`);
  return data;
}

export async function criarCurso(payload) {
  const { data } = await api.post("/api/cursos", payload);
  return data;
}

export async function atualizarCurso(payload) {
  const { data } = await api.put(`/api/cursos`, payload);
  return data;
}

export async function deletarCurso(id) {
  await api.delete(`/api/cursos/${id}`);
}
