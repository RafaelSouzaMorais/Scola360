import React, { createContext, useContext, useEffect, useState } from "react";
import api from "../services/api";
import { useAuth } from "./AuthContext.jsx";

const EnumContext = createContext({
  sexo: [],
  corRaca: [],
  loading: true,
});

export function EnumProvider({ children }) {
  const [sexo, setSexo] = useState([]);
  const [corRaca, setCorRaca] = useState([]);
  const [loading, setLoading] = useState(true);
  const { isAuthenticated } = useAuth();

  useEffect(() => {
    async function fetchEnums() {
      try {
        const [sexoRes, corRacaRes] = await Promise.all([
          api.get("/api/enums/sexo"),
          api.get("/api/enums/corraca"),
        ]);
        setSexo(
          Array.isArray(sexoRes.data)
            ? sexoRes.data.map((item) => ({
                label: item.name,
                value: item.value,
              }))
            : []
        );
        setCorRaca(
          Array.isArray(corRacaRes.data)
            ? corRacaRes.data.map((item) => ({
                label: item.name,
                value: item.value,
              }))
            : []
        );
      } catch (err) {
        setSexo([]);
        setCorRaca([]);
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
      setLoading(false);
    }
  }, [isAuthenticated]);

  return (
    <EnumContext.Provider value={{ sexo, corRaca, loading }}>
      {children}
    </EnumContext.Provider>
  );
}

export function useEnums() {
  return useContext(EnumContext);
}
