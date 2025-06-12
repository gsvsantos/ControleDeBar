using System.ComponentModel.DataAnnotations;
using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.WebApp.Extensions;

namespace ControleDeBar.WebApp.Models;

public class FormularioProdutoViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome do item deve ter entre 2 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Preço é obrigatório")]
    [DataType(DataType.Currency)]
    [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser um valor positivo")]
    public decimal Preco { get; set; }
}

public class CadastrarProdutoViewModel : FormularioProdutoViewModel
{
    public CadastrarProdutoViewModel() { }
    public CadastrarProdutoViewModel(string nome, decimal preco) : this()
    {
        Nome = nome;
        Preco = preco;
    }
}

public class VisualizarProdutosViewModel
{
    public List<DetalhesProdutoViewModel> Registros { get; } = [];
    public VisualizarProdutosViewModel(List<Produto> produtos)
    {
        foreach (Produto p in produtos)
        {
            DetalhesProdutoViewModel detalhesVM = p.ParaDetalhesVM();

            Registros.Add(detalhesVM);
        }
    }
}

public class EditarProdutoViewModel : FormularioProdutoViewModel
{
    public EditarProdutoViewModel() { }
    public EditarProdutoViewModel(Guid id, string nome, decimal preco) : this()
    {
        Id = id;
        Nome = nome;
        Preco = preco;
    }
}

public class ExcluirProdutoViewModel : FormularioProdutoViewModel
{
    public ExcluirProdutoViewModel() { }
    public ExcluirProdutoViewModel(Guid id, string nome) : this()
    {
        Id = id;
        Nome = nome;
    }
}

public class DetalhesProdutoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }

    public DetalhesProdutoViewModel(Guid id, string nome, decimal preco)
    {
        Id = id;
        Nome = nome;
        Preco = preco;
    }
}
