using System.Diagnostics.CodeAnalysis;
using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.ModuloGarcom;

public class Garcom : EntidadeBase<Garcom>
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;

    [ExcludeFromCodeCoverage]
    public Garcom() { }
    public Garcom(string nome, string cPF)
    {
        Nome = nome;
        CPF = cPF;
    }

    public override void AtualizarRegistro(Garcom registroEditado)
    {
        Nome = registroEditado.Nome;
        CPF = registroEditado.CPF;
    }
}
