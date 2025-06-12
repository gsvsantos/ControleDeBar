using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.WebApp.Models;

namespace ControleDeBar.WebApp.Extensions;

public static class ProdutoExtensions
{
    public static Produto ParaEntidade(this FormularioProdutoViewModel formularioVM)
    {
        return new(
            formularioVM.Nome,
            formularioVM.Preco);
    }
    public static DetalhesProdutoViewModel ParaDetalhesVM(this Produto produto)
    {
        return new(
            produto.Id,
            produto.Nome,
            produto.Preco);
    }
}
