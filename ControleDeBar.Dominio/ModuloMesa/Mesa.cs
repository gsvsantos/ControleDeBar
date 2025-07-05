using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.ModuloMesa;

public class Mesa : EntidadeBase<Mesa>
{
    public int Numero { get; set; }
    public int Capacidade { get; set; }
    public bool EstaOcupada { get; set; }

    public Mesa(int numero, int capacidade)
    {
        Numero = numero;
        Capacidade = capacidade;
    }
    public Mesa(Guid id, int numero, int capacidade, bool estaOcupada) : this(numero, capacidade)
    {
        Id = id;
        Numero = numero;
        Capacidade = capacidade;
        EstaOcupada = estaOcupada;
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
