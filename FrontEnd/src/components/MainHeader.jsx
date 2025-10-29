import React from "react";
import { Layout, Menu, Avatar, Badge, Dropdown, Space, Typography } from "antd";
import {
  BellOutlined,
  UserOutlined,
  LogoutOutlined,
  SettingOutlined,
} from "@ant-design/icons";
import { useAuth } from "../contexts/AuthContext";
import "./MainHeader.css";

const { Header } = Layout;
const { Text } = Typography;

const MainHeader = ({ onToggleSidebar, isSidebarCollapsed }) => {
  const { user, logout } = useAuth();

  const userMenuItems = [
    {
      key: "profile",
      icon: <UserOutlined />,
      label: "Meu Perfil",
    },
    {
      key: "settings",
      icon: <SettingOutlined />,
      label: "Configurações",
    },
    {
      type: "divider",
    },
    {
      key: "logout",
      icon: <LogoutOutlined />,
      label: "Sair",
      danger: true,
      onClick: logout,
    },
  ];

  const notificationItems = [
    {
      key: "1",
      label: "Novo aluno matriculado",
    },
    {
      key: "2",
      label: "Pagamento recebido - Turma A",
    },
    {
      key: "3",
      label: "5 provas para corrigir",
    },
  ];

  return (
    <Header className="main-header">
      <div className="header-left">
        {onToggleSidebar && (
          <span
            className="sidebar-toggle"
            onClick={onToggleSidebar}
            aria-label={isSidebarCollapsed ? "Abrir menu" : "Fechar menu"}
          >
            <span
              className="anticon"
              style={{ fontSize: 22, color: "white", marginRight: 12 }}
            >
              {isSidebarCollapsed ? (
                <svg
                  width="1em"
                  height="1em"
                  fill="currentColor"
                  viewBox="0 0 1024 1024"
                >
                  <path d="M328 544h368c4.4 0 8-3.6 8-8v-48c0-4.4-3.6-8-8-8H328c-4.4 0-8 3.6-8 8v48c0 4.4 3.6 8 8 8z" />
                </svg>
              ) : (
                <svg
                  width="1em"
                  height="1em"
                  fill="currentColor"
                  viewBox="0 0 1024 1024"
                >
                  <path d="M176 511.9c0 4.4 3.6 8 8 8h656c4.4 0 8-3.6 8-8v-48c0-4.4-3.6-8-8-8H184c-4.4 0-8 3.6-8 8v48z" />
                </svg>
              )}
            </span>
          </span>
        )}
        <div className="header-logo">
          <div className="logo-circle">🎓</div>
          <Text strong style={{ color: "white", fontSize: "18px" }}>
            Sistema Acadêmico
          </Text>
        </div>
      </div>
      <div className="header-actions">
        <Dropdown
          menu={{ items: notificationItems }}
          placement="bottomRight"
          arrow
        >
          <Badge count={3} offset={[-5, 5]}>
            <BellOutlined className="header-icon" />
          </Badge>
        </Dropdown>
        <Dropdown menu={{ items: userMenuItems }} placement="bottomRight" arrow>
          <Space className="user-info" style={{ cursor: "pointer" }}>
            <Avatar icon={<UserOutlined />} />
            <Text style={{ color: "white" }}>{user?.name || "Usuário"}</Text>
          </Space>
        </Dropdown>
      </div>
    </Header>
  );
};

export default MainHeader;
