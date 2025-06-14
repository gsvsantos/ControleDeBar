using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Infraestrutura.Arquivos.Compartilhado;

namespace ControleDeBar.Infraestrutura.Arquivos.ModuloConta;

public class RepositorioContaEmArquivo : RepositorioBaseEmArquivo<Conta>, IRepositorioConta
{
    public RepositorioContaEmArquivo(ContextoDados contexto) : base(contexto) { }

    public void CadastrarConta(Conta novaConta)
    {
        registros.Add(novaConta);

        contexto.Salvar();
    }

    public Conta SelecionarPorId(Guid idRegistro)
    {
        foreach (Conta c in registros)
        {
            if (c.Id == idRegistro)
                return c;
        }

        return null!;
    }

    public List<Conta> SelecionarContasAbertas()
    {
        List<Conta> contasAbertas = [];

        foreach (Conta c in registros)
        {
            if (c.EstaAberta)
                contasAbertas.Add(c);
        }

        return contasAbertas;
    }

    public List<Conta> SelecionarContasFechadas()
    {
        List<Conta> contasFechadas = [];

        foreach (Conta c in registros)
        {
            if (!c.EstaAberta)
                contasFechadas.Add(c);
        }

        return contasFechadas;
    }

    protected override List<Conta> ObterRegistros()
    {
        return contexto.Contas;
    }

    public List<Conta> SelecionarContasPeriodo(DateTime data)
    {
        List<Conta> contasPeriodo = [];

        foreach (Conta c in registros)
        {
            if (c.Fechamento.Date == data.Date)
                contasPeriodo.Add(c);
        }

        return contasPeriodo;
    }
}
