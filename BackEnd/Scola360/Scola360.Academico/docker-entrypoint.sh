#!/bin/sh
set -e

normalize_cs() {
  input="$1"

  # URLs postgres:// ou postgresql://
  case "$input" in
    postgres://*|postgresql://*)
      uri="${input#*://}"
      creds="${uri%%@*}"
      rest="${uri#*@}"

      user="${creds%%:*}"
      pass="${creds#*:}"
      [ "$pass" = "$creds" ] && pass=""

      hostport="${rest%%/*}"
      dbq="${rest#*/}"

      # db e query sem usar padrões com '?'
      db=$(printf "%s" "$dbq" | cut -d'?' -f1)
      query=$(printf "%s" "$dbq" | cut -s -d'?' -f2)

      host=$(printf "%s" "$hostport" | cut -d':' -f1)
      port=$(printf "%s" "$hostport" | cut -s -d':' -f2)
      [ -z "$port" ] && port="5432"

      cs="Host=$host;Port=$port;Database=$db;Username=$user"
      [ -n "$pass" ] && cs="$cs;Password=$pass"

      if [ -n "$query" ]; then
        # extrai sslmode=valor, se existir
        sslmode=$(printf "%s" "$query" | tr '&' '\n' | awk -F '=' '$1=="sslmode"{print $2; exit}')
        [ -n "$sslmode" ] && cs="$cs;Ssl Mode=$sslmode"
      fi

      printf "%s" "$cs"
      return 0
      ;;
  esac

  # Corrige Host=tcp://host:port
  echo "$input" | grep -qi "Host=tcp://" && {
    hostport=$(printf "%s" "$input" | sed -n 's/.*Host=tcp:\/\/\([^;]*\).*/\1/p')
    host=$(printf "%s" "$hostport" | cut -d':' -f1)
    port=$(printf "%s" "$hostport" | cut -s -d':' -f2)
    out=$(printf "%s" "$input" | sed "s/Host=tcp:\/\/$hostport/Host=$host/i")
    echo "$out" | grep -qi ";[[:space:]]*Port=" || {
      [ -n "$port" ] && out="$out;Port=$port"
    }
    printf "%s" "$out"
    return 0
  }

  # Caso seja uma URL tcp://host:port inteira
  case "$input" in
    tcp://*)
      uri="${input#*://}"
      host=$(printf "%s" "$uri" | cut -d':' -f1)
      port=$(printf "%s" "$uri" | cut -s -d':' -f2 | cut -d'/' -f1)
      [ -z "$port" ] && port="5432"
      db="${POSTGRES_DB:-SistemaAcademicoDb}"
      user="${POSTGRES_USER:-postgres}"
      pass="${POSTGRES_PASSWORD:-postgres}"
      printf "Host=%s;Port=%s;Database=%s;Username=%s;Password=%s" "$host" "$port" "$db" "$user" "$pass"
      return 0
      ;;
  esac

  printf "%s" "$input"
}

if [ -x "/app/migrate" ]; then
  echo "==> Executando migrations..."
  if [ -n "$ConnectionStrings__Default" ]; then
    CS=$(normalize_cs "$ConnectionStrings__Default")
    /app/migrate --connection "$CS"
  elif [ -n "$DATABASE_URL" ] || [ -n "$POSTGRES_URL" ] || [ -n "$POSTGRESQL_URL" ]; then
    RAW="${DATABASE_URL:-${POSTGRES_URL:-$POSTGRESQL_URL}}"
    CS=$(normalize_cs "$RAW")
    /app/migrate --connection "$CS"
  else
    /app/migrate
  fi
  echo "==> Migrations concluidas."
fi

echo "==> Iniciando API..."
exec dotnet Scola360.Academico.dll
