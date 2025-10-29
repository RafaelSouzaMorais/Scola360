import api from "./api";
import { jwtDecode } from "jwt-decode";

// Contrato do backend .NET:
// POST /auth/login { Username, Password } -> { token, user: { id, name, email, roles } }
export async function login(username, password) {
  const { data } = await api.post("/auth/login", {
    Username: username,
    Password: password,
  });
  return data; // { token, user }
}

export async function getCurrentUser() {
  const { data } = await api.get("/auth/me");
  return data; // { id, name, email, roles }
}

export function saveSession({ token, user }) {
  localStorage.setItem("auth_token", token);
  localStorage.setItem("auth_user", JSON.stringify(user));
}

export function loadSession() {
  const token = localStorage.getItem("auth_token");
  const userRaw = localStorage.getItem("auth_user");
  const user = userRaw ? JSON.parse(userRaw) : null;

  // Verifica se o token existe e se ainda é válido
  if (token && !isTokenExpired(token)) {
    return { token, user };
  }

  // Token expirado ou inexistente: limpa a sessão
  if (token) {
    clearSession();
  }

  return null;
}

export function clearSession() {
  localStorage.removeItem("auth_token");
  localStorage.removeItem("auth_user");
}

/**
 * Verifica se o token JWT está expirado
 * @param {string} token - Token JWT
 * @returns {boolean} - True se expirado, False se ainda válido
 */
export function isTokenExpired(token) {
  try {
    const decoded = jwtDecode(token);

    // JWT exp está em segundos, Date.now() em milissegundos
    if (!decoded.exp) {
      return false; // Se não tem exp, considera válido (depende do backend)
    }

    const currentTime = Date.now() / 1000;
    // Adiciona margem de segurança de 30 segundos
    return decoded.exp < currentTime + 30;
  } catch (error) {
    // Token inválido ou malformado
    console.error("Erro ao decodificar token:", error);
    return true;
  }
}

/**
 * Retorna o tempo restante até a expiração em segundos
 * @param {string} token - Token JWT
 * @returns {number|null} - Segundos até expiração ou null se inválido
 */
export function getTokenExpirationTime(token) {
  try {
    const decoded = jwtDecode(token);
    if (!decoded.exp) return null;

    const currentTime = Date.now() / 1000;
    const timeRemaining = decoded.exp - currentTime;
    return Math.max(0, timeRemaining);
  } catch (error) {
    return null;
  }
}
