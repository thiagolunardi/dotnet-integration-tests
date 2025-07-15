# .NET Integration Tests Samples
[![Build Project](https://github.com/thiagolunardi/dotnet-integration-tests/actions/workflows/build.yml/badge.svg)](https://github.com/thiagolunardi/dotnet-integration-tests/actions/workflows/build.yml)

A curated collection of **.NET 9+** integration test samples — showcasing best practices for testing common backend scenarios.

---

## 🚀 Features

1. **ASP.NET Core WebAPI**  
   Showcase `WebApplicationFactory` and HTTP client testing to validate controllers, routes, status codes, and headers.

1. **SQL Server Integration**  
   Run real DB tests via Docker-based SQL Server, including database migrations.

1. **RabbitMQ Message Queues**  
   Set up RabbitMQ and perform message-driven assertions.

1. **Email Assertions**  
   Use Mailpit to mock SMTP servers and validate email sending in tests.

1. **Planned Additions**
    - NoSQL database (e.g., MongoDB)
    - Blob storage (local or Azure emulator)    

---

## 📂 Repository Structure
```bash
.
├── src/ # Source code folder (WebAPI, database, models, message workers...)
│ ├── Common/ # Shared services, utilities, and extensions
│ ├── Contracts/ # Message contracts and DTOs
│ ├── Database/ # Database migrations and context
│ ├── MessageProcessor / # Message processing logic (e.g., RabbitMQ consumers)
│ ├── Models / # Domain models and entities
│ ├── Tests/ # Integration tests for the WebAPI and MessageProcessor
│ ├── WebApi/ # ASP.NET Core WebAPI project
│ └── ... # Future tests: NoSQL, blob storage
└── .github/ # CI workflows (GitHub Actions + Docker support)
```

---

## 🧪 How to Run

1. Start **Docker** for dependency services:
   1. SQL Server: 
      ```powershell
      docker run --name sqlserver `
        -p 1433:1433 --hostname sqlserver `
        -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Str0ng!Database?P@ssword0" `
        -d mcr.microsoft.com/mssql/server:2022-latest
      ``` 
   1. RabbitMQ:
      ```powershell
      docker run --name rabbitmq `
        -p 15672:15672 `
        -d masstransit/rabbitmq
      ``` 
      
   1. Mailpit:
      ```powershell
      docker run --name mailpit `
        -p 8025:8025 -p 1025:1025 `
        --restart unless-stopped `         
        -d axllent/mailpit
      ```

1. Use `dotnet test` to run all tests.

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
| Email assertion (Inbox mock) | 🟢 Done     |
| NoSQL DB (e.g., MongoDB)     | 🟡 Upcoming |
| Blob Storage (emulator)      | 🟡 Upcoming |

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
