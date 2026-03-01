#!/bin/bash

echo "Waiting for SQL Server to start"

until /opt/mssql-tools18/bin/sqlcmd -S 127.0.0.1,1433 -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" > /dev/null 2>&1
do
  sleep 5
done

echo "SQL Server is ready. Running schema"

 /opt/mssql-tools18/bin/sqlcmd -S 127.0.0.1,1433 -U sa -P "$SA_PASSWORD" -C -i /docker-entrypoint-initdb.d/schema.sql

echo "Database initialized successfully."