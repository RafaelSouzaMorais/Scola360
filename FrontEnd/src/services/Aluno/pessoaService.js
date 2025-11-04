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

/**
 * Busca uma pessoa pelo CPF usando o endpoint POST `/api/pessoas/cpf`.
 * Envia no body o JSON { cpf: "12345678901" } e espera um único objeto
 * de pessoa no retorno. Retorna `null` se não encontrado.
 * @param {string} cpf - CPF (apenas dígitos ou formatado)
 * @returns {Promise<Object|null>} Objeto pessoa ou null se não encontrado
 */
export async function buscarPessoaPorCpf(cpf) {
  if (!cpf) return null;
  const onlyDigits = String(cpf).replace(/\D/g, "");
  try {
    const { data } = await api.post(`/api/pessoas/cpf`, { cpf: onlyDigits });
    return data || null;
  } catch (err) {
    console.warn("buscarPessoaPorCpf: erro ao consultar /api/pessoas/cpf", err);
    return null;
  }
}

export async function associarResponsavelAluno(alunoId, responsavelId) {
  // POST para /api/alunos/{alunoId}/responsavel/{responsavelId} ou similar
  const { data } = await api.put(
    `/api/alunos/${alunoId}/responsavel/${responsavelId}`
  );
  console.log("Responsável associado ao aluno:", data);
  return data;
}
