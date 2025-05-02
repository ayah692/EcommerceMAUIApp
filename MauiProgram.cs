using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;
using EcommerceMAUIApp.ViewModels;
using EcommerceMAUIApp.Views;

namespace EcommerceMAUIApp
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()  //  Ensure 'App' is properly referenced
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddSingleton<InventoryViewModel>();
			builder.Services.AddSingleton<InventoryPage>();
			builder.Services.AddSingleton<CartPage>();
			builder.Services.AddSingleton<CheckoutPage>();

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
