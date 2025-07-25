﻿using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("produtos")]
public class ProdutoController : Controller
{
    private readonly IRepositorioConta repositorioConta;
    private readonly IRepositorioProduto repositorioProduto;

    public ProdutoController(IRepositorioConta repositorioConta, IRepositorioProduto repositorioProduto)
    {
        this.repositorioConta = repositorioConta;
        this.repositorioProduto = repositorioProduto;
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Produto> produtos = repositorioProduto.SelecionarRegistros();

        VisualizarProdutosViewModel visualizarVM = new(produtos);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        CadastrarProdutoViewModel cadastrarVM = new();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarProdutoViewModel cadastrarVM)
    {
        foreach (Produto produto in repositorioProduto.SelecionarRegistros())
        {
            if (produto.Nome == cadastrarVM.Nome)

            {
                ModelState.AddModelError("CadastroUnico", "Produto já registrado.");
                break;
            }
        }

        if (!ModelState.IsValid)
        {
            return View(cadastrarVM);
        }

        Produto novoProduto = cadastrarVM.ParaEntidade();

        repositorioProduto.CadastrarRegistro(novoProduto);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id)
    {
        Produto produtoSelecionado = repositorioProduto.SelecionarRegistroPorId(id)!;

        EditarProdutoViewModel editarVM = new(
            id,
            produtoSelecionado.Nome,
            produtoSelecionado.Preco);

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarProdutoViewModel editarVM)
    {
        foreach (Produto produto in repositorioProduto.SelecionarRegistros())
        {
            if (produto.Id != id && produto.Nome == editarVM.Nome)

            {
                ModelState.AddModelError("CadastroUnico", "Produto já registrado.");
                break;
            }
        }

        if (!ModelState.IsValid)
        {
            return View(editarVM);
        }

        Produto novoProduto = editarVM.ParaEntidade();

        repositorioProduto.EditarRegistro(id, novoProduto);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        Produto produtoSelecionado = repositorioProduto.SelecionarRegistroPorId(id)!;

        ExcluirProdutoViewModel excluirVM = new(
            id,
            produtoSelecionado.Nome);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        List<Conta> contas = repositorioConta.SelecionarRegistros();

        if (repositorioProduto.ProdutoContemVinculos(id, contas))
        {
            ModelState.AddModelError("ConflitosVinculos", "Este produto contém registros e não pode ser excluído.");
        }

        if (!ModelState.IsValid)
        {
            Produto produtoSelecionado = repositorioProduto.SelecionarRegistroPorId(id)!;

            return View(nameof(Excluir), new ExcluirProdutoViewModel(id, produtoSelecionado.Nome));
        }
        repositorioProduto.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }
}
