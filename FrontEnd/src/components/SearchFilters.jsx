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
  // Permite customizar o tamanho dos campos em cada breakpoint
  fieldColProps = { xs: 24, sm: 12, md: 12, lg: 8, xl: 6 },
  // Permite customizar a coluna dos botões
  actionsColProps = { xs: 24, sm: 24, md: 24, lg: "auto", xl: "auto" },
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
          <Col key={index} {...fieldColProps}>
            {child}
          </Col>
        ))}

        {/* Botões de ação */}
        <Col {...actionsColProps}>
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

export const FilterRole = ({ children }) => children;
// Você usa <FilterRole role="search"> ou role="local" só para semântica/organização

export default SearchFilters;
