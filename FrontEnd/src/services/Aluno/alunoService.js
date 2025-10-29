import api from "../api";

export async function cadastrarAluno(aluno) {
  // POST para /api/alunos
  const { data } = await api.post("/api/alunos", aluno);
  console.log("Aluno cadastrado:", data);
  return data;
}

export async function listarAlunos() {
  const { data } = await api.get("/api/alunos");
  console.log("Alunos listados:", data);
  return data;
}

export async function deletarAluno(id) {
  await api.delete(`/api/alunos/${id}`);
}

// Associar responsável ao aluno (se necessário endpoint específico)
export async function associarResponsavelAluno(alunoId, responsavelId) {
  // POST para /api/alunos/{alunoId}/responsavel/{responsavelId} ou similar
  const { data } = await api.put(
    `/api/alunos/${alunoId}/responsavel/${responsavelId}`
  );
  console.log("Responsável associado ao aluno:", data);
  return data;
}
