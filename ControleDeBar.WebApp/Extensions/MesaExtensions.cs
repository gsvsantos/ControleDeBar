using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.WebApp.Models;

namespace ControleDeBar.WebApp.Extensions;

public static class MesaExtensions
{
    public static Mesa ParaEntidade(this FormularioMesaViewModel formularioVM)
    {
        return new(
            formularioVM.Numero,
            formularioVM.Capacidade);
    }
    public static DataMesaViewModel ParaDetalhesVM(this Mesa mesa)
    {
        return new(
            mesa.Id,
            mesa.Numero,
            mesa.Capacidade);
    }
}
