using Microsoft.Extensions.Logging;
using DotNet.Meteor.HotReload.Plugin;
using Plugin.Maui.SwipeCardView;

namespace App_Imobiliaria_appMobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSwipeCardView()
			#if DEBUG
            .EnableHotReload()
			#endif
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

				fonts.AddFont("Montserrat-Bold.ttf",  "font01");
				fonts.AddFont("Montserrat-Light.ttf",  "font02");
				fonts.AddFont("Montserrat-Medium.ttf",  "font03");
				fonts.AddFont("Popping-Cute.ttf",  "font04");
				fonts.AddFont("Poppins-Thin.ttf",  "font05");

			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
