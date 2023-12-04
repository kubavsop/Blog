using DbUp;
namespace Blog.API.Data;

public class DatabaseMigrator
{
    private readonly IConfiguration _configuration;

    public DatabaseMigrator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Migrate()
    {
        var defaultConnection = _configuration.GetConnectionString("DefaultConnection");
        var sqlScriptsPath = _configuration.GetConnectionString("SqlScripsPath");
        var nlogConnection = _configuration.GetConnectionString("Nlog");
        
        var appDbContextUpgradeEngine = DeployChanges.To
            .PostgresqlDatabase(defaultConnection)
            .WithScriptsFromFileSystem(sqlScriptsPath)
            .LogToConsole()
            .Build();
        
        
        
        var result = appDbContextUpgradeEngine.PerformUpgrade();

        if (!result.Successful)
        {
            throw new Exception(result.Error.ToString());
        }
    }
}