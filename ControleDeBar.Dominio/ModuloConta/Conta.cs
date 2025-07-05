using ControleDeBar.Dominio.Compartilhado;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Dominio.ModuloProduto;
using System.Diagnostics.CodeAnalysis;

namespace ControleDeBar.Dominio.ModuloConta;

public class Conta : EntidadeBase<Conta>
{
    public string Titular { get; set; } = string.Empty;
    public Mesa Mesa { get; set; } = null!;
    public Garcom Garcom { get; set; } = null!;
    public DateTime Abertura { get; set; }
    public DateTime? Fechamento { get; set; }
    public bool EstaAberta { get; set; }
    public decimal ValorTotal { get; set; }
    public List<Pedido> Pedidos { get; set; } = [];

    [ExcludeFromCodeCoverage]
    public Conta() { }
    public Conta(string titular, Mesa mesa, Garcom garcom) : this()
    {
        Id = Guid.NewGuid();
        Titular = titular;
        Mesa = mesa;
        Garcom = garcom;

        Abrir();
    }

    public void Abrir()
    {
        EstaAberta = true;
        Abertura = DateTime.Now;
    }

    public void Fechar()
    {
        EstaAberta = false;
        Fechamento = DateTime.Now;
    }

    public Pedido RegistrarPedido(Produto produto, int quantidadeSolicitada)
    {
        Pedido novoPedido = new(produto, quantidadeSolicitada);

        Pedidos.Add(novoPedido);

        return novoPedido;
    }

    public Pedido RemoverPedido(Pedido pedido)
    {
        Pedidos.Remove(pedido);

        return pedido;
    }

    public Pedido RemoverPedido(Guid idPedido)
    {
        Pedido pedidoSelecionado = Pedidos.FirstOrDefault(p => p.Id == idPedido)!;

        if (pedidoSelecionado == null)
            return null!;

        Pedidos.Remove(pedidoSelecionado);

        return pedidoSelecionado;
    }

    public decimal CalcularValorTotal()
    {
        decimal valorTotal = 0;

        foreach (Pedido p in Pedidos)
            valorTotal += p.CalcularTotalParcial();

        return valorTotal;
    }

    public override void AtualizarRegistro(Conta registroAtualizado)
    {
        EstaAberta = registroAtualizado.EstaAberta;
        Fechamento = registroAtualizado.Fechamento;
    }
}
