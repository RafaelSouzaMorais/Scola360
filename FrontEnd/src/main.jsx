import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.jsx";
// Ant Design setup
import "antd/dist/reset.css";
import { ConfigProvider, App as AntdApp } from "antd";
import ptBR from "antd/locale/pt_BR";
import { AuthProvider } from "./contexts/AuthContext.jsx";
import { EnumProvider } from "./contexts/EnumContext.jsx";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <ConfigProvider
      locale={ptBR}
      theme={{ token: { colorPrimary: "#667eea" } }}
    >
      <AntdApp>
        <AuthProvider>
          <EnumProvider>
            <App />
          </EnumProvider>
        </AuthProvider>
      </AntdApp>
    </ConfigProvider>
  </StrictMode>
);
