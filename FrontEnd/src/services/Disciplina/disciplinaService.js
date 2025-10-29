import api from "../api";

export async function listarDisciplinas() {
  // GET para /api/disciplinas
  const { data } = await api.get("/api/disciplinas");
  //salva no log so em debug
  console.log("Disciplinas carregadas:", data);
  return data;
}
