using System.Text.Json;
using System.Text.Json.Serialization;
using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Dominio.ModuloProduto;

namespace ControleDeBar.Infraestrutura.Arquivos.Compartilhado;

public class ContextoDados
{
    public List<Conta> Contas { get; set; }
    public List<Garcom> Garcons { get; set; }
    public List<Mesa> Mesas { get; set; }
    public List<Produto> Produtos { get; set; }
    private string pastaArmazenamento = string.Empty;
    private readonly string arquivoArmazenamento = "dados-controle-de-bar.json";

    public ContextoDados()
    {
        Contas = [];
        Garcons = [];
        Mesas = [];
        Produtos = [];
    }

    public ContextoDados(bool carregarDados) : this()
    {
        if (carregarDados)
            Carregar();
    }

    public void VerificarSistemaOperacional()
    {
        pastaArmazenamento = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Controle-de-Bar");
    }

    public void Salvar()
    {
        VerificarSistemaOperacional();
        string caminhoCompleto = Path.Combine(pastaArmazenamento, arquivoArmazenamento);

        if (!Directory.Exists(pastaArmazenamento))
            Directory.CreateDirectory(pastaArmazenamento);

        JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        string json = JsonSerializer.Serialize(this, jsonOptions);

        File.WriteAllText(caminhoCompleto, json);
    }

    public void Carregar()
    {
        VerificarSistemaOperacional();
        string caminhoCompleto = Path.Combine(pastaArmazenamento, arquivoArmazenamento);

        if (!File.Exists(caminhoCompleto)) return;

        string json = File.ReadAllText(caminhoCompleto);

        if (string.IsNullOrWhiteSpace(json)) return;

        JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

        ContextoDados? contextoArmazenado = JsonSerializer.Deserialize<ContextoDados>(json, jsonOptions);

        if (contextoArmazenado == null) return;

        Mesas = contextoArmazenado.Mesas;
        Garcons = contextoArmazenado.Garcons;
        Produtos = contextoArmazenado.Produtos;
        Contas = contextoArmazenado.Contas;
    }
}