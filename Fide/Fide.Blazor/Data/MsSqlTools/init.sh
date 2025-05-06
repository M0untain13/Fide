#!/bin/bash

echo "Wait for database connection..."

timeout=120
while ! /opt/mssql-tools/bin/sqlcmd -S $DATABASE_HOST -U SA -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" &> /dev/null; do
    sleep 1
    ((timeout--))
    if [ $timeout -le 0 ]; then
        echo "Timeout waiting for SQL Server"
        exit 1
    fi
done

/opt/mssql-tools/bin/sqlcmd -S $DATABASE_HOST -U SA -P "$MSSQL_SA_PASSWORD" \
  -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$DATABASE_NAME') CREATE DATABASE $DATABASE_NAME;" &> /dev/null; do

echo "Executing SQL scripts..."
for file in scripts/*.sql; do
  echo "Executing $file"
  /opt/mssql-tools/bin/sqlcmd -S $DATABASE_HOST -U SA -P "$MSSQL_SA_PASSWORD" -i "$file" -b -r
  if [ $? -ne 0 ]; then
    exit 1
  fi
done
echo "All SQL scripts executed successfully."
