import { Tag, Tooltip } from "antd";

export default function FilterField({
  role = "search",
  label,
  children,
  className = "",
  style,
}) {
  const isSearch = role === "search";
  const badge = label ?? (isSearch ? "Busca" : "Local");
  return (
    <div
      className={`filter-field ${
        isSearch ? "filter-field--search" : "filter-field--local"
      } ${className}`}
      style={style}
    >
      <div className="filter-field-badge">
        <Tooltip
          title={
            isSearch
              ? "Usado no botão Buscar"
              : "Apenas limpo pelo botão Limpar"
          }
        >
          <Tag color={isSearch ? "blue" : "default"}>{badge}</Tag>
        </Tooltip>
      </div>
      {children}
    </div>
  );
}
