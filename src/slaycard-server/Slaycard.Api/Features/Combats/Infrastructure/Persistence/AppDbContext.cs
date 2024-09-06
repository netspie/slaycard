using Microsoft.EntityFrameworkCore;

namespace Slaycard.Api.Features.Combats.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions options) : DbContext(options);
