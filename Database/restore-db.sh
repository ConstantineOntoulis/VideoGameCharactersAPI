#!/bin/bash
set -e

echo "Waiting for SQL Server on host 'db'..."

for i in {1..20}; do
  if /opt/mssql-tools18/bin/sqlcmd -S db -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" > /dev/null 2>&1; then
    echo "SQL Server is reachable."
    break
  fi

  if [ "$i" -eq 20 ]; then
    echo "SQL Server did not become reachable in time."
    exit 1
  fi

  echo "Still waiting for SQL Server..."
  sleep 5
done

echo "Starting database restore..."

/opt/mssql-tools18/bin/sqlcmd -S db -U sa -P "$MSSQL_SA_PASSWORD" -C -b -Q "
IF DB_ID(N'VideoGameCharactersDb') IS NULL
BEGIN
    RESTORE DATABASE [VideoGameCharactersDb]
    FROM DISK = N'/var/opt/mssql/backup/VideoGameCharactersDb.bak'
    WITH
        MOVE N'VideoGameCharactersDb' TO N'/var/opt/mssql/data/VideoGameCharactersDb.mdf',
        MOVE N'VideoGameCharactersDb_log' TO N'/var/opt/mssql/data/VideoGameCharactersDb_log.ldf',
        RECOVERY,
        REPLACE;
END
"

echo "Database restore step finished."