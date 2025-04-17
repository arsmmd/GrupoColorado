using GrupoColorado.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrupoColorado.Infrastructure.Data.Mappings
{
  public class TelefoneConfiguration : IEntityTypeConfiguration<Telefone>
  {
    public void Configure(EntityTypeBuilder<Telefone> builder)
    {
      builder.ToTable("Telefones");

      builder.HasKey(t => new { t.CodigoCliente, t.NumeroTelefone });

      builder.Property(t => t.NumeroTelefone).HasMaxLength(11);
      builder.Property(t => t.Operadora).IsRequired().HasMaxLength(100);
      builder.Property(t => t.Ativo).HasDefaultValue(true);
      builder.Property(t => t.DataInsercao).HasDefaultValueSql("getdate()");

      builder.HasOne<Cliente>()
          .WithMany()
          .HasForeignKey(t => t.CodigoCliente)
          .HasForeignKey(c => c.CodigoCliente)
          .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne<TipoTelefone>()
          .WithMany()
          .HasForeignKey(t => t.CodigoTipoTelefone)
          .HasPrincipalKey(tt => tt.CodigoTipoTelefone)
          .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne<Usuario>()
          .WithMany()
          .HasForeignKey(t => t.UsuarioInsercao)
          .HasPrincipalKey(u => u.CodigoUsuario)
          .OnDelete(DeleteBehavior.Restrict);
    }
  }
}