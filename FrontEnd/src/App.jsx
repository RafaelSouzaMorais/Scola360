import React from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import MainLayout from "./layouts/MainLayout";
import Dashboard from "./pages/Dashboard/Dashboard";
import ListaAlunos from "./pages/Alunos/Aluno";
import { useAuth } from "./contexts/AuthContext";
import RoleProtectedRoute from "./components/RoleProtectedRoute";
import { usePermissions } from "./hooks/usePermissions";
import "./App.css";
import Disciplinas from "./pages/Disciplinas/Disciplina";
import Cursos from "./pages/Cursos/Curso";
import Curriculo from "./pages/Curriculos/Curriculo";
import Periodos from "./pages/Periodos/Periodo";
import Professores from "./pages/Funcionarios/Funcionario";
import Turmas from "./pages/Turmas/Turma";

// Componente de rota protegida
const ProtectedRoute = ({ children }) => {
  const { isAuthenticated, loading } = useAuth();

  console.log("ProtectedRoute:", { isAuthenticated, loading });

  if (loading) {
    return (
      <div style={{ padding: "20px", textAlign: "center" }}>Carregando...</div>
    );
  }

  return isAuthenticated ? children : <Navigate to="/login" replace />;
};

// Componente que usa as permissões centralizadas
const AppRoutes = () => {
  const { routePermissions } = usePermissions();

  return (
    <Routes>
      <Route path="/login" element={<Login />} />

      <Route
        path="/"
        element={
          <ProtectedRoute>
            <MainLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<Navigate to="/dashboard" replace />} />

        {/* Dashboard */}
        <Route
          path="dashboard"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.dashboard}>
              <Dashboard />
            </RoleProtectedRoute>
          }
        />

        {/* Alunos */}
        <Route
          path="students"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.students}>
              <ListaAlunos />
            </RoleProtectedRoute>
          }
        />

        {/* Professores */}
        <Route
          path="teachers"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.teachers}>
              <Professores />
            </RoleProtectedRoute>
          }
        />

        {/* Turmas */}
        <Route
          path="classes"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.classes}>
              <Turmas />
            </RoleProtectedRoute>
          }
        />

        {/* Disciplinas */}
        <Route
          path="subjects"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.subjects}>
              <Disciplinas />
            </RoleProtectedRoute>
          }
        />

        {/* Cursos */}
        <Route
          path="courses"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.courses}>
              <Cursos />
            </RoleProtectedRoute>
          }
        />

        {/* Currículos */}
        <Route
          path="curriculum"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.curriculum}>
              <Curriculo />
            </RoleProtectedRoute>
          }
        />

        {/* Períodos */}
        <Route
          path="periods"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.periods}>
              <Periodos />
            </RoleProtectedRoute>
          }
        />

        {/* Notas */}
        <Route
          path="grades"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.grades}>
              <div>Página de Notas (em breve)</div>
            </RoleProtectedRoute>
          }
        />

        {/* Financeiro */}
        <Route
          path="financial"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.financial}>
              <div>Página Financeira (em breve)</div>
            </RoleProtectedRoute>
          }
        />

        {/* Relatórios */}
        <Route
          path="reports"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.reports}>
              <div>Página de Relatórios (em breve)</div>
            </RoleProtectedRoute>
          }
        />

        {/* Comunicação */}
        <Route
          path="communication"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.communication}>
              <div>Página de Comunicação (em breve)</div>
            </RoleProtectedRoute>
          }
        />

        {/* Configurações */}
        <Route
          path="settings"
          element={
            <RoleProtectedRoute allowedRoles={routePermissions.settings}>
              <div>Página de Configurações (em breve)</div>
            </RoleProtectedRoute>
          }
        />
      </Route>

      <Route path="*" element={<Navigate to="/login" replace />} />
    </Routes>
  );
};

function App() {
  return (
    <BrowserRouter>
      <AppRoutes />
    </BrowserRouter>
  );
}

export default App;
