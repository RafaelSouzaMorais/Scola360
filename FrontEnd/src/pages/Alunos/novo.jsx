import React from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import {
  Button,
  Form,
  Input,
  DatePicker,
  Select,
  Row,
  Col,
  message,
  Switch,
} from "antd";
import { cadastrarAluno } from "../../services/Aluno/alunoService";
import dayjs from "dayjs";
import { useNavigate } from "react-router-dom";

const schema = yup.object().shape({
  nome: yup.string().required("Nome completo é obrigatório"),
  cpf: yup
    .string()
    .required("CPF é obrigatório")
    .matches(/^\d{11}$/g, "CPF deve ter 11 dígitos"),
  dataNascimento: yup.date().required("Data de nascimento é obrigatória"),
  email: yup.string().email("E-mail inválido").required("E-mail é obrigatório"),
  telefone: yup.string().required("Telefone é obrigatório"),
  endereco: yup.string().required("Endereço é obrigatório"),
  responsavel: yup.string().required("Responsável é obrigatório"),
  status: yup.boolean().required(),
});

const responsaveis = [
  { label: "Pai", value: "Pai" },
  { label: "Mãe", value: "Mãe" },
  { label: "Outro", value: "Outro" },
];

export default function NovoAluno() {
  const navigate = useNavigate();
  const {
    control,
    handleSubmit,
    reset,
    setError,
    formState: { errors, isSubmitting },
  } = useForm({
    resolver: yupResolver(schema),
    defaultValues: {
      nome: "",
      cpf: "",
      dataNascimento: null,
      email: "",
      telefone: "",
      endereco: "",
      responsavel: "",
      dataMatricula: dayjs(),
      status: true,
    },
  });

  const onSubmit = async (values) => {
    try {
      const payload = {
        ...values,
        dataNascimento: values.dataNascimento.format("YYYY-MM-DD"),
        dataMatricula: dayjs().format("YYYY-MM-DD"),
      };
      await cadastrarAluno(payload);
      message.success("Aluno cadastrado com sucesso!");
      reset();
    } catch (err) {
      if (err.response && err.response.data && err.response.data.message) {
        message.error(err.response.data.message);
        if (err.response.data.field) {
          setError(err.response.data.field, {
            message: err.response.data.message,
          });
        }
      } else {
        message.error("Erro ao cadastrar aluno.");
      }
    }
  };

  return (
    <div className="max-w-3xl mx-auto bg-white p-6 rounded shadow-md mt-6">
      <h2 className="text-2xl font-bold mb-4">Cadastro de Aluno</h2>
      <Form layout="vertical" onFinish={handleSubmit(onSubmit)}>
        <Row gutter={16}>
          <Col xs={24} md={12}>
            <Form.Item
              label="Nome completo"
              validateStatus={errors.nome ? "error" : ""}
              help={errors.nome?.message}
            >
              <Input
                {...control.register("nome")}
                placeholder="Nome completo"
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={12}>
            <Form.Item
              label="CPF"
              validateStatus={errors.cpf ? "error" : ""}
              help={errors.cpf?.message}
            >
              <Input
                {...control.register("cpf")}
                placeholder="Somente números"
                maxLength={11}
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={12}>
            <Form.Item
              label="Data de nascimento"
              validateStatus={errors.dataNascimento ? "error" : ""}
              help={errors.dataNascimento?.message}
            >
              <DatePicker
                {...control.register("dataNascimento")}
                format="DD/MM/YYYY"
                style={{ width: "100%" }}
                placeholder="Selecione a data"
                disabledDate={(current) => current && current > dayjs()}
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={12}>
            <Form.Item
              label="E-mail"
              validateStatus={errors.email ? "error" : ""}
              help={errors.email?.message}
            >
              <Input {...control.register("email")} placeholder="E-mail" />
            </Form.Item>
          </Col>
          <Col xs={24} md={12}>
            <Form.Item
              label="Telefone"
              validateStatus={errors.telefone ? "error" : ""}
              help={errors.telefone?.message}
            >
              <Input {...control.register("telefone")} placeholder="Telefone" />
            </Form.Item>
          </Col>
          <Col xs={24} md={12}>
            <Form.Item
              label="Endereço"
              validateStatus={errors.endereco ? "error" : ""}
              help={errors.endereco?.message}
            >
              <Input {...control.register("endereco")} placeholder="Endereço" />
            </Form.Item>
          </Col>
          <Col xs={24} md={12}>
            <Form.Item
              label="Responsável"
              validateStatus={errors.responsavel ? "error" : ""}
              help={errors.responsavel?.message}
            >
              <Select
                {...control.register("responsavel")}
                options={responsaveis}
                placeholder="Selecione o responsável"
                allowClear
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={12}>
            <Form.Item label="Data de matrícula">
              <Input value={dayjs().format("DD/MM/YYYY")} disabled />
            </Form.Item>
          </Col>
          <Col xs={24} md={12}>
            <Form.Item label="Status">
              <Switch
                checkedChildren="Ativo"
                unCheckedChildren="Inativo"
                defaultChecked
                {...control.register("status")}
              />
            </Form.Item>
          </Col>
        </Row>
        <Space size="middle" style={{ marginTop: 16 }} wrap>
          <Button type="primary" htmlType="submit" loading={isSubmitting}>
            Salvar
          </Button>
          <Button onClick={() => navigate("/students")}>Voltar</Button>
        </Space>
      </Form>
    </div>
  );
}
