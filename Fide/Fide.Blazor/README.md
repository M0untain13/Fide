# Fide.Blazor

## Синхронизация схемы БД
При изменении схемы данных, нужно будет выполнить следующие шаги:
1. Выполнить команду `dotnet ef migrations add <последняя_миграция>`
2. Выполнить команду `dotnet ef migrations script <предыдущая_миграция> <последняя_миграция> --output script.sql`
3. Переименовать script.sql в <полное_название_миграции>.sql
4. Добавить к скрипту следующие строчки:
```
IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'<полное_название_миграции>')
BEGIN

... скрипт миграции

END;
GO
```
5. Переместить полученный файл в Fide.Blazor\Data\MsSqlTools\scripts\

Теперь при запуске синхронизатора (контейнера из образа Fide.Blazor\Data\MsSqlTools\Dockerfile), новый скрипт будет применен к БД.

P.S. если контейнер не может найти файл init.sh, значит надо поменять CRLF на LF в этом файле перед сборкой образа.