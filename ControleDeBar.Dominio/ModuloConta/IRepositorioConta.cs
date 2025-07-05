using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.ModuloConta;

public interface IRepositorioConta : IRepositorio<Conta>
{
    List<Conta> SelecionarContasAbertas();
    List<Conta> SelecionarContasFechadas();
    List<Conta> SelecionarContasPeriodo(DateTime data);
    public void AdicionarPedido(Conta conta, Pedido pedido);
    public void RemoverPedido(Conta conta, Pedido pedido);
    public void FecharConta(Conta conta);
}
