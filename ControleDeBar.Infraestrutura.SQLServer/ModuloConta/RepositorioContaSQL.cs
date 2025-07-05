using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrutura.SQLServer.Extensions;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SQLServer.ModuloConta;

public class RepositorioContaSQL : IRepositorioConta
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ControleDeBarDb;Integrated Security=True";

    public void CadastrarRegistro(Conta novoRegistro)
    {
        const string sqlCadastrar =
            @"INSERT INTO [TBCONTA]
            (
	            [ID],
	            [TITULAR],
	            [MESA_ID],
	            [GARCOM_ID],
	            [ABERTURA],
	            [FECHAMENTO],
	            [ESTAABERTA],
	            [VALORTOTAL]
            )
            VALUES
            (
	            @ID,
	            @TITULAR,
	            @MESA_ID,
	            @GARCOM_ID,
	            @ABERTURA,
	            @FECHAMENTO,
	            @ESTAABERTA,
	            @VALORTOTAL
            )";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoCadastro = new(sqlCadastrar, conexaoComBanco);

        ConfigurarParametrosConta(novoRegistro, comandoCadastro);

        comandoCadastro.ExecuteNonQuery();

        conexaoComBanco.Close();

        AdicionarPedidos(novoRegistro);
    }

    public bool EditarRegistro(Guid idRegistro, Conta registroEditado)
    {
        const string sqlEditar =
            @"UPDATE [TBCONTA]
            SET
	            [TITULAR] = @TITULAR,
	            [MESA_ID] = @MESA_ID,
	            [GARCOM_ID] = @GARCOM_ID,
	            [ABERTURA] = @ABERTURA,
	            [FECHAMENTO] = @FECHAMENTO,
	            [ESTAABERTA] = @ESTAABERTA,
	            [VALORTOTAL] = @VALORTOTAL
            WHERE
	            [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoEdicao = new(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosConta(registroEditado, comandoEdicao);

        int linhasAfetadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        RemoverPedidos(idRegistro);

        CarregarPedidos(registroEditado);

        return linhasAfetadas >= 1;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        const string sqlExcluir =
            @"DELETE FROM [TBCONTA]
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

    public List<Conta> SelecionarContasAbertas()
    {
        const string sqlSelecionarTodos =
            @"SELECT
	            C.[ID],
	            C.[TITULAR],
	            C.[MESA_ID],
	            C.[GARCOM_ID],
	            C.[ABERTURA],
	            C.[FECHAMENTO],
	            C.[ESTAABERTA],
	            C.[VALORTOTAL],
                G.[NOME],
                G.[CPF],
                M.[NUMERO],
                M.[CAPACIDADE],
                M.[ESTAOCUPADA]
            FROM
                [TBCONTA] AS C
            INNER JOIN
                [TBGARCOM] AS G ON C.[GARCOM_ID] = G.[ID]
            INNER JOIN 
                [TBMESA] AS M ON C.[MESA_ID] = M.[ID]
            WHERE
                [ESTAABERTA] = @ESTAABERTA";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlSelecionarTodos, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ESTAABERTA", true);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Conta> contas = [];

        while (leitor.Read())
        {
            contas.Add(ConverterParaConta(leitor));
        }

        conexaoComBanco.Close();

        foreach (Conta conta in contas)
            CarregarPedidos(conta);

        return contas;
    }

    public List<Conta> SelecionarContasFechadas()
    {
        const string sqlSelecionarTodos =
            @"SELECT
	            C.[ID],
	            C.[TITULAR],
	            C.[MESA_ID],
	            C.[GARCOM_ID],
	            C.[ABERTURA],
	            C.[FECHAMENTO],
	            C.[ESTAABERTA],
	            C.[VALORTOTAL],
                G.[NOME],
                G.[CPF],
                M.[NUMERO],
                M.[CAPACIDADE],
                M.[ESTAOCUPADA]
            FROM
                [TBCONTA] AS C
            INNER JOIN
                [TBGARCOM] AS G ON C.[GARCOM_ID] = G.[ID]
            INNER JOIN 
                [TBMESA] AS M ON C.[MESA_ID] = M.[ID]
            WHERE
                [ESTAABERTA] = @ESTAABERTA";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlSelecionarTodos, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ESTAABERTA", false);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Conta> contas = [];

        while (leitor.Read())
        {
            contas.Add(ConverterParaConta(leitor));
        }

        conexaoComBanco.Close();

        foreach (Conta conta in contas)
            CarregarPedidos(conta);

        return contas;
    }

    public List<Conta> SelecionarContasPeriodo(DateTime data)
    {
        const string sqlSelecionarTodos =
            @"SELECT
	            C.[ID],
	            C.[TITULAR],
	            C.[MESA_ID],
	            C.[GARCOM_ID],
	            C.[ABERTURA],
	            C.[FECHAMENTO],
	            C.[ESTAABERTA],
	            C.[VALORTOTAL],
                G.[NOME],
                G.[CPF],
                M.[NUMERO],
                M.[CAPACIDADE],
                M.[ESTAOCUPADA]
            FROM
                [TBCONTA] AS C
            INNER JOIN
                [TBGARCOM] AS G ON C.[GARCOM_ID] = G.[ID]
            INNER JOIN 
                [TBMESA] AS M ON C.[MESA_ID] = M.[ID]
            WHERE
                CAST([FECHAMENTO] AS DATE) = @FECHAMENTO";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlSelecionarTodos, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("FECHAMENTO", data.Date);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Conta> contas = [];

        while (leitor.Read())
        {
            contas.Add(ConverterParaConta(leitor));
        }

        conexaoComBanco.Close();

        foreach (Conta conta in contas)
            CarregarPedidos(conta);

        return contas;
    }

    public Conta SelecionarRegistroPorId(Guid idRegistro)
    {
        const string sqlSelecionarTodos =
            @"SELECT
	            C.[ID],
	            C.[TITULAR],
	            C.[MESA_ID],
	            C.[GARCOM_ID],
	            C.[ABERTURA],
	            C.[FECHAMENTO],
	            C.[ESTAABERTA],
	            C.[VALORTOTAL],
                G.[NOME],
                G.[CPF],
                M.[NUMERO],
                M.[CAPACIDADE],
                M.[ESTAOCUPADA]
            FROM
                [TBCONTA] AS C
            INNER JOIN
                [TBGARCOM] AS G ON C.[GARCOM_ID] = G.[ID]
            INNER JOIN 
                [TBMESA] AS M ON C.[MESA_ID] = M.[ID]
            WHERE
	            C.ID = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlSelecionarTodos, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Conta? conta = null;

        if (leitor.Read())
            conta = ConverterParaConta(leitor);

        if (conta is not null)
            CarregarPedidos(conta);

        conexaoComBanco.Close();

        return conta!;
    }

    public List<Conta> SelecionarRegistros()
    {
        const string sqlSelecionarTodos =
            @"SELECT
	            C.[ID],
	            C.[TITULAR],
	            C.[MESA_ID],
	            C.[GARCOM_ID],
	            C.[ABERTURA],
	            C.[FECHAMENTO],
	            C.[ESTAABERTA],
	            C.[VALORTOTAL],
                G.[NOME],
                G.[CPF],
                M.[NUMERO],
                M.[CAPACIDADE],
                M.[ESTAOCUPADA]
            FROM
                [TBCONTA] AS C
            INNER JOIN
                [TBGARCOM] AS G ON C.[GARCOM_ID] = G.[ID]
            INNER JOIN 
                [TBMESA] AS M ON C.[MESA_ID] = M.[ID]";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlSelecionarTodos, conexaoComBanco);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Conta> contas = [];

        while (leitor.Read())
        {
            contas.Add(ConverterParaConta(leitor));
        }

        conexaoComBanco.Close();

        foreach (Conta conta in contas)
            CarregarPedidos(conta);

        return contas;
    }

    public void AdicionarPedido(Conta conta, Pedido pedido)
    {
        const string sqlAdicionarPedido =
            @"INSERT INTO [TBPEDIDO]
            (
	            [ID],
	            [CONTA_ID],
	            [PRODUTO_ID],
	            [QUANTIDADESOLICITADA]
            )
            VALUES
            (
                @ID,
	            @CONTA_ID,
	            @PRODUTO_ID,
                @QUANTIDADESOLICITADA
            )";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoAdicao = new(sqlAdicionarPedido, conexaoComBanco);

        comandoAdicao.Parameters.AddWithValue("ID", pedido.Id);
        comandoAdicao.Parameters.AddWithValue("CONTA_ID", conta.Id);
        comandoAdicao.Parameters.AddWithValue("PRODUTO_ID", pedido.Produto.Id);
        comandoAdicao.Parameters.AddWithValue("QUANTIDADESOLICITADA", pedido.QuantidadeSolicitada);

        comandoAdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        conta.Pedidos.Add(pedido);
        AtualizarValorTotal(conta);
    }

    public void RemoverPedido(Conta conta, Pedido pedido)
    {
        const string sqlRemoverCategoria =
            @"DELETE FROM 
                [TBPEDIDO]
            WHERE
                [CONTA_ID] = @CONTA_ID
            AND
                [ID] = @PEDIDO_ID";

        SqlConnection conexaoComBanco = new(connectionString);

        SqlCommand comandoExclusao = new(sqlRemoverCategoria, conexaoComBanco);

        conexaoComBanco.Open();

        comandoExclusao.Parameters.AddWithValue("CONTA_ID", conta.Id);
        comandoExclusao.Parameters.AddWithValue("PEDIDO_ID", pedido.Id);

        comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        conta.Pedidos.RemoveAll(p => p.Id == pedido.Id);
        AtualizarValorTotal(conta);
    }

    public void FecharConta(Conta conta)
    {
        conta.Fechar();

        const string sqlFecharConta =
            @"UPDATE [TBCONTA]
            SET
                [FECHAMENTO] = @FECHAMENTO,
                [ESTAABERTA] = @ESTAABERTA
            WHERE
                [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoFechamento = new(sqlFecharConta, conexaoComBanco);

        comandoFechamento.Parameters.AddWithValue("ID", conta.Id);
        comandoFechamento.Parameters.AddWithValue("FECHAMENTO", conta.Fechamento);
        comandoFechamento.Parameters.AddWithValue("ESTAABERTA", conta.EstaAberta);

        comandoFechamento.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    private void AdicionarPedidos(Conta conta)
    {
        const string sqlAdicionarPedido =
            @"INSERT INTO [TBPEDIDO]
            (
	            [ID],
	            [CONTA_ID],
	            [PRODUTO_ID],
	            [QUANTIDADESOLICITADA]
            )
            VALUES
            (
                @ID,
	            @CONTA_ID,
	            @PRODUTO_ID,
                @QUANTIDADESOLICITADA
            )";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        foreach (Pedido p in conta.Pedidos)
        {
            SqlCommand comandoAdicao = new(sqlAdicionarPedido, conexaoComBanco);

            comandoAdicao.Parameters.AddWithValue("ID", p.Id);
            comandoAdicao.Parameters.AddWithValue("CONTA_ID", conta.Id);
            comandoAdicao.Parameters.AddWithValue("PRODUTO_ID", p.Produto.Id);
            comandoAdicao.Parameters.AddWithValue("QUANTIDADESOLICITADA", p.QuantidadeSolicitada);

            comandoAdicao.ExecuteNonQuery();
        }

        conexaoComBanco.Close();
    }

    private void CarregarPedidos(Conta conta)
    {
        const string sqlCarregarPedidos =
            @"SELECT
	            PED.[ID],
	            PED.[CONTA_ID],
	            PED.[PRODUTO_ID],
	            PED.[QUANTIDADESOLICITADA],
	            PROD.[NOME],
	            PROD.[PRECO]
            FROM
	            [TBPEDIDO] AS PED
	            INNER JOIN [TBPRODUTO] AS PROD
            ON
	            PED.[PRODUTO_ID] = PROD.[ID]
            WHERE
	            PED.CONTA_ID = @CONTA_ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new(sqlCarregarPedidos, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("CONTA_ID", conta.Id);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        while (leitor.Read())
        {
            Pedido pedido = ConverterParaPedido(leitor);

            conta.Pedidos.Add(pedido);
        }

        conexaoComBanco.Close();

        AtualizarValorTotal(conta);
    }

    private void RemoverPedidos(Guid idConta)
    {
        const string sqlRemoverCategoria =
            @"DELETE FROM 
                [TBPEDIDO]
            WHERE
                [CONTA_ID] = @CONTA_ID";

        SqlConnection conexaoComBanco = new(connectionString);

        SqlCommand comandoExclusao = new(sqlRemoverCategoria, conexaoComBanco);

        conexaoComBanco.Open();

        comandoExclusao.Parameters.AddWithValue("CONTA_ID", idConta);

        comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    private void AtualizarValorTotal(Conta conta)
    {
        const string sqlAtualizarValorTotal =
            @"UPDATE [TBCONTA]
            SET 
                [VALORTOTAL] = @VALORTOTAL
            WHERE 
                [ID] = @ID";

        SqlConnection conexaoComBanco = new(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoAtualizaValor = new(sqlAtualizarValorTotal, conexaoComBanco);

        comandoAtualizaValor.Parameters.AddWithValue("VALORTOTAL", conta.CalcularValorTotal());
        comandoAtualizaValor.Parameters.AddWithValue("ID", conta.Id);

        comandoAtualizaValor.ExecuteNonQuery();
    }

    private Conta ConverterParaConta(SqlDataReader leitor)
    {
        Garcom? garcom = null;

        if (!leitor["GARCOM_ID"].Equals(DBNull.Value))
            garcom = ConverterParaGarcom(leitor);

        Mesa? mesa = null;

        if (!leitor["MESA_ID"].Equals(DBNull.Value))
            mesa = ConverterParaMesa(leitor);

        DateTime? fechamento = null;

        if (!leitor["FECHAMENTO"].Equals(DBNull.Value))
            fechamento = Convert.ToDateTime(leitor["FECHAMENTO"]);

        return new()
        {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Titular = Convert.ToString(leitor["TITULAR"])!,
            Mesa = mesa!,
            Garcom = garcom!,
            Abertura = Convert.ToDateTime(leitor["ABERTURA"]),
            Fechamento = fechamento,
            EstaAberta = Convert.ToBoolean(leitor["ESTAABERTA"]),
            ValorTotal = Convert.ToDecimal(leitor["VALORTOTAL"])
        };
    }

    private Garcom ConverterParaGarcom(SqlDataReader leitor)
    {
        return new(
            Guid.Parse(leitor["GARCOM_ID"].ToString()!),
            Convert.ToString(leitor["NOME"])!,
            Convert.ToString(leitor["CPF"])!
            );
    }

    private Pedido ConverterParaPedido(SqlDataReader leitor)
    {
        Produto produto = new(
            Guid.Parse(leitor["PRODUTO_ID"].ToString()!),
            leitor["NOME"].ToString()!,
            Convert.ToDecimal(leitor["PRECO"])
        );

        return new Pedido()
        {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Produto = produto,
            QuantidadeSolicitada = Convert.ToInt32(leitor["QUANTIDADESOLICITADA"])
        };
    }

    private Mesa ConverterParaMesa(SqlDataReader leitor)
    {
        return new(
            Guid.Parse(leitor["MESA_ID"].ToString()!),
            Convert.ToInt32(leitor["NUMERO"]),
            Convert.ToInt32(leitor["CAPACIDADE"]),
            Convert.ToBoolean(leitor["ESTAOCUPADA"])
            );
    }

    private void ConfigurarParametrosConta(Conta conta, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", conta.Id);
        comando.Parameters.AddWithValue("TITULAR", conta.Titular);
        comando.Parameters.AddWithValue("MESA_ID", conta.Mesa.Id);
        comando.Parameters.AddWithValue("GARCOM_ID", conta.Garcom.Id);
        comando.Parameters.AddWithValue("ABERTURA", conta.Abertura);
        comando.Parameters.AdicionarValorNullavel("FECHAMENTO", conta.Fechamento);
        comando.Parameters.AddWithValue("ESTAABERTA", conta.EstaAberta);
        comando.Parameters.AddWithValue("VALORTOTAL", conta.CalcularValorTotal());
    }
}