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
        var nlogConnection = _configuration.GetConnectionString("Nlog");
        
        var blogSqlScriptsPath = _configuration.GetConnectionString("BlogSqlScrips");
        var nlogSqlScriptsPath = _configuration.GetConnectionString("NlogSqlScrips");
        
        var appDbContextUpgradeEngine = DeployChanges.To
            .PostgresqlDatabase(defaultConnection)
            .WithScriptsFromFileSystem(blogSqlScriptsPath)
            .LogToConsole()
            .LogScriptOutput()
            .WithVariablesDisabled()
            .Build();
        
        EnsureDatabase.For.PostgresqlDatabase(nlogConnection);
        
        var nlogUpgradeEngine = DeployChanges.To
            .PostgresqlDatabase(nlogConnection)
            .WithScriptsFromFileSystem(nlogSqlScriptsPath)
            .LogToConsole()
            .LogScriptOutput()
            .WithVariablesDisabled()
            .Build();
        
        var blogResult = appDbContextUpgradeEngine.PerformUpgrade();

        if (!blogResult.Successful)
        {
            throw blogResult.Error;
        }
        
        var nlogResult = nlogUpgradeEngine.PerformUpgrade();

        if (!nlogResult.Successful)
        {
            throw nlogResult.Error;
        }
    }
}