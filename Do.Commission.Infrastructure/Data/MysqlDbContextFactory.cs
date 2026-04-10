using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Do.Commission.Infrastructure.Data;

/// <summary>
/// Creado para el Add-Migration
/// </summary>
public class MysqlDbContextFactory : IDesignTimeDbContextFactory<MysqlDbContext>
{
    public MysqlDbContext CreateDbContext(string[] args)
    {
        string stringConnection = "Server=localhost;Port=3306;Database=commissions_db;User=root;Password=root;";

        var optionsBuilder = new DbContextOptionsBuilder<MysqlDbContext>();
        optionsBuilder.UseMySql(stringConnection, ServerVersion.AutoDetect(stringConnection ));

        return new MysqlDbContext(optionsBuilder.Options);
    }
}