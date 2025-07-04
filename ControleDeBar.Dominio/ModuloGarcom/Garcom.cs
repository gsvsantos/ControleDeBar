using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.ModuloGarcom;

public class Garcom : EntidadeBase<Garcom>
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;

    public Garcom(string nome, string cPF)
    {
        Nome = nome;
        CPF = cPF;
    }
    public Garcom(Guid id, string nome, string cPF) : this(nome, cPF)
    {
        Id = id;
        Nome = nome;
        CPF = cPF;
    }

    public override void AtualizarRegistro(Garcom registroEditado)
    {
        Nome = registroEditado.Nome;
        CPF = registroEditado.CPF;
    }
}
