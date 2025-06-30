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

            string caminhoAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string caminhoArquivo = Path.Combine(caminhoAppData, "eAgenda", "erro.log");

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
