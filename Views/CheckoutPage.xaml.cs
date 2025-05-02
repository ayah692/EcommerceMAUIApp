using EcommerceMAUIApp.ViewModels;

namespace EcommerceMAUIApp.Views;

public partial class CheckoutPage : ContentPage
{
    public CheckoutPage()
    {
        InitializeComponent();
        BindingContext = App.InventoryViewModel;
    }

    private async void CompleteCheckout_Clicked(object sender, EventArgs e)
    {
        await App.InventoryViewModel.CompleteCheckout();
        await Shell.Current.GoToAsync("//CartPage");
    }

    private async void GoBack_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//CartPage");
    }
}
