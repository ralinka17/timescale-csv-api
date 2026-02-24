# timescale-csv-api

Web API на ASP.NET Core 8 для загрузки CSV-файлов, валидации и расчёта аггрегатов с временными рядами на базе TimescaleDB и PostgreSQL. (тестовое задание / стажировка)

## Как запустить

### Предварительные требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

1. **Клонировать репозиторий**

   ```bash
   git clone https://github.com/ralinka0017/TimescaleApi.git
   cd TimescaleApi
   ```

2. **Запустить базу данных в Docker**

```bash
   docker-compose up -d
```

3. **Применить миграции**

```bash
   dotnet ef database update
```

4. **Запустить приложение**

```bash
   dotnet run
```

5. **Открыть Swagger**
   Swagger откроется на: https://localhost:7236 (или http://localhost:5167)

### Примеры запросов

**POST /api/values/upload — загрузка и обработка CSV-файла**

```bash
   curl -X POST "https://localhost:7236/api/values/upload" -F "file=@test.csv"
```

**GET /api/values/last10 — последние 10 значений по файлу**

```bash
   curl "https://localhost:7236/api/values/last10?fileName=test"
```

**GET /api/results — результаты с фильтрами**

```bash
   curl "https://localhost:7236/api/results?fileName=test"
   curl "https://localhost:7236/api/results?MinAvgValue=10&MaxAvgValue=100"
```

### Запуск тестов

```bash
   dotnet test
```
