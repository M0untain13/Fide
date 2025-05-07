# Fide

# Инструкция по запуску
1. Создать файл .env и прописать переменные
```
# Blazor Server
FIDE_HOST=fide

# БД
DATABASE_HOST=mssql
DATABASE_NAME=FideDB
MSSQL_SA_PASSWORD=yourpassword
ACCEPT_EULA=Y

# Сервис анализа
AOMACA_HOST=aomaca

# Локальное S3
MINIO_HOST=minio
MINIO_ROOT_USER=minioadmin
MINIO_ROOT_PASSWORD=yourpassword

# Данные почтового сервера
SMTP_HOST=smtp.server.com
SMTP_PORT=47
SMTP_EMAIL=your@mail.ru
SMTP_PASSWORD=yourpassword
```

2. Для сервиса Fide отредактировать appsettings.json, добавив секции с данными
```
{
    ...,
    "SMTP": {
        "Host": "smtp.server.com",
        "Port": 47,
        "Email": "your@mail.ru",
        "Password": "yourpassword"
    },
    "S3": {
        "ServiceURL": "http://localhost:9000",
        "AccessKey": "minioadmin",
        "SecretKey": "yourpassword",
        "BucketName": "filesbucket",
        "ForcePathStyle": true
    },
    "Aomaca": {
        "ServiceUrl": "http://localhost:8081"
    }
}
/// При необходимости можно отредактировать строку подключения к БД ///
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=mssql;Database=FideDB;Trusted_Connection=false;MultipleActiveResultSets=true;Password=yourpassword;User Id=sa;Trust Server Certificate=True"
    },
    ... 
}
```

3. Запустить команду `docker-compose up --build`
