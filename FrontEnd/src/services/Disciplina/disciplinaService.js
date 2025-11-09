import api from "../api";

export async function listarDisciplinas() {
  // GET para /api/disciplinas
  const { data } = await api.get("/api/disciplinas");
  //salva no log so em debug
  console.log("Disciplinas carregadas:", data);
  return data;
}

/**
 * Busca uma disciplina pelo ID.
 * @param {string} id - ID da disciplina
 * @returns {Promise<Object>} Dados da disciplina
 */
export async function buscarDisciplinaPorId(id) {
  const { data } = await api.get(`/api/disciplinas/${id}`);
  return data;
}

/**
 * Cria uma nova disciplina.
 * @param {Object} disciplina - Objeto com os campos da disciplina
 * @returns {Promise<Object>} Recurso criado (retorno da API)
 */
export async function criarDisciplina(disciplina) {
  // POST para /api/disciplinas
  const { data } = await api.post("/api/disciplinas", disciplina);
  return data;
}

/**
 * Atualiza uma disciplina existente.
 * @param {Object} disciplina - Objeto com os campos da disciplina
 * @returns {Promise<Object>} Recurso atualizado (retorno da API)
 */
export async function atualizarDisciplina(disciplina) {
  // PUT para /api/disciplinas/:id
  const { data } = await api.put("/api/disciplinas", disciplina);
  return data;
}

/**
 * Exclui uma disciplina pelo ID.
 * @param {string} id - ID da disciplina
 * @returns {Promise<void>}
 */
export async function excluirDisciplina(id) {
  // DELETE para /api/disciplinas/:id
  const { data } = await api.delete(`/api/disciplinas/${id}`);
  return data;
}

/**
 * Busca disciplinas de um currículo para dropdown.
 * @param {string} curriculoId - ID do currículo
 * @returns {Promise<Array>} Lista de disciplinas com id, nome e codigo
 */
export async function buscarDisciplinasPorCurriculoDropdown(curriculoId) {
  const { data } = await api.get(
    `/api/disciplinas/curriculo/${curriculoId}/dropdown`
  );
  console.log("Disciplinas para currículo", curriculoId, ":", data);
  return data;
}
