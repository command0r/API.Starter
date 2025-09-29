# API.Starter - Clean Architecture .NET 8 Framework

## Introduction

**API.Starter** is a comprehensive, production-ready .NET 8 Web API framework built on Clean Architecture principles. This solution provides a robust foundation for building scalable, maintainable, and enterprise-grade applications, from small projects to complex microservice architectures.

Designed with modern development practices in mind, API.Starter implements Domain-Driven Design (DDD) patterns and offers a well-structured approach to feature development, ensuring clean separation of concerns and optimal code organization.

## Purpose

This framework serves as a **battle-tested starting point** for developers and teams who need:

- **Rapid Development**: Pre-configured infrastructure components and patterns to accelerate project delivery
- **Enterprise Scalability**: Multi-tenant architecture with robust caching, background processing, and real-time capabilities
- **Clean Architecture**: Strict separation of concerns following Clean Architecture and DDD principles
- **Production Readiness**: Comprehensive logging, monitoring, security, and deployment infrastructure
- **Modern .NET 8**: Leveraging the latest .NET features with optimized package management

## Architecture & Components

### Clean Architecture Layers

The solution follows Clean Architecture principles with clear dependency flow:

```
┌─────────────────┐
│     WebAPI      │ ← Presentation Layer (Controllers, Middleware)
└─────────┬───────┘
          │
┌─────────▼───────┐
│ Infrastructure  │ ← External Concerns (Database, External APIs, File System)
└─────────┬───────┘
          │
┌─────────▼───────┐
│  Application    │ ← Business Logic (Use Cases, CQRS Handlers, Interfaces)
└─────────┬───────┘
          │
┌─────────▼───────┐
│    Domain       │ ← Core Business Rules (Entities, Value Objects, Domain Events)
└─────────┬───────┘
          │
┌─────────▼───────┐
│    Shared       │ ← Common Utilities and Constants
└─────────────────┘
```

### Core Components

#### **Domain Layer**
- **Entities**: Core business objects with identity and behavior
- **Value Objects**: Immutable objects representing domain concepts
- **Domain Events**: Business events for decoupled communication
- **Specifications**: Reusable business rules and query logic
- **Interfaces**: Repository and service contracts

#### **Application Layer**
- **CQRS Implementation**: Command/Query separation with MediatR (v12.4.1)
- **Use Cases**: Application-specific business logic
- **DTOs & Mapping**: Data transfer objects with Mapster
- **Validation**: FluentValidation for request validation
- **Interfaces**: Application service contracts

#### **Infrastructure Layer**
- **Entity Framework Core**: PostgreSQL data persistence with audit trails
- **Identity Management**: JWT authentication and role-based authorization
- **Caching**: Redis distributed caching with fallback strategies
- **Background Jobs**: Hangfire for reliable background processing
- **Real-time Communication**: SignalR for live notifications
- **Email Services**: SMTP integration with templating
- **File Storage**: Configurable file management system

#### **WebAPI Layer**
- **RESTful Controllers**: Clean API endpoints with proper HTTP semantics
- **Middleware Pipeline**: Exception handling, logging, and security headers
- **API Documentation**: NSwag/Swagger with custom branding
- **Versioning**: Built-in API versioning support
- **Health Checks**: Comprehensive application health monitoring

### Key Features

#### **🏗️ Architecture & Patterns**
- **Clean Architecture**: Dependency inversion and separation of concerns
- **CQRS + MediatR**: Command/Query separation with pipeline behaviors
- **Repository Pattern**: Data access abstraction with specifications
- **Domain Events**: Decoupled event-driven communication
- **Specification Pattern**: Reusable business rules and queries

#### **🔐 Security & Authentication**
- **JWT Authentication**: Secure token-based authentication
- **Role-Based Authorization**: Granular permission management
- **Multi-Tenancy**: Full tenant isolation with Finbuckle.MultiTenant
- **Security Headers**: OWASP security best practices
- **Audit Logging**: Comprehensive security and compliance auditing

#### **⚡ Performance & Scalability**
- **Redis Caching**: Distributed caching with Entity Framework integration
- **Background Processing**: Reliable job processing with Hangfire
- **Real-time Features**: SignalR with Redis backplane support
- **Database Optimization**: Entity Framework with query specifications
- **Health Monitoring**: Built-in health checks and monitoring

#### **🛠️ Development Experience**
- **Structured Logging**: Serilog with multiple sinks (Console, File, Seq, Elasticsearch)
- **Code Quality**: StyleCop and Roslynator analyzers
- **API Documentation**: Interactive Swagger UI with custom branding
- **Validation**: Comprehensive request/response validation
- **Exception Handling**: Global exception handling with detailed logging

#### **🌐 Enterprise Features**
- **Localization**: Multi-language support infrastructure
- **Email Services**: Template-based email system with MailKit
- **File Management**: Secure file upload and storage
- **CORS Management**: Configurable cross-origin policies
- **Environment Configuration**: Flexible configuration management

## Getting Started

### Prerequisites

- **.NET 8 SDK** or later
- **Docker & Docker Compose** (recommended for dependencies)
- **PostgreSQL** database (provided via Docker)
- **Redis** server (optional, for caching - provided via Docker)

### Quick Start with Docker

1. **Clone the repository**
   ```bash
   git clone https://github.com/command0r/API.Starter.git
   cd API.Starter
   ```

