import React, { createContext, useContext, useEffect, useState } from "react";
import api from "../services/api";
import { useAuth } from "./AuthContext.jsx";

const EnumContext = createContext({
  sexo: [],
  corRaca: [],
  tipoCertidao: [],
  statusMatricula: [],
  tipoFuncionario: [],
  situacaoFinal: [],
  tipoCalculoNota: [],
  turno: [],
  loading: true,
});

export function EnumProvider({ children }) {
  const [sexo, setSexo] = useState([]);
  const [corRaca, setCorRaca] = useState([]);
  const [tipoCertidao, setTipoCertidao] = useState([]);
  const [statusMatricula, setStatusMatricula] = useState([]);
  const [tipoFuncionario, setTipoFuncionario] = useState([]);
  const [situacaoFinal, setSituacaoFinal] = useState([]);
  const [tipoCalculoNota, setTipoCalculoNota] = useState([]);
  const [turno, setTurno] = useState([]);
  const [loading, setLoading] = useState(true);
  const { isAuthenticated } = useAuth();

  useEffect(() => {
    async function fetchEnums() {
      try {
        const response = await api.get("/api/enums");
        const enumsData = response.data;

        // Função auxiliar para converter enums em formato de opções para Ant Design
        const convertToOptions = (enumArray) => {
          return Array.isArray(enumArray)
            ? enumArray.map((item) => ({
                label: formatEnumName(item.name),
                value: item.value,
              }))
            : [];
        };

        // Função para formatar nomes dos enums (converter de PascalCase para texto legível)
        const formatEnumName = (name) => {
          return name
            .replace(/([A-Z])/g, " $1") // Adiciona espaço antes de maiúsculas
            .replace(/^./, (str) => str.toUpperCase()) // Primeira letra maiúscula
            .trim(); // Remove espaços extras
        };

        // Processa todos os enums
        setSexo(convertToOptions(enumsData.sexo));
        setCorRaca(convertToOptions(enumsData.corRaca));
        setTipoCertidao(convertToOptions(enumsData.tipoCertidao));
        setStatusMatricula(convertToOptions(enumsData.statusMatricula));
        setTipoFuncionario(convertToOptions(enumsData.tipoFuncionario));
        setSituacaoFinal(convertToOptions(enumsData.situacaoFinal));
        setTipoCalculoNota(convertToOptions(enumsData.tipoCalculoNota));
        setTurno(convertToOptions(enumsData.turno));
      } catch (err) {
        console.error("Erro ao carregar enums:", err);
        // Limpa todos os enums em caso de erro
        setSexo([]);
        setCorRaca([]);
        setTipoCertidao([]);
        setStatusMatricula([]);
        setTipoFuncionario([]);
        setSituacaoFinal([]);
        setTipoCalculoNota([]);
        setTurno([]);
      } finally {
        setLoading(false);
      }
    }

    if (isAuthenticated) {
      setLoading(true);
      fetchEnums();
    } else {
      // Limpa enums quando desautenticar
      setSexo([]);
      setCorRaca([]);
      setTipoCertidao([]);
      setStatusMatricula([]);
      setTipoFuncionario([]);
      setSituacaoFinal([]);
      setTipoCalculoNota([]);
      setTurno([]);
      setLoading(false);
    }
  }, [isAuthenticated]);

  return (
    <EnumContext.Provider
      value={{
        sexo,
        corRaca,
        tipoCertidao,
        statusMatricula,
        tipoFuncionario,
        situacaoFinal,
        tipoCalculoNota,
        turno,
        loading,
      }}
    >
      {children}
    </EnumContext.Provider>
  );
}

export function useEnums() {
  return useContext(EnumContext);
}
