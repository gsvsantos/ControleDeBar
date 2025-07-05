using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Infraestrutura.Arquivos.Compartilhado;

namespace ControleDeBar.Infraestrutura.Arquivos.ModuloMesa;

public class RepositorioMesaEmArquivo : RepositorioBaseEmArquivo<Mesa>, IRepositorioMesa
{
    public RepositorioMesaEmArquivo(ContextoDados contexto) : base(contexto) { }

    public void DesocuparMesa(Mesa mesa)
    {
        throw new NotImplementedException();
    }

    public void OcuparMesa(Mesa mesa)
    {
        throw new NotImplementedException();
    }

    protected override List<Mesa> ObterRegistros()
    {
        return contexto.Mesas;
    }
}