2. **Start all services with Docker Compose**
   ```bash
   cd deployments
   docker-compose up -d
   ```
   This starts PostgreSQL, Seq logging, and the API in containers.

3. **Access the application**
   - **API**: `http://localhost:8080` (HTTP) / `https://localhost:8081` (HTTPS)
   - **Swagger UI**: `http://localhost:8080/swagger`
   - **Seq Logs**: `http://localhost:5341`

### Local Development (IDE)

When developing locally with your IDE (Visual Studio, Rider, VS Code):

1. **Start only infrastructure services**
   ```bash
   cd deployments
   # Stop the API container if running
   docker-compose stop api
   # Or start only PostgreSQL and Seq
   docker-compose up -d postgres seq
   ```

2. **Database Setup**
   - Connection strings in `src/WebApi/Configurations/database.json` and `hangfire.json` are pre-configured with `Host=localhost`
   - On first run, migrations will automatically create the database schema

3. **Run from your IDE**
   ```bash
   # From project root
   dotnet run --project src/WebApi

   # Or with hot reload
   dotnet watch run --project src/WebApi
   ```

4. **Access the API**
   - **Swagger UI**: `https://localhost:5001/swagger`
   - **Health Checks**: `https://localhost:5001/api/health`
   - **Hangfire Dashboard**: `https://localhost:5001/hangfire`

### Switching Between Docker and Local Development

**From Docker to Local IDE:**
1. Stop the API container: `docker-compose stop api`
2. Keep PostgreSQL running: `docker-compose up -d postgres seq`
3. Run from IDE - database connections will use `localhost`

**From Local IDE to Docker:**
1. Stop your IDE's debug session
2. Start all services: `docker-compose up -d`
3. API container will connect using Docker's internal network

**Clean Database Reset:**
```bash
# If you encounter migration issues, reset the database
docker exec postgres-api-starter psql -U postgres -c "DROP DATABASE IF EXISTS \"TenantDb\";"
docker exec postgres-api-starter psql -U postgres -c "CREATE DATABASE \"TenantDb\";"
# Migrations will run automatically on next application start
```

### Testing

Run the test suite:
```bash
dotnet test
```

Use the pre-configured Postman collection located in the `postman/` directory for API testing.

## Development Dependencies

### Required Services

#### **PostgreSQL Database**
```bash
# Using Docker
docker run --name postgres-api-starter -e POSTGRES_PASSWORD=yourpassword -e POSTGRES_DB=apistarter -p 5432:5432 -d postgres:15
```

#### **Redis Cache** (Optional)
```bash
# Using Docker
docker run --name redis-api-starter -p 6379:6379 -d redis:7-alpine
```

### Development Tools

#### **SEQ Console** - Structured Logging Viewer
```bash
# Pull the SEQ image
docker pull datalust/seq

# For development (no authentication)
docker run --name seq -d --restart unless-stopped \
  -e ACCEPT_EULA=Y \
  -e SEQ_FIRSTRUN_NOAUTHENTICATION=true \
  -p 5341:80 datalust/seq:latest

# For production (with authentication)
docker run --name seq -d --restart unless-stopped \
  -e ACCEPT_EULA=Y \
  -e SEQ_FIRSTRUN_ADMINPASSWORD=YourSecurePassword \
  -p 5341:80 datalust/seq:latest
```

Access SEQ at: `http://localhost:5341`

> **Note**: Newer SEQ versions require either authentication setup or explicit opt-out. The development command above disables authentication for local development convenience.

### Docker Compose Commands

Common Docker Compose operations:

```bash
# Start all services
cd deployments && docker-compose up -d

# Start specific services
docker-compose up -d postgres seq  # Just infrastructure

# View logs
docker-compose logs -f api

# Stop services
docker-compose stop api  # Stop specific service
docker-compose down      # Stop and remove all containers
```

## Configuration

### Environment Variables

Key configuration files in `src/WebApi/Configurations/`:

- `appsettings.json` - Application settings
- `database.json` - Database connection strings
- `hangfire.json` - Background job configuration
- `security.json` - JWT and security settings
- `logger.json` - Serilog configuration
- `signalr.json` - Real-time communication settings

### Multi-Tenancy Setup

Configure tenant settings in `database.json`:
```json
{
  "MultiTenancy": {
    "ConnectionString": "Host=localhost;Database=apistarter;Username=postgres;Password=yourpassword;",
    "Root": {
      "Id": "root",
      "Name": "Root Tenant",
      "ConnectionString": "Host=localhost;Database=apistarter_root;Username=postgres;Password=yourpassword;"
    }
  }
}
```

## Project Structure

```
API.Starter/
├── src/
│   ├── Core/
│   │   ├── Domain/              # Core business logic and entities
│   │   ├── Application/         # Use cases and application services
│   │   └── Shared/             # Common utilities and constants
│   ├── Infrastructure/         # External dependencies and infrastructure
│   ├── WebApi/                # API controllers and presentation layer
│   └── Data/
│       └── Migrations/        # Database migration projects
├── tests/
│   └── Infrastructure.Test/   # Test projects
├── postman/                   # API testing collections
├── deployments/              # Deployment configurations
└── scripts/                  # Utility scripts
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For questions, issues, or contributions, please:

- 📫 Open an issue on [GitHub Issues](https://github.com/command0r/API.Starter/issues)
- 💬 Start a discussion on [GitHub Discussions](https://github.com/command0r/API.Starter/discussions)

---

**API.Starter** - Accelerating .NET development with Clean Architecture principles and modern practices.