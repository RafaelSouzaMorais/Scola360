import React from "react";

/**
 * Componente de legenda para exibir o significado das cores das linhas em tabelas
 * @param {Object} props
 * @param {Array} props.items - Array de itens da legenda [{label, color}, ...]
 * @param {React.CSSProperties} props.style - Estilos adicionais para o container
 */
export default function RowStatusLegend({ items, style }) {
  // Items padrão caso não sejam fornecidos
  const defaultItems = [
    { label: "Linhas novas", color: "var(--row-new-bg)" },
    { label: "Linhas editadas", color: "var(--row-edited-bg)" },
  ];

  const legendItems = items || defaultItems;

  return (
    <div className="legend-status" style={style}>
      {legendItems.map((item, index) => (
        <span key={index} className="legend-item">
          <span className="legend-dot" style={{ background: item.color }} />
          {item.label}
        </span>
      ))}
    </div>
  );
}
