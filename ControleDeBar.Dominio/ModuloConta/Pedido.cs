using ControleDeBar.Dominio.ModuloProduto;
using System.Diagnostics.CodeAnalysis;

namespace ControleDeBar.Dominio.ModuloConta;

public class Pedido
{
    public Guid Id { get; set; }
    public Conta Conta { get; set; } = null!;
    public Produto Produto { get; set; } = null!;
    public int QuantidadeSolicitada { get; set; }

    [ExcludeFromCodeCoverage]
    public Pedido() { }
    public Pedido(Produto produto, int quantidadeSolicitada) : this()
    {
        Id = Guid.NewGuid();
        Produto = produto;
        QuantidadeSolicitada = quantidadeSolicitada;
    }

    public decimal CalcularTotalParcial()
    {
        return Produto.Preco * QuantidadeSolicitada;
    }

    public override string ToString()
    {
        return $"{QuantidadeSolicitada}x {Produto.Preco}";
    }
}
