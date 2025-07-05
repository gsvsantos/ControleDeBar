using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.ModuloMesa;

public interface IRepositorioMesa : IRepositorio<Mesa>
{
    public void OcuparMesa(Mesa mesa);
    public void DesocuparMesa(Mesa mesa);
}
