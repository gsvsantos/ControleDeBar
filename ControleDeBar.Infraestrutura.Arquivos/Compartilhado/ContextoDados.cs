using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;

namespace ControleDeBar.Infraestrutura.Arquivos.Compartilhado;

public class ContextoDados
{
    public List<Mesa> Mesas { get; set; }
    public List<Garcom> Garcons { get; set; }
    private string pastaArmazenamento = string.Empty;
    private string arquivoArmazenamento = "dados-controle-de-bar.json";

    public ContextoDados()
    {
        Mesas = new List<Mesa>();
        Garcons = new List<Garcom>();
    }

    public void VerificarSistemaOperacional()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            pastaArmazenamento = @"C:\temp";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            pastaArmazenamento = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "temp");
        }
    }

    public ContextoDados(bool carregarDados) : this()
    {
        if (carregarDados)
            Carregar();
    }

    public void Salvar()
    {
        VerificarSistemaOperacional();
        string caminhoCompleto = Path.Combine(pastaArmazenamento, arquivoArmazenamento);

        JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        jsonOptions.WriteIndented = true;
        jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

        string json = JsonSerializer.Serialize(this, jsonOptions);

        if (!Directory.Exists(pastaArmazenamento))
            Directory.CreateDirectory(pastaArmazenamento);

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

        ContextoDados contextoArmazenado = JsonSerializer.Deserialize<ContextoDados>(json, jsonOptions)!;

        if (contextoArmazenado == null) return;

        Mesas = contextoArmazenado.Mesas;
        Garcons = contextoArmazenado.Garcons;
    }
}