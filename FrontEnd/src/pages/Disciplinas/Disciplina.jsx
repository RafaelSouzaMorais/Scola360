import React, { useEffect, useState } from "react";
import {
  Table,
  Button,
  Space,
  Popconfirm,
  message,
  Drawer,
  Tabs,
  Form,
  Input,
  DatePicker,
  Select,
  Switch,
  Row,
  Col,
} from "antd";
import { listarDisciplinas } from "../../services/Disciplina/disciplinaService";

export default function Disciplinas() {
  const [disciplinas, setDisciplinas] = useState([]);
  const [loading, setLoading] = useState(false);

  const fetchDisciplinas = async () => {
    setLoading(true);
    try {
      const response = await listarDisciplinas();
      console.log("Disciplinas carregadas:", response);
      setDisciplinas(response);
    } catch (error) {
      message.error("Erro ao carregar disciplinas");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDisciplinas();
  }, []);

  const columns = [
    {
      title: "Nome",
      dataIndex: "nome",
      key: "nome",
      width: 200,
      sorter: (a, b) => a.nome.localeCompare(b.nome),
    },
    {
      title: "Código",
      dataIndex: "codigo",
      key: "codigo",
      width: 100,
      sorter: (a, b) => a.codigo.localeCompare(b.codigo),
    },
    {
      title: "Carga Horária",
      dataIndex: "cargaHoraria",
      key: "cargaHoraria",
      width: 150,
      sorter: (a, b) => a.cargaHoraria - b.cargaHoraria,
    },
  ];

  return (
    <div>
      <div style={{ width: "100%", overflowX: "auto" }}>
        <Table
          columns={columns}
          dataSource={disciplinas}
          rowKey="id"
          loading={loading}
          pagination={{ pageSize: 10 }}
          scroll={{ x: 600 }}
        />
      </div>
    </div>
  );
}
