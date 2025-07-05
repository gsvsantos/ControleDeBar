using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("contas")]
public class ContaController : Controller
{
    private readonly IRepositorioConta repositorioConta;
    private readonly IRepositorioMesa repositorioMesa;
    private readonly IRepositorioGarcom repositorioGarcom;
    private readonly IRepositorioProduto repositorioProduto;

    public ContaController(IRepositorioConta repositorioConta, IRepositorioMesa repositorioMesa,
        IRepositorioGarcom repositorioGarcom, IRepositorioProduto repositorioProduto)
    {
        this.repositorioConta = repositorioConta;
        this.repositorioMesa = repositorioMesa;
        this.repositorioGarcom = repositorioGarcom;
        this.repositorioProduto = repositorioProduto;
    }

    [HttpGet]
    public IActionResult Index(string status, Guid? garcomId)
    {
        List<Conta> contas = status switch
        {
            "abertas" => repositorioConta.SelecionarContasAbertas(),
            "fechadas" => repositorioConta.SelecionarContasFechadas(),
            _ => repositorioConta.SelecionarRegistros(),
        };

        if (garcomId.HasValue)
            contas = [.. contas.Where(c => c.Garcom.Id == garcomId.Value)];

        List<Garcom> garcons = repositorioGarcom.SelecionarRegistros();

        VisualizarContasViewModel visualizarVM = new(contas, garcons, status, garcomId);

        return View(visualizarVM);
    }

    [HttpGet("abrir")]
    public IActionResult Abrir()
    {
        List<Mesa> mesas = repositorioMesa.SelecionarRegistros();
        List<Garcom> garcons = repositorioGarcom.SelecionarRegistros();

        AbrirContaViewModel abrirVM = new(mesas, garcons);

        return View(abrirVM);
    }

    [HttpPost("abrir")]
    [ValidateAntiForgeryToken]
    public IActionResult Abrir(AbrirContaViewModel abrirVM)
    {
        foreach (Conta c in repositorioConta.SelecionarRegistros())
        {
            if (c.Titular.Equals(abrirVM.Titular) && c.EstaAberta)
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma conta aberta para este titular.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(abrirVM);

        List<Mesa> mesas = repositorioMesa.SelecionarRegistros();
        List<Garcom> garcons = repositorioGarcom.SelecionarRegistros();

        Conta novaConta = abrirVM.ParaEntidade(mesas, garcons);

        repositorioConta.CadastrarRegistro(novaConta);
        repositorioMesa.OcuparMesa(novaConta.Mesa);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet, Route("/contas/{id:guid}/fechar")]
    public IActionResult Fechar(Guid id)
    {
        Conta contaSelecionada = repositorioConta.SelecionarRegistroPorId(id)!;

        FecharContaViewModel fecharContaVM = new(
            contaSelecionada.Id,
            contaSelecionada.Titular,
            contaSelecionada.Mesa.Numero,
            contaSelecionada.Garcom.Nome,
            contaSelecionada.CalcularValorTotal(),
            contaSelecionada.Pedidos);

        return View(fecharContaVM);
    }

    [HttpPost, Route("/contas/{id:guid}/fechar")]
    public IActionResult FecharConfirmado(Guid id)
    {
        Conta contaSelecionada = repositorioConta.SelecionarRegistroPorId(id)!;

        repositorioConta.FecharConta(contaSelecionada);
        repositorioMesa.DesocuparMesa(contaSelecionada.Mesa);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet, Route("/contas/{id:guid}/gerenciar-pedidos")]
    public IActionResult GerenciarPedidos(Guid id)
    {
        Conta contaSelecionada = repositorioConta.SelecionarRegistroPorId(id)!;
        List<Produto> produtos = repositorioProduto.SelecionarRegistros();

        GerenciarPedidosViewModel gerenciarPedidosVm = new(
            contaSelecionada,
            produtos);

        return View(gerenciarPedidosVm);
    }

    [HttpPost, Route("/contas/{id:guid}/adicionar-pedido")]
    public IActionResult AdicionarPedido(Guid id, AdicionarPedidoViewModel adicionarPedidoVm)
    {
        Conta contaSelecionada = repositorioConta.SelecionarRegistroPorId(id)!;
        Produto produtoSelecionado = repositorioProduto.SelecionarRegistroPorId(adicionarPedidoVm.IdProduto)!;

        Pedido novoPedido = new()
        {
            Id = Guid.NewGuid(),
            Produto = produtoSelecionado,
            QuantidadeSolicitada = adicionarPedidoVm.QuantidadeSolicitada
        };

        repositorioConta.AdicionarPedido(contaSelecionada, novoPedido);
        contaSelecionada.RegistrarPedido(produtoSelecionado, novoPedido.QuantidadeSolicitada);

        List<Produto> produtos = repositorioProduto.SelecionarRegistros();

        GerenciarPedidosViewModel gerenciarPedidosVm = new(
            contaSelecionada,
            produtos);

        return RedirectToAction(nameof(GerenciarPedidos), new { id });
    }

    [HttpPost, Route("/contas/{id:guid}/remover-pedido/{idPedido:guid}")]
    public IActionResult RemoverPedido(Guid id, Guid idPedido)
    {
        Conta contaSelecionada = repositorioConta.SelecionarRegistroPorId(id)!;
        Pedido pedidoRemovido = contaSelecionada.RemoverPedido(idPedido);

        repositorioConta.RemoverPedido(contaSelecionada, pedidoRemovido);

        List<Produto> produtos = repositorioProduto.SelecionarRegistros();

        GerenciarPedidosViewModel gerenciarPedidosVm = new(
            contaSelecionada,
            produtos);

        return RedirectToAction(nameof(GerenciarPedidos), new { id });
    }

    [HttpGet("faturamento")]
    public IActionResult Faturamento(DateTime? data)
    {
        if (!data.HasValue)
            return View();

        List<Conta> contas = repositorioConta.SelecionarContasPeriodo(data.GetValueOrDefault());

        FaturamentoViewModel faturamentoVM = new(contas);

        return View(faturamentoVM);
    }
}
