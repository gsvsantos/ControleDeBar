using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Infraestrutura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloGarcom;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("garcons")]
public class GarcomController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IRepositorioGarcom repositorioGarcom;

    public GarcomController()
    {
        contextoDados = new(true);
        repositorioGarcom = new RepositorioGarcomEmArquivo(contextoDados);
    }

    [HttpGet]
    public IActionResult Index()
    {
        VisualizarGarconsViewModel visualizarVM = new(repositorioGarcom.SelecionarRegistros());

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        CadastrarGarcomViewModel cadastrarVM = new();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarGarcomViewModel cadastrarVM)
    {
        foreach (Garcom garcom in repositorioGarcom.SelecionarRegistros())
        {
            if (garcom.CPF == cadastrarVM.CPF)
            {
                ModelState.AddModelError("CadastroUnico", "O C.P.F. inserido já contém um cadastro.");
                break;
            }
        }

        if (!ModelState.IsValid)
        {
            return View(cadastrarVM);
        }

        Garcom novoGarcom = cadastrarVM.ParaEntidade();

        repositorioGarcom.CadastrarRegistro(novoGarcom);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id)
    {
        Garcom garcomSelecionado = repositorioGarcom.SelecionarRegistroPorId(id);

        DetalhesGarcomViewModel detalhesVM = new(
            garcomSelecionado.Id,
            garcomSelecionado.Nome,
            garcomSelecionado.CPF);

        return View(detalhesVM);
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id)
    {
        Garcom garcomSelecionado = repositorioGarcom.SelecionarRegistroPorId(id);

        EditarGarcomViewModel editarVM = new(
            garcomSelecionado.Id,
            garcomSelecionado.Nome,
            garcomSelecionado.CPF);

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    public IActionResult Editar(Guid id, EditarGarcomViewModel editarVM)
    {
        foreach (Garcom garcom in repositorioGarcom.SelecionarRegistros())
        {
            if (garcom.Id != id && garcom.CPF == editarVM.CPF)
            {
                ModelState.AddModelError("CadastroUnico", "O C.P.F. inserido já contém um cadastro.");
                break;
            }
        }

        if (!ModelState.IsValid)
        {
            return View(editarVM);
        }

        Garcom garcomEditado = editarVM.ParaEntidade();

        repositorioGarcom.EditarRegistro(id, garcomEditado);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        Garcom garcomSelecionado = repositorioGarcom.SelecionarRegistroPorId(id);

        ExcluirGarcomViewModel excluirVM = new(
            id,
            garcomSelecionado.Nome);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        repositorioGarcom.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }
}
