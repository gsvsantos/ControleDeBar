using ControleDeBar.Dominio.Compartilhado;
using ControleDeBar.Dominio.ModuloConta;

namespace ControleDeBar.Dominio.ModuloMesa;

public interface IRepositorioMesa : IRepositorio<Mesa>
{
    public bool VerificarMesaCheia(Mesa mesa, List<Conta> contasAbertas);
    public void OcuparMesa(Mesa mesa);
    public void DesocuparMesa(Mesa mesa);
    public bool MesaContemVinculos(Guid mesaId, List<Conta> contas);
}
