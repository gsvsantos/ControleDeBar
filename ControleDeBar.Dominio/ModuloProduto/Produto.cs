using System.Diagnostics.CodeAnalysis;
using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.ModuloProduto;

public class Produto : EntidadeBase<Produto>
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }

    [ExcludeFromCodeCoverage]
    public Produto() { }
    public Produto(string nome, decimal preco)
    {
        Nome = nome;
        Preco = preco;
    }

    public override void AtualizarRegistro(Produto registroEditado)
    {
        Nome = registroEditado.Nome;
        Preco = registroEditado.Preco;
    }
}
