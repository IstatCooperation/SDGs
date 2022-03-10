FROM microsoft/mssql-server-linux:latest

COPY ./PxWeb/indicatordata_SDGs_PxWeb.sql .
COPY ./PxWeb/init_PxWeb.sql .
COPY ./PxWeb/nordic_model_change.sql .
COPY ./PxWeb/populate_PxWeb.sql .

ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Express
ENV SA_PASSWORD=Pcbs2020!

CMD ( /opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" && /opt/mssql-tools/bin/sqlcmd -S 127.0.0.1,1433 -U SA -P 'Pcbs2020!' -d master -i  init_PxWeb.sql
