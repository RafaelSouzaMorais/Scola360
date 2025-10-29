import React from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import MainLayout from "./layouts/MainLayout";
import Dashboard from "./pages/Dashboard/Dashboard";
import ListaAlunos from "./pages/Alunos/Aluno";
import { useAuth } from "./contexts/AuthContext";
import "./App.css";
import Disciplinas from "./pages/Disciplinas/Disciplina";

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

function App() {
  return (
    <BrowserRouter>
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
          <Route path="dashboard" element={<Dashboard />} />
          <Route path="students" element={<ListaAlunos />} />
          <Route path="teachers" element={<Disciplinas />} />
          <Route
            path="classes"
            element={<div>Página de Turmas (em breve)</div>}
          />
          <Route
            path="subjects"
            element={<div>Página de Disciplinas (em breve)</div>}
          />
          <Route
            path="grades"
            element={<div>Página de Notas (em breve)</div>}
          />
          <Route
            path="financial"
            element={<div>Página Financeira (em breve)</div>}
          />
          <Route
            path="reports"
            element={<div>Página de Relatórios (em breve)</div>}
          />
          <Route
            path="communication"
            element={<div>Página de Comunicação (em breve)</div>}
          />
          <Route
            path="settings"
            element={<div>Página de Configurações (em breve)</div>}
          />
        </Route>

        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
