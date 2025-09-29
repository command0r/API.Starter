using API.Starter.Application.Common.Persistence;
using API.Starter.Infrastructure.Common;
using Microsoft.Extensions.Options;
using Npgsql;

namespace API.Starter.Infrastructure.Persistence.ConnectionString;

public class ConnectionStringSecurer : IConnectionStringSecurer
{
    private const string HiddenValueDefault = "*******";
    private readonly DatabaseSettings _dbSettings;

    public ConnectionStringSecurer(IOptions<DatabaseSettings> dbSettings) =>
        _dbSettings = dbSettings.Value;

    public string? MakeSecure(string? connectionString, string? dbProvider)
    {
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }

        if (string.IsNullOrWhiteSpace(dbProvider))
        {
            dbProvider = _dbSettings.DBProvider;
        }

        return dbProvider?.ToLower() switch
        {
            DbProviderKeys.Npgsql => MakeSecureNpgsqlConnectionString(connectionString),
            // Future SQL Server Support - Uncomment when needed
            // DbProviderKeys.SqlServer => MakeSecureSqlConnectionString(connectionString),
            _ => connectionString
        };
    }

    // Future SQL Server Support - Uncomment when needed
    // private string MakeSecureSqlConnectionString(string connectionString)
    // {
    //     var builder = new SqlConnectionStringBuilder(connectionString);
    //
    //     if (!string.IsNullOrEmpty(builder.Password) || !builder.IntegratedSecurity)
    //     {
    //         builder.Password = HiddenValueDefault;
    //     }
    //
    //     if (!string.IsNullOrEmpty(builder.UserID) || !builder.IntegratedSecurity)
    //     {
    //         builder.UserID = HiddenValueDefault;
    //     }
    //
    //     return builder.ToString();
    // }

    private string MakeSecureNpgsqlConnectionString(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        if (!string.IsNullOrEmpty(builder.Password))
        {
            builder.Password = HiddenValueDefault;
        }

        if (!string.IsNullOrEmpty(builder.Username))
        {
            builder.Username = HiddenValueDefault;
        }

        return builder.ToString();
    }
}