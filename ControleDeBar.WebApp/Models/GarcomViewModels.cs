using System.ComponentModel.DataAnnotations;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.WebApp.Extensions;

namespace ControleDeBar.WebApp.Models;

public abstract class FormularioGarcomViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do item deve ter entre 3 e 100 caracteres.")]
    [RegularExpression("^[a-zA-ZÄäÖöÜüÀàÈèÌìÒòÙùÁáÉéÍíÓóÚúÝýÂâÊêÎîÔôÛûÃãÑñÇç'\\-\\s]+$", ErrorMessage = "Nome inválido.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "C.P.F. é obrigatório")]
    [RegularExpression("^[0-9]{3}[\\.]?[0-9]{3}[\\.]?[0-9]{3}[-]?[0-9]{2}$", ErrorMessage = "C.P.F. inválido.")]
    public string CPF { get; set; } = string.Empty;
}
public class CadastrarGarcomViewModel : FormularioGarcomViewModel
{
    public CadastrarGarcomViewModel() { }
    public CadastrarGarcomViewModel(string nome, string cPF)
    {
        Nome = nome;
        CPF = cPF;
    }
}
public class VisualizarGarconsViewModel
{
    public List<DetalhesGarcomViewModel> Registros { get; } = [];
    public VisualizarGarconsViewModel(List<Garcom> garcons)
    {
        foreach (Garcom g in garcons)
        {
            DetalhesGarcomViewModel detalhesVM = g.ParaDetalhesVM();

            Registros.Add(detalhesVM);
        }
    }
}
public class EditarGarcomViewModel : FormularioGarcomViewModel
{
    public EditarGarcomViewModel() { }
    public EditarGarcomViewModel(Guid id, string nome, string cPF)
    {
        Id = id;
        Nome = nome;
        CPF = cPF;
    }
}
public class ExcluirGarcomViewModel : FormularioGarcomViewModel
{
    public ExcluirGarcomViewModel() { }
    public ExcluirGarcomViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}
public class DetalhesGarcomViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }

    public DetalhesGarcomViewModel(Guid id, string nome, string cPF)
    {
        Id = id;
        Nome = nome;
        CPF = cPF;
    }
}
