import api from "../api";

/**
 * Serviço de API para Turmas
 * Contém as chamadas HTTP usadas pela UI para gerenciar turmas.
 */

/**
 * Lista todas as turmas.
 * @returns {Promise<Array>} Lista de turmas
 */
export async function listarTurmas(curriculoId) {
  const { data } = await api.get(
    `/api/turmas/by-curso-curriculo?curriculo=${curriculoId}`
  );
  return data;
}

/**
 * Cria uma nova turma.
 * @param {Object} payload - Dados da turma (periodoId, curriculoId, codigoTurma, capacidadeMaxima, turmasDisciplinas)
 * @returns {Promise<Object>} Turma criada
 */
export async function criarTurma(payload) {
  const { data } = await api.post("/api/turmas", payload);
  return data;
}

/**
 * Atualiza uma turma existente.
 * @param {Object} payload - Dados da turma incluindo o id
 * @returns {Promise<Object>} Turma atualizada
 */
export async function atualizarTurma(payload) {
  console.log("Atualizando turma com payload:", payload);
  const { data } = await api.put("/api/turmas", payload);
  return data;
}

/**
 * Deleta uma turma pelo ID.
 * @param {string} id - ID da turma
 * @returns {Promise<void>}
 */
export async function deletarTurma(id) {
  await api.delete(`/api/turmas/${id}`);
}

/** * Lista as disciplinas associadas a uma turma.
 * @param {string} id - ID da turma
 * @returns {Promise<Array>} Lista de disciplinas da turma
 */
export async function listarDisciplinasTurmaByIdTurma(id) {
  try {
    const { data } = await api.get(`/api/turmas/${id}/disciplinas`);
    console.log("Disciplinas da turma", id, ":", data);
    if (!data) {
      console.warn("Nenhuma disciplina encontrada para a turma com id:", id);
      return [];
    }
    return data;
  } catch (error) {
    return [];
  }
}

/**
 * Deleta uma TurmaDisciplina pelo ID.
 * @param {string} id - ID da TurmaDisciplina
 * @returns {Promise<void>}
 */
export async function deletarTurmaDisciplina(id) {
  await api.delete(`/api/turmas/disciplina/${id}`);
}
