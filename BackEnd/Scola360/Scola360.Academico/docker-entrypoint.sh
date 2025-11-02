#!/usr/bin/env sh
set -e

normalize_cs() {
  input="$1"
  case "$input" in
    postgres://*|postgresql://*)
      uri="${input#*://}"
      creds="${uri%@*}"
      rest="${uri#*@}"
      user="${creds%%:*}"
      pass="${creds#*:}"
      [ "$pass" = "$creds" ] && pass=""
      hostport="${rest%%/*}"
      dbq="${rest#*/}"
      db="${dbq%%\?*}"
      query="${dbq#*\?}"
      [ "$query" = "$dbq" ] && query=""
      host="${hostport%%:*}"
      port="${hostport#*:}"
      [ "$port" = "$hostport" ] && port="5432"
      cs="Host=$host;Port=$port;Database=$db;Username=$user"
      [ -n "$pass" ] && cs="$cs;Password=$pass"
      # sslmode
      if [ -n "$query" ]; then
        OLD_IFS="$IFS"
        IFS='&'
        for kv in $query; do
          key="${kv%%=*}"; val="${kv#*=}"
          if [ "$key" = "sslmode" ]; then
            cs="$cs;Ssl Mode=$val"
          fi
        done
        IFS="$OLD_IFS"
      fi
      printf "%s" "$cs"
      return 0
      ;;
  esac

  case "$input" in
    *Host=tcp://*)
      hostport=$(printf "%s" "$input" | sed -n 's/.*Host=tcp:\/\/\([^;]*\).*/\1/p')
      host="$hostport"; port=""
      case "$hostport" in
        *:*
          host="${hostport%:*}"
          port="${hostport##*:}"
          ;;
      esac
      out=$(printf "%s" "$input" | sed "s/Host=tcp:\/\/$hostport/Host=$host/i")
      if ! printf "%s" "$out" | grep -qi ";[[:space:]]*Port="; then
        [ -n "$port" ] && out="$out;Port=$port"
      fi
      printf "%s" "$out"
      return 0
      ;;
    tcp://*)
      uri="${input#*://}"
      host="${uri%%:*}"
      rest="${uri#*:}"
      port="${rest%%/*}"
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
