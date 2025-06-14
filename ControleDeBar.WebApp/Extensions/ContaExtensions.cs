using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.WebApp.Models;

namespace ControleDeBar.WebApp.Extensions;

public static class ContaExtensions
{
    public static Conta ParaEntidade(this AbrirContaViewModel abrirVM, List<Mesa> mesas, List<Garcom> garcons)
    {
        Mesa? mesaSelecionada = null;

        foreach (Mesa m in mesas)
        {
            if (m.Id == abrirVM.MesaId)
                mesaSelecionada = m;
        }

        Garcom? garcomSelecionado = null;

        foreach (Garcom g in garcons)
        {
            if (g.Id == abrirVM.GarcomId)
                garcomSelecionado = g;
        }

        return new(
            abrirVM.Titular,
            mesaSelecionada!,
            garcomSelecionado!);
    }

    public static DetalhesContaViewModel ParaDetalhesVM(this Conta conta)
    {
        return new(
                conta.Id,
                conta.Titular,
                conta.Mesa.Numero,
                conta.Garcom.Nome,
                conta.Abertura,
                conta.Fechamento,
                conta.EstaAberta,
                conta.CalcularValorTotal(),
                conta.Pedidos);
    }
}
