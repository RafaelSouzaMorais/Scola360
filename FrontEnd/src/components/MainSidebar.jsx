import React, { useState } from "react";
import { Layout, Menu } from "antd";
import {
  HomeOutlined,
  UserOutlined,
  TeamOutlined,
  BookOutlined,
  ReadOutlined,
  BarChartOutlined,
  DollarOutlined,
  FileTextOutlined,
  MessageOutlined,
  SettingOutlined,
  ScheduleOutlined,
} from "@ant-design/icons";
import { useNavigate, useLocation } from "react-router-dom";

const { Sider } = Layout;

const MainSidebar = ({ userRoles = [], collapsed, setCollapsed }) => {
  const navigate = useNavigate();
  const location = useLocation();

  // Normaliza roles para lowercase e garante array
  const effectiveRoles = Array.isArray(userRoles)
    ? userRoles.map((r) => String(r).toLowerCase())
    : [];

  // Define quais menus cada role pode acessar
  const menuPermissions = {
    dashboard: ["admin", "teacher", "secretary", "financial"],
    students: ["admin", "secretary", "teacher"],
    teachers: ["admin", "secretary"],
    classes: ["admin", "secretary", "teacher"],
    subjects: ["admin", "teacher"],
    grades: ["admin", "teacher"],
    financial: ["admin", "financial"],
    reports: ["admin", "secretary", "financial"],
    communication: ["admin", "teacher", "secretary"],
    settings: ["secretary"],
  };

  // Verifica se o usuário tem permissão para acessar um menu
  const hasPermission = (menuKey) => {
    if (!menuPermissions[menuKey]) return true;
    // Fallback: se não há roles (não carregado/ausente), não esconda o menu
    if (!effectiveRoles.length) return true;
    return menuPermissions[menuKey].some((role) =>
      effectiveRoles.includes(role)
    );
  };

  const menuItems = [
    hasPermission("dashboard") && {
      key: "/dashboard",
      icon: <HomeOutlined />,
      label: "Dashboard",
    },
    hasPermission("students") && {
      key: "/students",
      icon: <UserOutlined />,
      label: "Alunos",
    },
    hasPermission("teachers") && {
      key: "/teachers",
      icon: <TeamOutlined />,
      label: "Professores",
    },
    hasPermission("classes") && {
      key: "/classes",
      icon: <ScheduleOutlined />,
      label: "Turmas e Horários",
    },
    hasPermission("subjects") && {
      key: "/subjects",
      icon: <BookOutlined />,
      label: "Disciplinas",
    },
    hasPermission("grades") && {
      key: "/grades",
      icon: <ReadOutlined />,
      label: "Notas e Avaliações",
    },
    hasPermission("financial") && {
      key: "/financial",
      icon: <DollarOutlined />,
      label: "Financeiro",
    },
    hasPermission("reports") && {
      key: "/reports",
      icon: <BarChartOutlined />,
      label: "Relatórios",
    },
    hasPermission("communication") && {
      key: "/communication",
      icon: <MessageOutlined />,
      label: "Comunicação",
    },
    hasPermission("settings") && {
      key: "/settings",
      icon: <SettingOutlined />,
      label: "Configurações",
      children: [
        {
          key: "/settings/courses",
          label: "Cursos",
        },
        {
          key: "/settings/curriculum",
          label: "Currículos",
        },
        {
          key: "/settings/periods",
          label: "Períodos",
        },
        {
          key: "/settings/general",
          label: "Geral",
        },
      ],
    },
  ].filter(Boolean); // Remove itens null/undefined

  const handleMenuClick = ({ key }) => {
    navigate(key);
  };

  return (
    <Sider
      collapsible
      collapsed={collapsed}
      onCollapse={setCollapsed}
      theme="light"
      width={250}
      style={{
        overflow: "auto",
        height: "100vh",
        position: "sticky",
        top: 0,
        left: 0,
        boxShadow: "2px 0 8px rgba(0,0,0,0.05)",
      }}
    >
      <Menu
        mode="inline"
        selectedKeys={[location.pathname]}
        onClick={handleMenuClick}
        items={menuItems}
        style={{ borderRight: 0, paddingTop: 16 }}
      />
    </Sider>
  );
};

export default MainSidebar;
