import { useState } from "react";
import { App } from "antd";

/**
 * Hook customizado para operações de API com feedback automático
 * Gerencia loading e exibe mensagens de sucesso/erro automaticamente
 *
 * @returns {Object} - { executeOperation, loading }
 *
 * @example
 * const { executeOperation, loading } = useApiOperation();
 *
 * await executeOperation({
 *   operation: () => criarTurma(payload),
 *   successMessage: "Turma criada com sucesso",
 *   errorMessage: "Erro ao criar turma",
 *   onSuccess: () => fetchTurmas(),
 * });
 */
export function useApiOperation() {
  const [loading, setLoading] = useState(false);
  const { message } = App.useApp();

  /**
   * Executa uma operação de API com tratamento automático de loading e mensagens
   * @param {Object} config - Configuração da operação
   * @param {Function} config.operation - Função assíncrona que executa a operação
   * @param {string} config.successMessage - Mensagem exibida em caso de sucesso
   * @param {string} config.errorMessage - Mensagem exibida em caso de erro (opcional)
   * @param {Function} config.onSuccess - Callback executado após sucesso (opcional)
   * @param {Function} config.onError - Callback executado após erro (opcional)
   * @param {boolean} config.showSuccessMessage - Se deve exibir mensagem de sucesso (padrão: true)
   * @param {boolean} config.showErrorMessage - Se deve exibir mensagem de erro (padrão: true)
   * @returns {Promise<{success: boolean, data?: any, error?: any}>}
   */
  const executeOperation = async ({
    operation,
    successMessage,
    errorMessage,
    onSuccess,
    onError,
    showSuccessMessage = true,
    showErrorMessage = true,
  }) => {
    console.log("🔄 executeOperation iniciada:", {
      successMessage,
      errorMessage,
    });
    setLoading(true);
    try {
      const result = await operation();
      console.log("✅ Operação bem-sucedida:", result);

      if (showSuccessMessage && successMessage) {
        console.log("📢 Exibindo mensagem de sucesso:", successMessage);
        message.success(successMessage);
      }

      if (onSuccess) {
        console.log("🎯 Executando callback onSuccess");
        await onSuccess(result);
      }

      return { success: true, data: result };
    } catch (error) {
      console.error("❌ Erro na operação:", error);

      if (showErrorMessage) {
        // Tenta extrair mensagem de erro da resposta da API
        const apiErrorMessage =
          error?.response?.data?.error ||
          error?.response?.data?.message ||
          error?.message;

        const finalErrorMessage =
          apiErrorMessage || errorMessage || "Erro ao executar operação";
        console.log("📢 Exibindo mensagem de erro:", finalErrorMessage);
        message.error(finalErrorMessage);
      }

      if (onError) {
        console.log("🎯 Executando callback onError");
        await onError(error);
      }

      return { success: false, error };
    } finally {
      console.log("🏁 executeOperation finalizada, setLoading(false)");
      setLoading(false);
    }
  };

  return { executeOperation, loading };
}

/**
 * Hook para operações de listagem/busca (GET)
 * Simplifica operações que apenas carregam dados
 */
export function useFetchOperation() {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState(null);
  const [error, setError] = useState(null);
  const { message } = App.useApp();

  const fetchData = async ({
    operation,
    onSuccess,
    onError,
    showErrorMessage = true,
    errorMessage = "Erro ao carregar dados",
  }) => {
    setLoading(true);
    setError(null);
    try {
      const result = await operation();
      setData(result);

      if (onSuccess) {
        await onSuccess(result);
      }

      return { success: true, data: result };
    } catch (err) {
      console.error("Erro ao buscar dados:", err);
      setError(err);

      if (showErrorMessage) {
        const apiErrorMessage =
          err?.response?.data?.error ||
          err?.response?.data?.message ||
          err?.message;

        const finalErrorMessage = apiErrorMessage || errorMessage;
        message.error(finalErrorMessage);
      }

      if (onError) {
        await onError(err);
      }

      return { success: false, error: err };
    } finally {
      setLoading(false);
    }
  };

  return { fetchData, loading, data, error };
}
