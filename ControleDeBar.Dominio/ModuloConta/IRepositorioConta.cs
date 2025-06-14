using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.ModuloConta;

public interface IRepositorioConta : IRepositorio<Conta>
{
    void CadastrarConta(Conta conta);
    Conta SelecionarPorId(Guid idRegistro);
    List<Conta> SelecionarContasAbertas();
    List<Conta> SelecionarContasFechadas();
}
