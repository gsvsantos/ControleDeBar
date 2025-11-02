using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Infraestrutura.ORM.Compartilhado;

namespace ControleDeBar.Infraestrutura.ORM.ModuloMesa;

public class RepositorioMesaORM : RepositorioBaseORM<Mesa>, IRepositorioMesa
{
    public RepositorioMesaORM(ControleDeBarDbContext contexto) : base(contexto) { }

    public bool MesaContemVinculos(Guid mesaId, List<Conta> contas)
    {
        return contas.Any(c => c.Mesa.Id == mesaId);
    }

    public bool VerificarMesaCheia(Mesa mesa, List<Conta> contasAbertas)
    {
        return mesa.EstaOcupada && contasAbertas.Count(c => c.Mesa.Id == mesa.Id) == mesa.Capacidade;
    }
}
