import api from "../api";

/**
 * Serviço de API para Funcionários
 * Contém as chamadas HTTP usadas pela UI para listar, criar, atualizar e deletar funcionários.
 * Todas as funções retornam os dados do body (`response.data`) quando aplicável.
 */

/**
 * Lista funcionários. Se `nome` for informado, adiciona como parâmetro de query para busca no servidor.
 * @param {string} [nome] - Opcional: termo de busca por nome
 * @returns {Promise<Array>} Lista de funcionários
 */
export async function listarFuncionarios(nome) {
  const params = {};
  if (nome) params.nome = nome;
  // Log para facilitar debugging local
  console.log("listando funcionarios com params:", params);
  const { data } = await api.get("/api/funcionarios", { params });
  return data;
}

/**
 * Recupera um funcionário pelo ID.
 * @param {string} id - ID do funcionário
 * @returns {Promise<Object>} Dados do funcionário
 */
export async function buscarFuncionarioPorId(id) {
  console.log("buscando funcionario com id:", id);
  const { data } = await api.get(`/api/funcionarios/${id}`);
  return data;
}

/**
 * Cria um novo funcionário.
 * @param {Object} payload - Objeto com os campos do funcionário
 * @returns {Promise<Object>} Recurso criado (retorno da API)
 */
export async function criarFuncionario(payload) {
  console.log("criando funcionario com payload:", payload);
  const { data } = await api.post("/api/funcionarios", payload);
  return data;
}

/**
 * Atualiza um funcionário existente.
 * @param {string} id - ID do funcionário
 * @param {Object} payload - Campos a atualizar
 * @returns {Promise<Object>} Recurso atualizado (retorno da API)
 */
export async function atualizarFuncionario(id, payload) {
  console.log("atualizando funcionario id:", id, "payload:", payload);
  const { data } = await api.put(`/api/funcionarios/${id}`, payload);
  return data;
}

/**
 * Deleta um funcionário pelo ID.
 * @param {string} id - ID do funcionário
 * @returns {Promise<void>} Sem conteúdo no retorno
 */
export async function deletarFuncionario(id) {
  console.log("deletando funcionario com id:", id);
  await api.delete(`/api/funcionarios/${id}`);
}

/**
 * Busca professores ativos para dropdown.
 * @returns {Promise<Array>} Lista de professores com id e nomeCompleto
 */
export async function buscarProfessoresDropdown() {
  const { data } = await api.get("/api/funcionarios/professores/dropdown");
  return data;
}
