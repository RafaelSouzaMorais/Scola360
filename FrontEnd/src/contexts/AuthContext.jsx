import React, {
  createContext,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react";
import {
  login as loginRequest,
  saveSession,
  loadSession,
  clearSession,
  getCurrentUser,
  getTokenExpirationTime,
} from "../services/authService";
import { message } from "antd";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const [loading, setLoading] = useState(true);

  // Carrega sessão do localStorage no boot
  useEffect(() => {
    console.log("AuthContext: Carregando sessão...");
    const session = loadSession();
    if (session?.token) {
      console.log("AuthContext: Sessão encontrada", session);
      setToken(session.token);
      setUser(session.user || null);
      // Reativado: buscar usuário atual do backend
      getCurrentUser()
        .then(setUser)
        .catch(() => {
          handleLogout();
        })
        .finally(() => setLoading(false));
    } else {
      console.log("AuthContext: Nenhuma sessão encontrada");
      setLoading(false);
    }
  }, []);

  // Auto-logout quando o token expirar
  useEffect(() => {
    if (!token) return;

    const expirationTime = getTokenExpirationTime(token);
    if (expirationTime === null || expirationTime <= 0) {
      handleLogout();
      return;
    }

    // Agenda logout automático quando o token expirar
    const timeoutId = setTimeout(() => {
      message.warning("Sua sessão expirou. Faça login novamente.");
      handleLogout();
    }, expirationTime * 1000);

    return () => clearTimeout(timeoutId);
  }, [token]);

  const handleLogin = async (username, password) => {
    const data = await loginRequest(username, password);
    const { token, user } = data;
    saveSession({ token, user });
    setToken(token);
    setUser(user);
    return user;
  };

  const handleLogout = () => {
    clearSession();
    setToken(null);
    setUser(null);
    message.info("Sessão encerrada");
  };

  const value = useMemo(
    () => ({
      user,
      token,
      loading,
      isAuthenticated: Boolean(token),
      login: handleLogin,
      logout: handleLogout,
    }),
    [user, token, loading]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth deve ser usado dentro de AuthProvider");
  return ctx;
}
