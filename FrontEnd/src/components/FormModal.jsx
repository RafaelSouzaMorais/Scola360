import React from "react";
import { Modal, Button, Space } from "antd";
import { SaveOutlined, CloseOutlined } from "@ant-design/icons";
import "./FormModal.css";

const FormModal = ({
  // Propriedades do Modal
  open = false,
  onClose,
  title,
  width = 600,
  centered = true,
  destroyOnClose = true,
  maskClosable = false,

  // Propriedades do Formulário
  children,
  onSave,
  onCancel,

  // Estado de loading e mode
  loading = false,
  mode = "create", // "create" ou "edit"

  // Textos customizáveis
  saveText,
  cancelText = "Cancelar",
  createTitle = "Novo Registro",
  editTitle = "Editar Registro",

  // Configurações dos botões
  showSaveButton = true,
  showCancelButton = true,
  saveButtonProps = {},
  cancelButtonProps = {},

  // Layout e estilo
  className = "",
  bodyStyle = {},
  footerStyle = {},
}) => {
  // Define o título automaticamente baseado no mode se não for fornecido
  const modalTitle = title || (mode === "edit" ? editTitle : createTitle);

  // Define o texto do botão salvar baseado no mode se não for fornecido
  const buttonSaveText = saveText || (mode === "edit" ? "Atualizar" : "Salvar");

  const handleSave = () => {
    if (onSave) {
      onSave();
    }
  };

  const handleCancel = () => {
    if (onCancel) {
      onCancel();
    } else if (onClose) {
      onClose();
    }
  };

  const footer = (
    <div className="form-modal-footer" style={footerStyle}>
      <Space>
        {showCancelButton && (
          <Button
            icon={<CloseOutlined />}
            onClick={handleCancel}
            className="cancel-button"
            {...cancelButtonProps}
          >
            {cancelText}
          </Button>
        )}

        {showSaveButton && (
          <Button
            type="primary"
            icon={<SaveOutlined />}
            onClick={handleSave}
            loading={loading}
            className="save-button"
            {...saveButtonProps}
          >
            {buttonSaveText}
          </Button>
        )}
      </Space>
    </div>
  );

  return (
    <Modal
      title={modalTitle}
      open={open}
      onCancel={onClose}
      width={width}
      centered={centered}
      destroyOnClose={destroyOnClose}
      maskClosable={maskClosable}
      footer={footer}
      className={`form-modal ${mode}-mode ${className}`}
      styles={{ body: bodyStyle }}
    >
      <div className="form-modal-content">{children}</div>
    </Modal>
  );
};

export default FormModal;
