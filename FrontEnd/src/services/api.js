import axios from "axios";
import { jwtDecode } from "jwt-decode";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || "https://localhost:7009",
  timeout: 15000,
});

// Log para debug - remover em produção
console.log("🔧 API Base URL:", api.defaults.baseURL);

// Função auxiliar para verificar token (evita importação circular)
function isTokenExpiredLocal(token) {
  try {
    const decoded = jwtDecode(token);
    if (!decoded.exp) return false;
    const currentTime = Date.now() / 1000;
    return decoded.exp < currentTime + 30;
  } catch {
    return true;
  }
}

// Helpers
const isAuthEndpoint = (url = "") =>
  url.includes("/auth/login") || url.includes("/auth/me");

const redirectToLoginSafely = () => {
  if (window.location.pathname !== "/login") {
    window.location.href = "/login";
  }
};

// Intercepta requisições para adicionar token e validar expiração
api.interceptors.request.use((config) => {
  console.log("📤 Requisição:", config.method?.toUpperCase(), config.url);

  const token = localStorage.getItem("auth_token");

  if (token) {
    // Verifica se o token está expirado antes de enviar a requisição
    if (isTokenExpiredLocal(token)) {
      // Limpa sessão, mas NÃO redireciona aqui para evitar loops; deixa a resposta tratar
      localStorage.removeItem("auth_token");
      localStorage.removeItem("auth_user");
    } else {
      config.headers.Authorization = `Bearer ${token}`;
    }
  }

  return config;
});

// Intercepta respostas para tratar erros comuns
api.interceptors.response.use(
  (response) => {
    console.log("✅ Resposta:", response.status, response.config.url);
    return response;
  },
  (error) => {
    console.error("❌ Erro na requisição:", {
      url: error.config?.url,
      status: error.response?.status,
      message: error.message,
      data: error.response?.data,
    });

    if (error.response) {
      const status = error.response.status;
      const url = error.config?.url || "";
      // 401/403 indica sessão expirada ou sem permissão
      if ([401, 403].includes(status)) {
        // Não redireciona em endpoints de autenticação ou quando já está no /login
        if (!isAuthEndpoint(url)) {
          localStorage.removeItem("auth_token");
          localStorage.removeItem("auth_user");
          redirectToLoginSafely();
        }
      }
    }
    return Promise.reject(error);
  }
);

export default api;
