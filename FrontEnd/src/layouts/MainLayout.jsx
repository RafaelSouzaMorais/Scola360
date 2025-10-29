import React, { useState } from "react";
import { Layout } from "antd";
import { Outlet } from "react-router-dom";
import MainHeader from "../components/MainHeader";
import MainSidebar from "../components/MainSidebar";
import { useAuth } from "../contexts/AuthContext";

const { Content } = Layout;

const MainLayout = () => {
  const { user } = useAuth();

  const [sidebarCollapsed, setSidebarCollapsed] = useState(false);
  return (
    <Layout style={{ minHeight: "100vh" }}>
      <MainHeader
        onToggleSidebar={() => setSidebarCollapsed((c) => !c)}
        isSidebarCollapsed={sidebarCollapsed}
      />
      <Layout>
        <MainSidebar
          userRoles={user?.roles || []}
          collapsed={sidebarCollapsed}
          setCollapsed={setSidebarCollapsed}
        />
        <Layout style={{ padding: "24px" }}>
          <Content
            style={{
              background: "#fff",
              padding: 24,
              margin: 0,
              minHeight: 280,
              borderRadius: 8,
            }}
          >
            <Outlet />
          </Content>
        </Layout>
      </Layout>
    </Layout>
  );
};

export default MainLayout;
