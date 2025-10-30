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
