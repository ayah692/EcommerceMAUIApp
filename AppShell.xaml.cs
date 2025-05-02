namespace EcommerceMAUIApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes
        Routing.RegisterRoute(nameof(Views.InventoryPage), typeof(Views.InventoryPage));
        Routing.RegisterRoute(nameof(Views.CartPage), typeof(Views.CartPage));
        Routing.RegisterRoute(nameof(Views.SettingsPage), typeof(Views.SettingsPage));
        Routing.RegisterRoute(nameof(Views.CheckoutPage), typeof(Views.CheckoutPage));
    }
}