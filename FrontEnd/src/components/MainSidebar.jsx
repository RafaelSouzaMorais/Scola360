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
import { usePermissions } from "../hooks/usePermissions";

const { Sider } = Layout;

const MainSidebar = ({ collapsed, setCollapsed }) => {
  const navigate = useNavigate();
  const location = useLocation();
  const { hasPermission } = usePermissions();

  const menuItems = [
    hasPermission("dashboard") && {
      key: "/dashboard",
      icon: <HomeOutlined />,
      label: "Dashboard",
    },
    // Grupo de Cadastros
    (hasPermission("students") ||
      hasPermission("subjects") ||
      hasPermission("teachers") ||
      hasPermission("classes") ||
      hasPermission("courses") ||
      hasPermission("curriculum") ||
      hasPermission("periods")) && {
      key: "cadastro",
      icon: <FileTextOutlined />,
      label: "Cadastro",
      children: [
        hasPermission("students") && {
          key: "/students",
          icon: <UserOutlined />,
          label: "Alunos",
        },
        hasPermission("subjects") && {
          key: "/subjects",
          icon: <BookOutlined />,
          label: "Disciplinas",
        },
        hasPermission("courses") && {
          key: "/courses",
          icon: <FileTextOutlined />,
          label: "Cursos",
        },
        hasPermission("curriculum") && {
          key: "/curriculum",
          icon: <FileTextOutlined />,
          label: "Currículos",
        },
        hasPermission("periods") && {
          key: "/periods",
          icon: <FileTextOutlined />,
          label: "Períodos",
        },
        hasPermission("classes") && {
          key: "/classes",
          icon: <ScheduleOutlined />,
          label: "Turmas",
        },
        hasPermission("teachers") && {
          key: "/teachers",
          icon: <TeamOutlined />,
          label: "Professores",
        },
      ].filter(Boolean),
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
