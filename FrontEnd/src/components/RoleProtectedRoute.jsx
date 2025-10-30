import React from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import { Result, Button } from "antd";

const RoleProtectedRoute = ({
  children,
  allowedRoles = [],
  requireAuth = true,
}) => {
  const { user, isAuthenticated, loading } = useAuth();

  // Aguarda carregamento da autenticação
  if (loading) {
    return (
      <div style={{ padding: "20px", textAlign: "center" }}>Carregando...</div>
    );
  }

  // Verifica se está autenticado (se necessário)
  if (requireAuth && !isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  // Se não há roles específicas exigidas, permite acesso
  if (!allowedRoles.length) {
    return children;
  }

  // Normaliza roles do usuário
  const userRoles = Array.isArray(user?.roles)
    ? user.roles.map((r) => String(r).toLowerCase())
    : [];

  // Normaliza roles permitidas
  const normalizedAllowedRoles = allowedRoles.map((r) =>
    String(r).toLowerCase()
  );

  // Verifica se o usuário tem pelo menos uma das roles permitidas
  const hasPermission = normalizedAllowedRoles.some((role) =>
    userRoles.includes(role)
  );

  if (!hasPermission) {
    return (
      <Result
        status="403"
        title="403"
        subTitle="Desculpe, você não tem permissão para acessar esta página."
        extra={
          <Button type="primary" onClick={() => window.history.back()}>
            Voltar
          </Button>
        }
      />
    );
  }

  return children;
};

export default RoleProtectedRoute;
