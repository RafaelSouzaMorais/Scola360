import React, { useState } from "react";
import { Card, Typography, Form, Input, Button, Flex, theme, App } from "antd";
import { UserOutlined, LockOutlined, LoginOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import "./Login.css";
import { useAuth } from "../contexts/AuthContext.jsx";

const { Title, Text, Link } = Typography;

const Login = () => {
  const [loading, setLoading] = useState(false);
  const [form] = Form.useForm();
  const { token: antdToken } = theme.useToken();
  const { login } = useAuth();
  const { message } = App.useApp();
  const navigate = useNavigate();

  const onFinish = async (values) => {
    setLoading(true);
    try {
      const { username, password } = values;
      await login(username, password);
      message.success("Login realizado com sucesso!");
      navigate("/dashboard");
    } catch (err) {
      // Trata erros específicos do backend
      const status = err?.response?.status;
      const apiMessage = err?.response?.data?.message;

      if (status === 401) {
        message.error("Usuário ou senha inválidos");
      } else if (status === 404) {
        message.error("Usuário não encontrado");
      } else if (status === 400) {
        message.error(apiMessage || "Dados inválidos");
      } else if (apiMessage) {
        message.error(apiMessage);
      } else if (err.message === "Network Error") {
        message.error(
          "Erro de conexão com o servidor. Verifique se o backend está rodando."
        );
      } else {
        message.error("Erro ao realizar login. Tente novamente.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      className="login-container"
      style={{
        background: `linear-gradient(135deg, ${antdToken.colorPrimary} 0%, ${antdToken.colorBgLayout} 100%)`,
      }}
    >
      <Card className="login-card" variant="borderless">
        <Flex vertical gap={8} align="center" className="login-header">
          <Title level={2} style={{ marginBottom: 0 }}>
            Sistema Acadêmico
          </Title>
          <Text type="secondary">Faça login para acessar o sistema</Text>
        </Flex>

        <Form
          form={form}
          layout="vertical"
          onFinish={onFinish}
          requiredMark={false}
          className="login-form"
          size="large"
        >
          <Form.Item
            label="Usuário"
            name="username"
            rules={[{ required: true, message: "Informe seu usuário" }]}
          >
            <Input
              prefix={<UserOutlined />}
              placeholder="Digite seu nome de usuário"
              autoComplete="username"
            />
          </Form.Item>

          <Form.Item
            label="Senha"
            name="password"
            rules={[{ required: true, message: "Informe sua senha" }]}
          >
            <Input.Password
              prefix={<LockOutlined />}
              placeholder="Digite sua senha"
              autoComplete="current-password"
            />
          </Form.Item>

          <Form.Item>
            <Button
              type="primary"
              htmlType="submit"
              icon={<LoginOutlined />}
              loading={loading}
              block
            >
              Entrar
            </Button>
          </Form.Item>
        </Form>

        <div className="login-footer">
          <Text>
            Não tem uma conta?{" "}
            <Link onClick={(e) => e.preventDefault()}>Solicite acesso</Link>
          </Text>
        </div>
      </Card>
    </div>
  );
};

export default Login;
