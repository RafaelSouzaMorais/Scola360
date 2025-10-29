import React from "react";
import { Row, Col, Card, Statistic, Typography, Table, Progress } from "antd";
import {
  UserOutlined,
  TeamOutlined,
  BookOutlined,
  DollarOutlined,
  RiseOutlined,
  FallOutlined,
} from "@ant-design/icons";
import {
  BarChart,
  Bar,
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";
import "./Dashboard.css";

const { Title, Text } = Typography;

const Dashboard = () => {
  // Dados simulados - virão do backend futuramente
  const performanceData = [
    { turma: "1º A", media: 7.5 },
    { turma: "1º B", media: 8.2 },
    { turma: "2º A", media: 7.1 },
    { turma: "2º B", media: 8.5 },
    { turma: "3º A", media: 7.8 },
    { turma: "3º B", media: 8.0 },
  ];

  const revenueData = [
    { mes: "Jan", receita: 45000 },
    { mes: "Fev", receita: 52000 },
    { mes: "Mar", receita: 48000 },
    { mes: "Abr", receita: 61000 },
    { mes: "Mai", receita: 55000 },
    { mes: "Jun", receita: 67000 },
  ];

  const recentPayments = [
    {
      key: "1",
      aluno: "João Silva",
      turma: "1º A",
      valor: "R$ 850,00",
      data: "15/10/2025",
      status: "Pago",
    },
    {
      key: "2",
      aluno: "Maria Santos",
      turma: "2º B",
      valor: "R$ 850,00",
      data: "15/10/2025",
      status: "Pago",
    },
    {
      key: "3",
      aluno: "Pedro Oliveira",
      turma: "3º A",
      valor: "R$ 950,00",
      data: "14/10/2025",
      status: "Pago",
    },
    {
      key: "4",
      aluno: "Ana Costa",
      turma: "1º B",
      valor: "R$ 850,00",
      data: "14/10/2025",
      status: "Pendente",
    },
  ];

  const columns = [
    {
      title: "Aluno",
      dataIndex: "aluno",
      key: "aluno",
    },
    {
      title: "Turma",
      dataIndex: "turma",
      key: "turma",
    },
    {
      title: "Valor",
      dataIndex: "valor",
      key: "valor",
    },
    {
      title: "Data",
      dataIndex: "data",
      key: "data",
    },
    {
      title: "Status",
      dataIndex: "status",
      key: "status",
      render: (status) => (
        <Text type={status === "Pago" ? "success" : "warning"}>{status}</Text>
      ),
    },
  ];

  return (
    <div className="dashboard">
      <Title level={2}>Dashboard</Title>

      {/* Indicadores Principais */}
      <Row gutter={[16, 16]} style={{ marginTop: 24 }}>
        <Col xs={24} sm={12} lg={6}>
          <Card>
            <Statistic
              title="Total de Alunos"
              value={1200}
              prefix={<UserOutlined />}
              valueStyle={{ color: "#3f8600" }}
              suffix={<RiseOutlined />}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card>
            <Statistic
              title="Inadimplência"
              value={5}
              suffix="%"
              prefix={<DollarOutlined />}
              valueStyle={{ color: "#cf1322" }}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card>
            <Statistic
              title="Turmas Ativas"
              value={45}
              prefix={<TeamOutlined />}
              valueStyle={{ color: "#1677ff" }}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card>
            <Statistic
              title="Média Geral"
              value={7.3}
              precision={1}
              prefix={<BookOutlined />}
              valueStyle={{ color: "#faad14" }}
            />
          </Card>
        </Col>
      </Row>

      {/* Indicadores Secundários */}
      <Row gutter={[16, 16]} style={{ marginTop: 16 }}>
        <Col xs={24} md={12}>
          <Card title="Presença Média" size="small">
            <Progress percent={93} status="active" />
            <Text type="secondary">Últimos 30 dias</Text>
          </Card>
        </Col>
        <Col xs={24} md={12}>
          <Card title="Taxa de Aprovação" size="small">
            <Progress percent={87} strokeColor="#52c41a" />
            <Text type="secondary">Ano letivo atual</Text>
          </Card>
        </Col>
      </Row>

      {/* Gráficos */}
      <Row gutter={[16, 16]} style={{ marginTop: 24 }}>
        <Col xs={24} lg={12}>
          <Card title="📊 Desempenho por Turma">
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={performanceData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="turma" />
                <YAxis domain={[0, 10]} />
                <Tooltip />
                <Legend />
                <Bar dataKey="media" fill="#667eea" name="Média" />
              </BarChart>
            </ResponsiveContainer>
          </Card>
        </Col>

        <Col xs={24} lg={12}>
          <Card title="📉 Receita Mensal">
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={revenueData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="mes" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Line
                  type="monotone"
                  dataKey="receita"
                  stroke="#52c41a"
                  strokeWidth={2}
                  name="Receita (R$)"
                />
              </LineChart>
            </ResponsiveContainer>
          </Card>
        </Col>
      </Row>

      {/* Tabela de Pagamentos */}
      <Row gutter={[16, 16]} style={{ marginTop: 24 }}>
        <Col span={24}>
          <Card title="🧾 Últimos Pagamentos Recebidos">
            <Table
              columns={columns}
              dataSource={recentPayments}
              pagination={false}
              size="small"
            />
          </Card>
        </Col>
      </Row>
    </div>
  );
};

export default Dashboard;
