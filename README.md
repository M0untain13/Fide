# Fide

# Инструкция по запуску
1. Создать файл .env и прописать переменные
* CONNECTION_STRING - строка подключения к SqlServer
* FIDE_HOST - название хоста Blazor сервера
* AOMACA_HOST - название хоста сервиса анализа
* MINIO_HOST - название хоста S3
* MINIO_ROOT_USER - админ S3
* MINIO_ROOT_PASSWORD - пароль S3

2. Запустить команду `docker-compose up --build`
