#!/usr/bin/env sh
set -e

# Apply migrations using EF Core bundle if present
if [ -x "/app/migrate" ]; then
  echo "==> Executando migrations..."
  if [ -n "$ConnectionStrings__Default" ]; then
    /app/migrate --connection "$ConnectionStrings__Default"
  else
    /app/migrate
  fi
  echo "==> Migrations concluídas."
fi

echo "==> Iniciando API..."
exec dotnet Scola360.Academico.dll
