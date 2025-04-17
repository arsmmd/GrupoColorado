using GrupoColorado.Infrastructure.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace GrupoColorado.Infrastructure.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
      modelBuilder.ApplyConfiguration(new ClienteConfiguration());
      modelBuilder.ApplyConfiguration(new TelefoneConfiguration());
      modelBuilder.ApplyConfiguration(new TipoTelefoneConfiguration());
    }
  }
}