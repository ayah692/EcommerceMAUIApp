using EcommerceMAUIApp.Models;
using EcommerceMAUIApp.ViewModels;

namespace EcommerceMAUIApp.Views;
public partial class InventoryPage : ContentPage
{
   // private InventoryViewModel _viewModel;

    public InventoryPage()
    {
        InitializeComponent();
        BindingContext = App.InventoryViewModel;  // Use shared ViewModel
    }


    private void AddToCart_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var product = button?.BindingContext as Product;

        if (product != null)
        {
            App.InventoryViewModel.AddToCart(product);  //  Use shared ViewModel
        }
    }
    private async void ShowAddProductPopup(object sender, EventArgs e)
    {
        string name = await DisplayPromptAsync("New Product", "Enter product name:");
        if (string.IsNullOrWhiteSpace(name)) return;

        string quantityStr = await DisplayPromptAsync("New Product", "Enter quantity:");
        if (!int.TryParse(quantityStr, out int quantity) || quantity <= 0)
        {
            await DisplayAlert("Invalid Input", "Quantity must be a positive number.", "OK");
            return;
        }

        string priceStr = await DisplayPromptAsync("New Product", "Enter price:");
        if (!double.TryParse(priceStr, out double price) || price <= 0)
        {
            await DisplayAlert("Invalid Input", "Price must be a positive number.", "OK");
            return;
        }

        //  Call ViewModel to add the product
        App.InventoryViewModel.AddProductToInventory(name, quantity, price);

        //  Ensure UI updates
        OnPropertyChanged(nameof(App.InventoryViewModel.Inventory));
    }



    private async void EditProduct_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var product = button?.BindingContext as Product;
        if (product == null) return;

        string newName = await DisplayPromptAsync("Edit Product", "Enter new product name:", initialValue: product.Name);
        if (string.IsNullOrWhiteSpace(newName)) return;

        string newQuantityStr = await DisplayPromptAsync("Edit Product", "Enter new quantity:", initialValue: product.Quantity.ToString());
        if (!int.TryParse(newQuantityStr, out int newQuantity) || newQuantity < 0) return;

        string newPriceStr = await DisplayPromptAsync("Edit Product", "Enter new price:", initialValue: product.Price.ToString());
        if (!double.TryParse(newPriceStr, out double newPrice) || newPrice < 0) return;

        //Call ViewModel to update the product
        App.InventoryViewModel.EditProductInInventory(product, newName, newQuantity, newPrice);
    }


    private async void DeleteProduct_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var product = button?.BindingContext as Product;
        if (product == null) return;

        bool confirm = await DisplayAlert("Delete Product", $"Are you sure you want to delete {product.Name}?", "Yes", "No");
        if (confirm)
        {
            App.InventoryViewModel.DeleteProductFromInventory(product);
        }
    }
private void AddCustomQuantity_Clicked(object sender, EventArgs e)
{
    if (sender is Button button && button.BindingContext is Product product)
    {
        var viewModel = BindingContext as InventoryViewModel;
        viewModel?.AddCustomQuantityToCart(product);
    }
}
private void OnSortChanged(object sender, EventArgs e)
{
    var picker = sender as Picker;
    var selected = picker?.SelectedItem?.ToString();
    if (!string.IsNullOrEmpty(selected))
    {
        App.InventoryViewModel.SortInventory(selected);
    }
}
private async void GoBackToMain(object sender, EventArgs e)
{
    await Shell.Current.GoToAsync("//MainPage");
}


private void OnSortInventoryChanged(object sender, EventArgs e)
{
    var picker = sender as Picker;
    var selected = picker?.SelectedItem?.ToString();

    if (!string.IsNullOrEmpty(selected))
    {
        App.InventoryViewModel.SortInventory(selected);
    }
}



}