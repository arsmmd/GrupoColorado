using GrupoColorado.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrupoColorado.Infrastructure.Data.Mappings
{
  public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
  {
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
      builder.ToTable("Clientes");

      builder.HasKey(c => c.CodigoCliente);

      builder.Property(c => c.CodigoCliente)
          .ValueGeneratedOnAdd();

      builder.Property(c => c.RazaoSocial).IsRequired().HasMaxLength(100);
      builder.Property(c => c.NomeFantasia).IsRequired().HasMaxLength(100);
      builder.Property(c => c.TipoPessoa).IsRequired().HasColumnType("char(1)");
      builder.Property(c => c.Documento).IsRequired().HasMaxLength(14);
      builder.Property(c => c.Endereco).IsRequired().HasMaxLength(100);
      builder.Property(c => c.Complemento).HasMaxLength(100);
      builder.Property(c => c.Bairro).IsRequired().HasMaxLength(100);
      builder.Property(c => c.Cidade).IsRequired().HasMaxLength(100);
      builder.Property(c => c.CEP).IsRequired().HasColumnType("char(8)");
      builder.Property(c => c.UF).IsRequired().HasColumnType("char(2)");
      builder.Property(c => c.DataInsercao).HasDefaultValueSql("getdate()");

      builder
        .HasOne(c => c.Usuario)
        .WithMany(u => u.Clientes)
        .HasForeignKey(c => c.UsuarioInsercao)
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}