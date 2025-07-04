using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrutura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloConta;
using ControleDeBar.Infraestrutura.SQLServer.ModuloGarcom;
using ControleDeBar.Infraestrutura.SQLServer.ModuloMesa;
using ControleDeBar.Infraestrutura.SQLServer.ModuloProduto;
using ControleDeBar.WebApp.ActionFilters;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace ControleDeBar.WebApp
{
#pragma warning disable RCS1102 // Make class static
    public class Program
#pragma warning restore RCS1102 // Make class static
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews((options) =>
            {
                options.Filters.Add<ValidarModeloAttribute>();
                options.Filters.Add<LogarAcaoAttribute>();
            });
            builder.Services.AddScoped((IServiceProvider _) => new ContextoDados(true));
            builder.Services.AddScoped<IRepositorioConta, RepositorioContaEmArquivo>();
            builder.Services.AddScoped<IRepositorioGarcom, RepositorioGarcomSQL>();
            builder.Services.AddScoped<IRepositorioMesa, RepositorioMesaSQL>();
            builder.Services.AddScoped<IRepositorioProduto, RepositorioProdutoSQL>();

            string caminhoAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string caminhoArquivo = Path.Combine(caminhoAppData, "Controle-de-Bar", "erro.log");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new CompactJsonFormatter(), caminhoArquivo, LogEventLevel.Error)
                .CreateLogger();

            builder.Logging.ClearProviders();

            builder.Services.AddSerilog(Log.Logger);

            WebApplication app = builder.Build();

            if (!app.Environment.IsDevelopment())
                app.UseExceptionHandler("/erro");
            else
                app.UseDeveloperExceptionPage();

            app.UseAntiforgery();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}
