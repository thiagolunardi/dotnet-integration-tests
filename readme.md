# .NET Integration Tests Samples
[![Build Project](https://github.com/thiagolunardi/dotnet-integration-tests/actions/workflows/build.yml/badge.svg)](https://github.com/thiagolunardi/dotnet-integration-tests/actions/workflows/build.yml)

A curated collection of **.NET 9+** integration test samples — showcasing best practices for testing common backend scenarios.

---

## 🚀 Features

1. **ASP.NET Core WebAPI**  
   Showcase `WebApplicationFactory` and HTTP client testing to validate controllers, routes, status codes, and headers.

2. **SQL Server Integration**  
   Run real DB tests via Docker-based SQL Server or EF Core, replacing live DB context using `ConfigureServices`.

3. **RabbitMQ Message Queues**  
   Set up RabbitMQ via Testcontainers and perform message-driven assertions.

4. **Planned Additions**
    - NoSQL database (e.g., MongoDB)
    - Blob storage (local or Azure emulator)
    - Email assertion (via SMTP mock server)

---

## 📂 Repository Structure
```bash
.
├── src/ # Sample applications (WebAPI, background workers...)
├── tests/
│ ├── WebApi.Tests/ # xUnit + TestServer + SQL Server + RabbitMQ
│ └── ... # Future tests: NoSQL, blob storage, email
├── docs/ # Resources, links & best practices
└── .github/ # CI workflows (GitHub Actions + Docker support)
```


---

## 🧪 How to Run

1. Start **Docker** for dependency services:
   1. SQL Server: 
      ```bash
      docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Str0ng!Database?P@ssword0" \
        -p 1433:1433 --name sqlserver --hostname sqlserver \
        -d mcr.microsoft.com/mssql/server:2022-latest
      ``` 
   1. RabbitMQ:
      ```bash
      docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq
      ``` 
1. Use `dotnet test` to run all tests (unit + integration).

---

## 🎯 Goals & Best Practices

- Use `WebApplicationFactory<TEntryPoint>` for full HTTP request/response flow.
- Replace live services in tests via `builder.ConfigureServices(...)` for test isolation.
- Follow **Arrange → Act → Assert** pattern with real I/O and infrastructure.
- Keep unit tests separate from integration tests for speed and clarity.
- Drop the need for using Postman or manual testing by providing comprehensive integration tests.

---

## 📅 Roadmap

| Feature                      | Status      |
|------------------------------|-------------|
| SQL Database: SQL Server     | 🟢 Done     |
| Messaging: RabbitMQ          | 🟢 Done     |
| NoSQL DB (e.g., MongoDB)     | 🟡 Upcoming |
| Blob Storage (emulator)      | 🟡 Upcoming |
| Email assertion (Inbox mock) | 🟡 Upcoming |

Contributions welcome! Feel free to submit PRs, feature requests or issues.

---

## 🛠️ Contributing

1. Open an issue to discuss your feature or improvement
1. Fork the repo 
1. Create a branch: `feature/nosql-integration`
1. Add your samples/tests
1. Open a PR detailing your additions and run steps

---

## ⚖️ License

MIT — see [LICENSE](LICENSE) for details.

---

### 👍 Why This Matters

Integration tests validate real behavior: complete HTTP pipelines, actual databases, messaging queues, blob storage, and email. This repo provides real-world samples and starter templates so developers can confidently test .NET apps in production-like environments.

---

**Let me know if you'd like code snippets added, badges, or CI examples!**
