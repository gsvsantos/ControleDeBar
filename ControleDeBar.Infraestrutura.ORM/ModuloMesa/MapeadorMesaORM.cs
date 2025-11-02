using ControleDeBar.Dominio.ModuloMesa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeBar.Infraestrutura.ORM.ModuloMesa;
public class MapeadorMesaORM : IEntityTypeConfiguration<Mesa>
{
    public void Configure(EntityTypeBuilder<Mesa> builder)
    {
        builder.ToTable("TBMesa");

        builder.Property(m => m.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(m => m.Numero)
            .IsRequired();

        builder.Property(m => m.Capacidade)
            .IsRequired();

        builder.Property(m => m.EstaOcupada)
            .IsRequired();
    }
}
