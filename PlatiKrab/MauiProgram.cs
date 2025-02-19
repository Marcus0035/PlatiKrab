using KeyChordFinder.Data;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using PlatiKrab.Data;
using System.Reflection;
using PlatiKrab.Services;
using PlatiKrab.Data.Models;

namespace PlatiKrab
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
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddMudServices();

            //Copy the database file from the embedded resources to the local storage
            var assembly = Assembly.GetExecutingAssembly();
            DbHelper.CopyIfDoesntExist("PlatiKrab.db", assembly);
            //Add Context
            builder.Services.AddDbContext<PlatiKrabDbContext>();
            builder.Services.AddSingleton<PlayersService>();

            var userSettings = PlatiKrabDbContext.GetUserSettings();
            builder.Services.AddSingleton(provider => new QRService(userSettings.AccountNumber, userSettings.BankCode));



#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
            
#endif

            return builder.Build();
        }
    }
}
