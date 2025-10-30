import api from "../api";

export async function listarDisciplinas() {
  // GET para /api/disciplinas
  const { data } = await api.get("/api/disciplinas");
  //salva no log so em debug
  console.log("Disciplinas carregadas:", data);
  return data;
}

export async function criarDisciplina(disciplina) {
  // POST para /api/disciplinas
  const { data } = await api.post("/api/disciplinas", disciplina);
  return data;
}
export async function atualizarDisciplina(disciplina) {
  // PUT para /api/disciplinas/:id
  const { data } = await api.put("/api/disciplinas", disciplina);
  return data;
}
export async function excluirDisciplina(id) {
  // DELETE para /api/disciplinas/:id
  const { data } = await api.delete(`/api/disciplinas/${id}`);
  return data;
}