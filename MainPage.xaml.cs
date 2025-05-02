using Microsoft.Maui.Controls;

namespace EcommerceMAUIApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void GoToInventoryPage(object sender, EventArgs e)
        {
            try { Vibration.Vibrate(50); } catch { } // Optional haptic feedback
            await Shell.Current.GoToAsync("//InventoryPage");
        }

        async void GoToCartPage(object sender, EventArgs e)
        {
            try { Vibration.Vibrate(50); } catch { } // Optional haptic feedback
            await Shell.Current.GoToAsync("//CartPage");
        }

       async void GoToSettingsPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SettingsPage"); // changed from Navigation.PushAsync
    }
}
}
