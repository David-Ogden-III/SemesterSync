using C971_Ogden.Database;
using C971_Ogden.Pages;
using C971_Ogden.ViewModel;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace C971_Ogden
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
                })
                .UseMauiCommunityToolkit();

            Task.Run(async () => await MockData.CreateAllMockData());

            builder.Services.AddSingleton<ActiveTerm>();
            builder.Services.AddSingleton<ActiveTermViewModel>();

            builder.Services.AddSingleton<AllTerms>();
            builder.Services.AddSingleton<AllTermsViewModel>();

            builder.Services.AddSingleton<AllClasses>();
            builder.Services.AddSingleton<AllClassesViewModel>();

            builder.Services.AddTransient<TermDetails>();
            builder.Services.AddTransient<TermDetailsViewModel>();

            builder.Services.AddTransient<DetailedClass>();
            builder.Services.AddTransient<DetailedClassViewModel>();

            builder.Services.AddTransient<UpdateClass>();
            builder.Services.AddTransient<UpdateClassViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
