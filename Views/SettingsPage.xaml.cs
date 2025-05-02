using Microsoft.Maui.Controls;
using EcommerceMAUIApp.ViewModels;

namespace EcommerceMAUIApp.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = App.InventoryViewModel;
    }
private async void GoBackToMain(object sender, EventArgs e)
{
    await Shell.Current.GoToAsync("//MainPage");
}


   private async void SaveButton_Clicked(object sender, EventArgs e)
{
        if (double.TryParse(TaxRateEntry.Text, out double newTax))
        {
            App.InventoryViewModel.TaxRate = newTax;
            await DisplayAlert("Saved", $"Tax rate set to {newTax}%", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Please enter a valid number.", "OK");
        }
    }
    
}
