import { useAuth } from "../contexts/AuthContext";

// Hook para verificar permissões
export const usePermissions = () => {
  const { user } = useAuth();

  // Normaliza roles do usuário
  const userRoles = Array.isArray(user?.roles)
    ? user.roles.map((r) => String(r).toLowerCase())
    : [];

  // Define as permissões de cada rota
  const routePermissions = {
    dashboard: ["admin", "teacher", "secretary", "financial"],
    students: ["admin", "secretary", "teacher"],
    teachers: ["admin", "secretary"], // Apenas admin e secretary podem gerenciar professores
    classes: ["admin", "secretary", "teacher"],
    subjects: ["admin", "teacher"],
    courses: ["admin", "secretary"],
    curriculum: ["admin", "secretary"],
    periods: ["admin", "secretary"],
    grades: ["admin", "teacher"],
    financial: ["admin", "financial"],
    reports: ["admin", "secretary", "financial"],
    communication: ["admin", "teacher", "secretary"],
    settings: ["admin", "secretary"], // Apenas admin e secretary podem acessar configurações
  };

  // Função para verificar se tem permissão
  const hasPermission = (routeKey) => {
    // Se não há permissões definidas para a rota, permite acesso
    if (!routePermissions[routeKey]) return true;

    // Se não há roles do usuário, nega acesso
    if (!userRoles.length) return false;

    // Verifica se o usuário tem pelo menos uma das roles necessárias
    return routePermissions[routeKey].some((role) => userRoles.includes(role));
  };

  // Função para verificar múltiplas permissões
  const hasAnyPermission = (routeKeys) => {
    return routeKeys.some((key) => hasPermission(key));
  };

  // Função para verificar todas as permissões
  const hasAllPermissions = (routeKeys) => {
    return routeKeys.every((key) => hasPermission(key));
  };

  return {
    userRoles,
    hasPermission,
    hasAnyPermission,
    hasAllPermissions,
    routePermissions,
  };
};

export default usePermissions;
