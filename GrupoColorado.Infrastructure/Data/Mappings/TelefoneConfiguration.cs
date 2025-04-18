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

      builder
          .HasOne(t => t.Cliente)
          .WithMany(c => c.Telefones)
          .HasForeignKey(t => t.CodigoCliente)
          .OnDelete(DeleteBehavior.Restrict);

      builder
          .HasOne(t => t.TipoTelefone)
          .WithMany(tt => tt.Telefones)
          .HasForeignKey(t => t.CodigoTipoTelefone)
          .OnDelete(DeleteBehavior.Restrict);

      builder
          .HasOne(t => t.Usuario)
          .WithMany(u => u.Telefones)
          .HasForeignKey(t => t.UsuarioInsercao)
          .OnDelete(DeleteBehavior.Restrict);
    }
  }
}