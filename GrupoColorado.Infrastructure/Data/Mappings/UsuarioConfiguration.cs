using GrupoColorado.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrupoColorado.Infrastructure.Data.Mappings
{
  public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
  {
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
      builder.ToTable("Usuarios");

      builder.HasKey(u => u.CodigoUsuario);

      builder.Property(u => u.CodigoUsuario)
          .ValueGeneratedOnAdd();

      builder.Property(u => u.Nome)
          .IsRequired()
          .HasMaxLength(250);

      builder.Property(u => u.Email)
          .IsRequired()
          .HasMaxLength(250);

      builder.Property(u => u.Senha)
          .IsRequired()
          .HasMaxLength(50);

      builder.Property(t => t.Ativo).HasDefaultValue(true);

      builder.Property(u => u.DataInsercao)
          .HasDefaultValueSql("getdate()");
    }
  }
}