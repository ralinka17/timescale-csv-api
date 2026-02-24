# timescale-csv-api
Web API на ASP.NET Core 8 для загрузки CSV-файлов, валидации, сохранения в TimescaleDB и расчёта аггрегатов (тестовое задание / стажировка)

## Как запустить

1. Запустить PostgreSQL + TimescaleDB в Docker:
   bash
   docker-compose up -d

2. Применить миграции
    dotnet ef database update

4. Запустить проект
   dotnet run
