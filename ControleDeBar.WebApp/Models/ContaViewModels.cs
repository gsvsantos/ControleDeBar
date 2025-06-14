using System.ComponentModel.DataAnnotations;
using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControleDeBar.WebApp.Models;

public class AbrirContaViewModel
{
    [Required(ErrorMessage = "Titular é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do item deve ter entre 3 e 100 caracteres")]
    public string Titular { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mesa é obrigatório")]
    public Guid MesaId { get; set; }
    public List<SelectListItem> MesasDisponiveis { get; set; } = [];

    [Required(ErrorMessage = "Garçom é obrigatório")]
    public Guid GarcomId { get; set; }
    public List<SelectListItem> GarconsDisponiveis { get; set; } = [];

    public AbrirContaViewModel() { }
    public AbrirContaViewModel(List<Mesa> mesas, List<Garcom> garcons)
    {
        foreach (Mesa m in mesas)
        {
            SelectListItem mesaDisponivel = new(
                m.Numero.ToString(),
               m.Id.ToString());

            MesasDisponiveis.Add(mesaDisponivel);
        }

        foreach (Garcom g in garcons)
        {
            SelectListItem nomeDisponivel = new(
                g.Nome,
                g.Id.ToString());

            GarconsDisponiveis.Add(nomeDisponivel);
        }
    }
}

public class FecharContaViewModel
{
    public Guid Id { get; set; }
    public string Titular { get; set; } = string.Empty;
    public int Mesa { get; set; }
    public string Garcom { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }

    public FecharContaViewModel() { }
    public FecharContaViewModel(Guid id, string titular, int mesa, string garcom, decimal valorTotal)
    {
        Id = id;
        Titular = titular;
        Mesa = mesa;
        Garcom = garcom;
        ValorTotal = valorTotal;
    }
}

public class VisualizarContasViewModel
{
    public List<DetalhesContaViewModel> Registros { get; set; } = [];
    public List<SelectListItem> GarconsDisponiveis { get; set; } = [];
    public Guid? GarcomId { get; set; }
    public string Status { get; set; } = string.Empty;

    public VisualizarContasViewModel(List<Conta> contas, List<Garcom> garcons, string status, Guid? garcomId)
    {
        foreach (Conta c in contas)
            Registros.Add(c.ParaDetalhesVM());

        foreach (Garcom g in garcons)
        {
            GarconsDisponiveis.Add(new SelectListItem
            {
                Text = g.Nome,
                Value = g.Id.ToString(),
                Selected = garcomId.HasValue && g.Id == garcomId.Value
            });
        }

        GarcomId = garcomId;
        Status = status;
    }
}

public class DetalhesContaViewModel
{
    public Guid Id { get; set; }
    public string Titular { get; set; }
    public int Mesa { get; set; }
    public string Garcom { get; set; }
    public bool EstaAberta { get; set; }
    public decimal ValorTotal { get; set; }
    public List<PedidoContaViewModel> Pedidos { get; set; } = [];

    public DetalhesContaViewModel(Guid id, string titular, int mesa, string garcom, bool estaAberta, decimal valorTotal, List<Pedido> pedidos)
    {
        Id = id;
        Titular = titular;
        Mesa = mesa;
        Garcom = garcom;
        EstaAberta = estaAberta;
        ValorTotal = valorTotal;

        foreach (Pedido p in pedidos)
        {
            PedidoContaViewModel pedidoVM = new(
                p.Id,
                p.Produto.Nome,
                p.QuantidadeSolicitada,
                p.CalcularTotalParcial());

            Pedidos.Add(pedidoVM);
        }
    }
}

public class PedidoContaViewModel
{
    public Guid Id { get; set; }
    public string Produto { get; set; } = string.Empty;
    public int QuantidadeSolicitada { get; set; }
    public decimal TotalParcial { get; set; }

    public PedidoContaViewModel() { }
    public PedidoContaViewModel(Guid id, string produto, int quantidadeSolicitada, decimal totalParcial)
    {
        Id = id;
        Produto = produto;
        QuantidadeSolicitada = quantidadeSolicitada;
        TotalParcial = totalParcial;
    }
}

public class GerenciarPedidosViewModel
{
    public DetalhesContaViewModel Conta { get; set; } = null!;
    public List<SelectListItem> Produtos { get; set; } = [];

    public GerenciarPedidosViewModel() { }
    public GerenciarPedidosViewModel(Conta conta, List<Produto> produtos) : this()
    {
        Conta = conta.ParaDetalhesVM();

        foreach (Produto p in produtos)
        {
            SelectListItem selectItem = new(
                p.Nome,
                p.Id.ToString());

            Produtos.Add(selectItem);
        }
    }
}

public class AdicionarPedidoViewModel
{
    public Guid IdProduto { get; set; }
    public int QuantidadeSolicitada { get; set; }
}
