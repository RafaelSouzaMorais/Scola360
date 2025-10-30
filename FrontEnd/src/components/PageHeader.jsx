import React from "react";
import { Button } from "antd";
import "./PageHeader.css";

const PageHeader = ({
  title,
  buttonText,
  onButtonClick,
  buttonProps = {},
  showButton = true,
  className = "",
}) => {
  return (
    <div className={`page-header ${className}`}>
      <h2 className="page-header-title">{title}</h2>
      {showButton && buttonText && (
        <Button
          type="primary"
          size="large"
          onClick={onButtonClick}
          className="page-header-button"
          {...buttonProps}
        >
          {buttonText}
        </Button>
      )}
    </div>
  );
};

export default PageHeader;
