using Microsoft.Extensions.Logging;
using DotNet.Meteor.HotReload.Plugin;

namespace App_Imobiliaria_appMobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			#if DEBUG
            .EnableHotReload()
			#endif
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

				fonts.AddFont("Angels.ttf", "font01");
				fonts.AddFont("MIROLES.ttf", "font02");

				
				
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
