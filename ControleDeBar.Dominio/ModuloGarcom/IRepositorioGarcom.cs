using ControleDeBar.Dominio.Compartilhado;
using ControleDeBar.Dominio.ModuloConta;

namespace ControleDeBar.Dominio.ModuloGarcom;

public interface IRepositorioGarcom : IRepositorio<Garcom>
{
    public bool GarcomContemVinculos(Guid garcomId, List<Conta> contas);
}
