using ControleDeBar.Dominio.ModuloMesa;
using Microsoft.EntityFrameworkCore;

namespace ControleDeBar.Infraestrutura.ORM.Compartilhado;
public class ControleDeBarDbContext : DbContext
{
    public DbSet<Mesa> Mesas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ControleDeBarDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
