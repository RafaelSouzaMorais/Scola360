#!/usr/bin/env sh
set -e

if [ -x "/app/migrate" ]; then
  echo "==> Executando migrations..."
  if [ -n "$ConnectionStrings__Default" ]; then
    /app/migrate --connection "$ConnectionStrings__Default"
  else
    /app/migrate
  fi
  echo "==> Migrations concluidas."
fi

echo "==> Iniciando API..."
exec dotnet Scola360.Academico.dll
