using ControleDeBar.Dominio.Compartilhado;
using ControleDeBar.Dominio.ModuloConta;

namespace ControleDeBar.Dominio.ModuloProduto;

public class Produto : EntidadeBase<Produto>
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public List<Pedido> Pedidos { get; set; } = [];

    public Produto(string nome, decimal preco)
    {
        Nome = nome;
        Preco = preco;
    }
    public Produto(Guid id, string nome, decimal preco) : this(nome, preco)
    {
        Id = id;
        Nome = nome;
        Preco = preco;
    }

    public override void AtualizarRegistro(Produto registroEditado)
    {
        Nome = registroEditado.Nome;
        Preco = registroEditado.Preco;
    }
}
