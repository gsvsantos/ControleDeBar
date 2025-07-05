using ControleDeBar.Dominio.ModuloGarcom;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SQLServer.ModuloGarcom;

public class RepositorioGarcomSQL : IRepositorioGarcom
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ControleDeBarDb;Integrated Security=True";

    public void CadastrarRegistro(Garcom novoRegistro)
    {
        novoRegistro.Id = Guid.NewGuid();

        const string sqlCadastrar =
            @"INSERT INTO [TBGarcom]
            (
	            [ID],
	            [NOME],
	            [CPF]
            )
            VALUES
            (
	            @ID,
	            @NOME,
	            @CPF
            );";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoCadastro = new(sqlCadastrar, conexaoComBanco);

        ConfigurarParametrosGarcom(novoRegistro, comandoCadastro);

        comandoCadastro.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public bool EditarRegistro(Guid idRegistro, Garcom registroEditado)
    {
        const string sqlEditar =
            @"UPDATE [TBGarcom]
            SET
	            [NOME] = @NOME,
	            [CPF] = @CPF
            WHERE
	            [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoEditar = new(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosGarcom(registroEditado, comandoEditar);

        int linhasAfetadas = comandoEditar.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas >= 1;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        const string sqlEditar =
            @"DELETE FROM [TBGarcom]
            WHERE
	            [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoExclusao = new(sqlEditar, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", idRegistro);

        int linhasAfetadas = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas >= 1;
    }

    public Garcom? SelecionarRegistroPorId(Guid idRegistro)
    {
        const string sqlSelecionarPorId =
            @"SELECT 
                [ID],
                [NOME],
                [CPF]
            FROM
                [TBGARCOM]
            WHERE
	            [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecaoPorId = new(sqlSelecionarPorId, conexaoComBanco);

        comandoSelecaoPorId.Parameters.AddWithValue("ID", idRegistro);

        SqlDataReader leitor = comandoSelecaoPorId.ExecuteReader();

        Garcom? garcom = null;

        if (leitor.Read())
            garcom = ConverterParaGarcom(leitor);

        conexaoComBanco.Close();

        return garcom;
    }

    public List<Garcom> SelecionarRegistros()
    {
        const string sqlSelecionarTodos =
            @"
            SELECT 
                [ID],
                [NOME],
                [CPF]
            FROM
                [TBGARCOM]";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlSelecionarTodos, conexaoComBanco);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Garcom> garcons = [];

        while (leitor.Read())
        {
            garcons.Add(ConverterParaGarcom(leitor));
        }

        conexaoComBanco.Close();

        return garcons;
    }

    private Garcom ConverterParaGarcom(SqlDataReader leitor)
    {
        return new(
            Guid.Parse(leitor["ID"].ToString()!),
            Convert.ToString(leitor["NOME"])!,
            Convert.ToString(leitor["CPF"])!
            );
    }

    public void ConfigurarParametrosGarcom(Garcom garcom, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", garcom.Id);
        comando.Parameters.AddWithValue("NOME", garcom.Nome);
        comando.Parameters.AddWithValue("CPF", garcom.CPF);
    }
}
