import React from "react";
import { Row, Col, Button, Space } from "antd";
import { SearchOutlined } from "@ant-design/icons";
import "./SearchFilters.css";

const SearchFilters = ({
  children,
  onSearch,
  onClear,
  searchText = "Buscar",
  clearText = "Limpar",
  showSearchButton = true,
  showClearButton = true,
  loading = false,
  className = "",
  gutter = [16, 8],
  justify = "start",
}) => {
  const handleSearch = () => {
    if (onSearch) {
      onSearch();
    }
  };

  const handleClear = () => {
    if (onClear) {
      onClear();
    }
  };

  return (
    <div className={`search-filters ${className}`}>
      <Row gutter={gutter} align="middle" justify={justify} wrap>
        {/* Renderiza os campos de filtro passados como children */}
        {React.Children.map(children, (child, index) => (
          <Col key={index} xs={24} sm={12} md={8} lg={6} xl={4}>
            {child}
          </Col>
        ))}

        {/* Botões de ação */}
        <Col xs={24} sm={24} md={24} lg="auto" xl="auto">
          <Space wrap className="search-filters-actions">
            {showSearchButton && (
              <Button
                type="primary"
                icon={<SearchOutlined />}
                onClick={handleSearch}
                loading={loading}
                className="search-button"
              >
                {searchText}
              </Button>
            )}

            {showClearButton && (
              <Button onClick={handleClear} className="clear-button">
                {clearText}
              </Button>
            )}
          </Space>
        </Col>
      </Row>
    </div>
  );
};

export default SearchFilters;
