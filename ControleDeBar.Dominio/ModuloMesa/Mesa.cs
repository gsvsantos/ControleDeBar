using System.Diagnostics.CodeAnalysis;
using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.ModuloMesa;

public class Mesa : EntidadeBase<Mesa>
{
    public int Numero { get; set; }
    public int Capacidade { get; set; }
    public bool EstaOcupada { get; set; }

    [ExcludeFromCodeCoverage]
    public Mesa() { }

    public Mesa(int numero, int capacidade) : this()
    {
        Numero = numero;
        Capacidade = capacidade;
    }

    public void Ocupar()
    {
        EstaOcupada = true;
    }

    public void Desocupar()
    {
        EstaOcupada = false;
    }

    public override void AtualizarRegistro(Mesa registroEditado)
    {
        Numero = registroEditado.Numero;
        Capacidade = registroEditado.Capacidade;
    }
}
