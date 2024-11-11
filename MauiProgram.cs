using Microsoft.Extensions.Logging;
using Plugin.Maui.SwipeCardView;
using DotNet.Meteor.HotReload.Plugin;
using CommunityToolkit.Maui;

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
			.UseMauiCommunityToolkit()
			#endif
			.UseSwipeCardView()			
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
