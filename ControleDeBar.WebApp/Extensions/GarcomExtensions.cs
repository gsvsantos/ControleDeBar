using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.WebApp.Models;

namespace ControleDeBar.WebApp.Extensions;

public static class GarcomExtensions
{
    public static Garcom ParaEntidade(this FormularioGarcomViewModel formularioVM)
    {
        return new(
            formularioVM.Nome,
            formularioVM.CPF);
    }
    public static DetalhesGarcomViewModel ParaDetalhesVM(this Garcom garcom)
    {
        return new(
            garcom.Id,
            garcom.Nome,
            garcom.CPF);
    }
}
