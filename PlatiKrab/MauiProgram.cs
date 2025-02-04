using KeyChordFinder.Data;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using PlatiKrab.Data;
using PlatiKrab.Data.Models;
using System.Reflection;

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
            //Initialize the database for the first time
            DbHelper.DbInitializer();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
