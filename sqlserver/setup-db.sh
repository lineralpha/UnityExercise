#!/bin/bash

# run the setup script to create the DB
for i in {1..60}; do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i init-db.sql
    if [[ $? = 0 ]]; then
        echo "init-db.sql completed."
        break
    else
        echo "waiting for sql server to get up ..."
        sleep 1
    fi
done
