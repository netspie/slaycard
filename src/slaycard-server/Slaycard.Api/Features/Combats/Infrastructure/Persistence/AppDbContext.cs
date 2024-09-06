using Microsoft.EntityFrameworkCore;

namespace Slaycard.Api.Features.Combats.Infrastructure.Persistence;

public class AppDbContext(
    IConfiguration configuration,
    DbContextOptions options) : DbContext(options)
{
    private readonly IConfiguration _configuration = configuration;

    public DbSet<Player> Players { get; private set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("SlaycardDb");

        optionsBuilder.UseNpgsql(connectionString);
    }
}

public class Player(string id, string name)
{
    public string Id { get; set; } = id;
    public string Name { get; set; } = name;
}
