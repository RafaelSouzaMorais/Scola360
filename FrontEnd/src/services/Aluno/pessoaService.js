import api from "../api";

// Responsável (Pessoa)
export async function cadastrarPessoa(pessoa) {
  // POST para /api/pessoas
  const { data } = await api.post("/api/pessoas", pessoa);
  console.log("Pessoa cadastrada:", data);
  return data;
}

export async function atualizarPessoa(pessoaId, pessoa) {
  // PUT para /api/pessoas/{id}
  const { data } = await api.put(`/api/pessoas/${pessoaId}`, pessoa);
  console.log("Pessoa atualizada:", data);
  return data;
}

export async function associarResponsavelAluno(alunoId, responsavelId) {
  // POST para /api/alunos/{alunoId}/responsavel/{responsavelId} ou similar
  const { data } = await api.put(
    `/api/alunos/${alunoId}/responsavel/${responsavelId}`
  );
  console.log("Responsável associado ao aluno:", data);
  return data;
}
