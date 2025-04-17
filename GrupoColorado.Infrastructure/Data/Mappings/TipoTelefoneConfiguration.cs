using GrupoColorado.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrupoColorado.Infrastructure.Data.Mappings
{
  public class TipoTelefoneConfiguration : IEntityTypeConfiguration<TipoTelefone>
  {
    public void Configure(EntityTypeBuilder<TipoTelefone> builder)
    {
      builder.ToTable("TiposTelefone");

      builder.HasKey(t => t.CodigoTipoTelefone);

      builder.Property(t => t.CodigoTipoTelefone)
          .ValueGeneratedOnAdd();

      builder.Property(t => t.DescricaoTipoTelefone).IsRequired().HasMaxLength(80);
      builder.Property(t => t.DataInsercao).HasDefaultValueSql("getdate()");

      builder
        .HasOne(tt => tt.Usuario)
        .WithMany(u => u.TiposTelefone)
        .HasForeignKey(tt => tt.UsuarioInsercao)
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}