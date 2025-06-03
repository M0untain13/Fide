FROM mcr.microsoft.com/mssql-tools
copy . .

# Устанавливаем dos2unix
RUN apt-get update && apt-get install -y dos2unix

# Преобразуем CRLF → LF и делаем исполняемым
RUN dos2unix ./init.sh && \
    chmod +x ./init.sh

CMD ["./init.sh"]