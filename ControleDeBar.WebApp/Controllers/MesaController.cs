using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Infraestrutura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloMesa;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("mesas")]
public class MesaController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IRepositorioMesa repositorioMesa;

    public MesaController()
    {
        contextoDados = new(true);
        repositorioMesa = new RepositorioMesaEmArquivo(contextoDados);
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Mesa> mesas = repositorioMesa.SelecionarRegistros().OrderBy(m => m.Numero).ToList();

        VisualizarMesasViewModel visualizarVM = new(mesas);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        CadastrarMesaViewModel cadastrarVM = new();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarMesaViewModel cadastrarVM)
    {
        foreach (Mesa mesa in repositorioMesa.SelecionarRegistros())
        {
            if (mesa.Numero == cadastrarVM.Numero)

            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma mesa com esse número.");
                break;
            }
        }

        if (!ModelState.IsValid)
        {
            return View(cadastrarVM);
        }

        Mesa novaMesa = cadastrarVM.ParaEntidade();

        repositorioMesa.CadastrarRegistro(novaMesa);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public ActionResult Detalhes(Guid id)
    {
        Mesa mesaSelecionada = repositorioMesa.SelecionarRegistroPorId(id);

        DetalhesMesaViewModel detalhesVM = new(
            id,
            mesaSelecionada.Numero,
            mesaSelecionada.Capacidade);

        return View(detalhesVM);
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id)
    {
        Mesa mesaSelecionada = repositorioMesa.SelecionarRegistroPorId(id);

        EditarMesaViewModel editarVM = new(
            mesaSelecionada.Id,
            mesaSelecionada.Numero,
            mesaSelecionada.Capacidade);

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarMesaViewModel editarVM)
    {
        foreach (Mesa mesa in repositorioMesa.SelecionarRegistros())
        {
            if (mesa.Id != id && mesa.Numero == editarVM.Numero)

            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma mesa com esse número.");
                break;
            }
        }

        if (!ModelState.IsValid)
        {
            return View(editarVM);
        }

        Mesa mesaEditada = editarVM.ParaEntidade();

        repositorioMesa.EditarRegistro(id, mesaEditada);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        Mesa mesaSelecionada = repositorioMesa.SelecionarRegistroPorId(id);

        ExcluirMesaViewModel excluirVM = new(
            id,
            mesaSelecionada.Numero);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        repositorioMesa.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }
}
