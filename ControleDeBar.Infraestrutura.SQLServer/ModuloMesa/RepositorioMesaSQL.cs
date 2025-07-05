using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloMesa;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SQLServer.ModuloMesa;

public class RepositorioMesaSQL : IRepositorioMesa
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ControleDeBarDb;Integrated Security=True";

    public void CadastrarRegistro(Mesa novoRegistro)
    {
        novoRegistro.Id = Guid.NewGuid();

        const string sqlCadastrar =
            @"INSERT INTO [TBMESA]
            (
                [ID],
                [NUMERO],
                [CAPACIDADE],
                [ESTAOCUPADA]
            )
            VALUES
            (
                @ID,
                @NUMERO,
                @CAPACIDADE,
                @ESTAOCUPADA
            )";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoCadastro = new(sqlCadastrar, conexaoComBanco);

        ConfigurarParametrosMesa(novoRegistro, comandoCadastro);

        comandoCadastro.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public bool EditarRegistro(Guid idRegistro, Mesa registroEditado)
    {
        const string sqlEditar =
            @"UPDATE [TBMESA]
            SET
                [NUMERO] = @NUMERO,
                [CAPACIDADE] = @CAPACIDADE
            WHERE
                [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoEdicao = new(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosMesa(registroEditado, comandoEdicao);

        int linhasAfetadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas >= 1;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        const string sqlExcluir =
            @"DELETE FROM [TBMESA]
            WHERE
                [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoExclusao = new(sqlExcluir, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", idRegistro);

        int linhasAfetadas = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas >= 1;


    }

    public Mesa? SelecionarRegistroPorId(Guid idRegistro)
    {
        const string sqlSelecionarTodos =
            @"SELECT
                [ID],
                [NUMERO],
                [CAPACIDADE],
                [ESTAOCUPADA]
            FROM
                [TBMESA]
            WHERE
                [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlSelecionarTodos, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Mesa? mesa = null;

        if (leitor.Read())
            mesa = ConverterParaMesa(leitor);

        conexaoComBanco.Close();

        return mesa;
    }

    public List<Mesa> SelecionarRegistros()
    {
        const string sqlSelecionarTodos =
            @"SELECT
                [ID],
                [NUMERO],
                [CAPACIDADE],
                [ESTAOCUPADA]
            FROM
                [TBMESA]";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlSelecionarTodos, conexaoComBanco);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Mesa> mesas = [];

        while (leitor.Read())
        {
            mesas.Add(ConverterParaMesa(leitor));
        }

        conexaoComBanco.Close();

        return mesas;
    }

    public bool VerificarMesaCheia(Mesa mesa, List<Conta> contasAbertas)
    {
        return mesa.EstaOcupada && contasAbertas.Count(c => c.Mesa.Id == mesa.Id) == mesa.Capacidade;
    }

    public void OcuparMesa(Mesa mesa)
    {
        mesa.Ocupar();

        const string sqlFecharConta =
            @"UPDATE [TBMESA]
            SET
                [ESTAOCUPADA] = @ESTAOCUPADA
            WHERE
                [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoFechamento = new(sqlFecharConta, conexaoComBanco);

        comandoFechamento.Parameters.AddWithValue("ID", mesa.Id);
        comandoFechamento.Parameters.AddWithValue("ESTAOCUPADA", mesa.EstaOcupada);

        comandoFechamento.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public void DesocuparMesa(Mesa mesa)
    {
        mesa.Desocupar();

        const string sqlFecharConta =
            @"UPDATE [TBMESA]
            SET
                [ESTAOCUPADA] = @ESTAOCUPADA
            WHERE
                [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoFechamento = new(sqlFecharConta, conexaoComBanco);

        comandoFechamento.Parameters.AddWithValue("ID", mesa.Id);
        comandoFechamento.Parameters.AddWithValue("ESTAOCUPADA", mesa.EstaOcupada);

        comandoFechamento.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    private Mesa ConverterParaMesa(SqlDataReader leitor)
    {
        return new(
            Guid.Parse(leitor["ID"].ToString()!),
            Convert.ToInt32(leitor["NUMERO"]),
            Convert.ToInt32(leitor["CAPACIDADE"]),
            Convert.ToBoolean(leitor["ESTAOCUPADA"])
            );
    }

    private void ConfigurarParametrosMesa(Mesa mesa, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", mesa.Id);
        comando.Parameters.AddWithValue("NUMERO", mesa.Numero);
        comando.Parameters.AddWithValue("CAPACIDADE", mesa.Capacidade);
        comando.Parameters.AddWithValue("ESTAOCUPADA", mesa.EstaOcupada);
    }
}
