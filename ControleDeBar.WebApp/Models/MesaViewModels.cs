using System.ComponentModel.DataAnnotations;
using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.WebApp.Extensions;

namespace ControleDeBar.WebApp.Models;

public abstract class FormularioMesaViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Número é obrigatório")]
    [Range(1, 50, ErrorMessage = "O número deve ser um número entre 1 e 50")]
    public int Numero { get; set; } = 1;

    [Required(ErrorMessage = "Capacidade é obrigatória")]
    [Range(0, 4, ErrorMessage = "O número deve ser um número entre 0 e 4")]
    public int Capacidade { get; set; }
}

public class CadastrarMesaViewModel : FormularioMesaViewModel
{
    public CadastrarMesaViewModel() { }
    public CadastrarMesaViewModel(int numero, int capacidade) : this()
    {
        Numero = numero;
        Capacidade = capacidade;
    }
}

public class VisualizarMesasViewModel
{
    public List<DetalhesMesaViewModel> Registros { get; } = [];

    public VisualizarMesasViewModel(List<Mesa> mesas)
    {
        foreach (Mesa m in mesas)
        {
            DetalhesMesaViewModel detalhesVM = m.ParaDetalhesVM();

            Registros.Add(detalhesVM);
        }
    }
}

public class EditarMesaViewModel : FormularioMesaViewModel
{
    public EditarMesaViewModel() { }
    public EditarMesaViewModel(Guid id, int numero, int capacidade)
    {
        Id = id;
        Numero = numero;
        Capacidade = capacidade;
    }
}

public class ExcluirMesaViewModel : FormularioMesaViewModel
{
    public ExcluirMesaViewModel() { }
    public ExcluirMesaViewModel(Guid id, int numero)
    {
        Id = id;
        Numero = numero;
    }
}

public class DetalhesMesaViewModel
{
    public Guid Id { get; set; }
    public int Numero { get; set; }
    public int Capacidade { get; set; }
    public List<DetalhesContaViewModel> Registros { get; set; } = [];
    public decimal Total { get; set; }

    public DetalhesMesaViewModel(Guid id, int numero, int capacidade)
    {
        Id = id;
        Numero = numero;
        Capacidade = capacidade;
    }
    public DetalhesMesaViewModel(Guid id, int numero, int capacidade, List<Conta> contas) : this(id, numero, capacidade)
    {
        foreach (Conta c in contas)
        {
            Total += c.CalcularValorTotal();

            Registros.Add(c.ParaDetalhesVM());
        }
    }
}