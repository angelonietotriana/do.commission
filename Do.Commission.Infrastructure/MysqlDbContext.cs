

using Do.Commission.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace Do.Commission.Infrastructure;

public class MysqlDbContext : DbContext
{
    public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Position> Positions { get; set; } = null!;
    public DbSet<PositionHistory> PositionHistory { get; set; }
}