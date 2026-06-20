using Microsoft.Extensions.Logging;
using EventAssetTracker.Services;
using EventAssetTracker.ViewModels;
using EventAssetTracker.Views;

namespace EventAssetTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
			// ---- Base de Datos ----
            // Esta es la clase para que trabaje la companera encargada de la base de datos.
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<ListaActivosViewModel>();
            builder.Services.AddTransient<ActivoFormViewModel>();
            builder.Services.AddTransient<CambiarEstadoViewModel>();
            builder.Services.AddTransient<HistorialViewModel>();

            // ---- Views ----
            // Estas son las variables para que trabaje la companera encargada del view.
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<ListaActivosPage>();
            builder.Services.AddTransient<ActivoFormPage>();
            builder.Services.AddTransient<CambiarEstadoPage>();
            builder.Services.AddTransient<HistorialPage>();

            return builder.Build();
        }
    }
}
