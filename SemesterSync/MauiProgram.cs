﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using SemesterSync.Views;
using ViewModelLibrary;

namespace SemesterSync
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
                .UseMauiCommunityToolkit()
                .UseLocalNotification();



            builder.Services.AddSingleton<Login>();
            builder.Services.AddSingleton<LoginViewModel>();

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

            builder.Services.AddTransientPopup<AddModifyExamPopup, AddModifyExamPopupViewModel>();
            builder.Services.AddTransientPopup<NotificationPopup, NotificationPopupViewModel>();

            builder.Services.AddSingleton<Profile>();
            builder.Services.AddSingleton<ProfileViewModel>();

            builder.Services.AddSingleton<Progress>();
            builder.Services.AddSingleton<ProgressViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
            Task.Run(async () => await MockData.CreateAllMockData());
#endif

            return builder.Build();
        }
    }
}
