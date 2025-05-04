using Microsoft.Extensions.Logging;

namespace ImprovingPhysicalHealthApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Loads the main app
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    // uses custom fonts
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            
            builder.Logging.AddDebug();
#endif

            return builder.Build(); // finish and return app
        }
    }
}
