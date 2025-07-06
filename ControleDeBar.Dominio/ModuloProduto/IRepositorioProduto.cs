using ControleDeBar.Dominio.Compartilhado;
using ControleDeBar.Dominio.ModuloConta;

namespace ControleDeBar.Dominio.ModuloProduto;

public interface IRepositorioProduto : IRepositorio<Produto>
{
    public bool ProdutoContemVinculos(Guid produtoId, List<Conta> contas);
}
