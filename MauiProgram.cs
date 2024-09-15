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
				fonts.AddFont("Mondapick.ttf", "font03");
				fonts.AddFont("celticmd.ttf",  "font04");

				fonts.AddFont("Birds.ttf",  "font05");
				fonts.AddFont("MIROLS.ttf",  "font06");
				fonts.AddFont("Ruler Stencil Bold.ttf",  "font07");
				fonts.AddFont("Ruler Stencil Light.ttf",  "font08");
				fonts.AddFont("Ruler Stencil Regular.ttf",  "font09");
				
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
