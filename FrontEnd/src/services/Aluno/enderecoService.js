import api from "../api";

// Endereços aninhados por pessoa (aluno)
export async function cadastrarEnderecoPessoa(pessoaId, endereco) {
  // POST para /api/pessoas/{pessoaId}/enderecos
  const { data } = await api.post(
    `/api/pessoas/${pessoaId}/enderecos`,
    endereco
  );
  console.log("Endereço cadastrado:", data);
  return data;
}

export async function atualizarEnderecoPessoa(pessoaId, enderecoId, endereco) {
  // PUT para /api/pessoas/{pessoaId}/enderecos/{enderecoId}
  const { data } = await api.put(
    `/api/pessoas/${pessoaId}/enderecos/${enderecoId}`,
    endereco
  );
  console.log("Endereço atualizado:", data);
  return data;
}
