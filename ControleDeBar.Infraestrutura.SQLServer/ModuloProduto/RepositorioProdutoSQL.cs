using ControleDeBar.Dominio.ModuloProduto;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SQLServer.ModuloProduto;

public class RepositorioProdutoSQL : IRepositorioProduto
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ControleDeBarDb;Integrated Security=True";

    public void CadastrarRegistro(Produto novoRegistro)
    {
        novoRegistro.Id = Guid.NewGuid();

        const string sqlCadastrar =
            @"INSERT INTO [TBPRODUTO]
            (
	            [ID],
	            [NOME],
	            [PRECO]
            )
            VALUES
            (
	            @ID,
	            @NOME,
	            @PRECO
            )";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoCadastrar = new(sqlCadastrar, conexaoComBanco);

        ConfigurarParametrosProduto(novoRegistro, comandoCadastrar);

        comandoCadastrar.ExecuteReader();

        conexaoComBanco.Close();
    }

    public bool EditarRegistro(Guid idRegistro, Produto registroEditado)
    {
        const string sqlEditar =
            @"UPDATE [TBPRODUTO]
            SET
	            [NOME] = @NOME,
	            [PRECO] = @PRECO
            WHERE
	            [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoEdicao = new(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosProduto(registroEditado, comandoEdicao);

        int linhaAfetadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhaAfetadas >= 1;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        const string sqlExcluir =
            @"DELETE FROM [TBPRODUTO]
            WHERE
                [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoExclusao = new(sqlExcluir, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", idRegistro);

        int linhaAfetadas = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhaAfetadas >= 1;
    }

    public Produto? SelecionarRegistroPorId(Guid idRegistro)
    {
        const string sqlSelecionarTodos =
            @"SELECT 
	            [ID],
	            [NOME],
	            [PRECO]
            FROM
	            [TBPRODUTO]
            WHERE
                [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlSelecionarTodos, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Produto? produto = null;

        if (leitor.Read())
            produto = ConverterParaProduto(leitor);

        conexaoComBanco.Close();

        return produto;
    }

    public List<Produto> SelecionarRegistros()
    {
        const string sqlSelecionarTodos =
            @"SELECT 
	            [ID],
	            [NOME],
	            [PRECO]
            FROM
	            [TBPRODUTO]";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlSelecionarTodos, conexaoComBanco);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Produto> produtos = [];

        while (leitor.Read())
        {
            produtos.Add(ConverterParaProduto(leitor));
        }

        conexaoComBanco.Close();

        return produtos;
    }

    private Produto ConverterParaProduto(SqlDataReader leitor)
    {
        return new(
            Guid.Parse(leitor["ID"].ToString()!),
            Convert.ToString(leitor["NOME"])!,
            Convert.ToDecimal(leitor["PRECO"])
            );
    }

    private void ConfigurarParametrosProduto(Produto produto, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", produto.Id);
        comando.Parameters.AddWithValue("NOME", produto.Nome);
        comando.Parameters.AddWithValue("PRECO", produto.Preco);
    }
}
