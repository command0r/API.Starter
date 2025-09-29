# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy solution and project files
COPY *.sln .
COPY src/Core/Domain/Domain.csproj src/Core/Domain/
COPY src/Core/Shared/Shared.csproj src/Core/Shared/
COPY src/Core/Application/Application.csproj src/Core/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/Data/Migrations/Migrations.csproj src/Data/Migrations/
COPY src/WebApi/WebApi.csproj src/WebApi/
COPY tests/Infrastructure.Test/Infrastructure.Test.csproj tests/Infrastructure.Test/
COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY dotnet.ruleset .
COPY stylecop.json .

# Restore packages
RUN dotnet restore

# Copy source code
COPY src/ src/
COPY tests/ tests/

# Build and publish
WORKDIR /source/src/WebApi
RUN dotnet publish -c Release -o /app --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create required directories
RUN mkdir -p /app/Files

# Copy published app
COPY --from=build /app .

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser
RUN chown -R appuser:appuser /app
USER appuser

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "WebApi.dll"]