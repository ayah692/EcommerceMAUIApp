using EcommerceMAUIApp.Models;
using EcommerceMAUIApp.ViewModels;

namespace EcommerceMAUIApp.Views;

public partial class CartPage : ContentPage
{
    public CartPage()
    {
        InitializeComponent();
        BindingContext = App.InventoryViewModel;
    }

    private void RemoveFromCart_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Product product)
        {
            App.InventoryViewModel.RemoveFromCart(product);
        }
    }

    private async void GoToCheckoutPage(object sender, EventArgs e)
    {
        await App.InventoryViewModel.CompleteCheckout();
    }

    private void IncreaseQuantity_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Product product)
        {
            App.InventoryViewModel.IncreaseCartQuantity(product);
        }
    }

    private void ReturnAllToInventory_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Product product)
        {
            App.InventoryViewModel.ReturnAllToInventory(product);
        }
    }

    private async void NewCart_Clicked(object sender, EventArgs e)
    {
        string cartName = await DisplayPromptAsync("New Cart", "Enter a name for the new cart:", initialValue: "Wishlist");
        if (!string.IsNullOrWhiteSpace(cartName))
        {
            App.InventoryViewModel.CreateNewCart(cartName);
        }
    }


    private void OnSortChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        var selected = picker?.SelectedItem?.ToString();
        if (!string.IsNullOrEmpty(selected))
        {
            App.InventoryViewModel.SortCart(selected);
        }
    }
private async void GoBackToMain(object sender, EventArgs e)
{
    await Shell.Current.GoToAsync("//MainPage");
}


    private void OnCartPickerChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker.SelectedItem is string cartName)
        {
            App.InventoryViewModel.SelectedCart = cartName;
        }
    }
private async void SwitchCart_Clicked(object sender, EventArgs e)
{
    var allCarts = App.InventoryViewModel.CartNames;
    string current = App.InventoryViewModel.SelectedCart;

    var otherCarts = allCarts.Where(c => c != current).ToArray();

    if (otherCarts.Length == 0)
    {
        await DisplayAlert("No Other Carts", "You have no other carts to switch to.", "OK");
        return;
    }

    string selectedCart = await DisplayActionSheet("Switch to Cart", "Cancel", null, otherCarts);

    if (!string.IsNullOrEmpty(selectedCart) && selectedCart != "Cancel")
    {
        App.InventoryViewModel.SelectedCart = selectedCart;
    }
}

   private async void MoveToAnotherCart_Clicked(object sender, EventArgs e)
{
    if (sender is Button button && button.BindingContext is Product product)
    {
        // Get all cart names except the current one
        var cartNames = App.InventoryViewModel.CartNames
            .Where(c => c != App.InventoryViewModel.SelectedCart)
            .ToList();

        if (cartNames.Count == 0)
        {
            await DisplayAlert("No Wishlist", "No other cart to move the item to.", "OK");
            return;
        }

        // Prompt user to select a cart to move the item to
        string toCart = await DisplayActionSheet("Move to Wishlist", "Cancel", null, cartNames.ToArray());
        
        if (!string.IsNullOrEmpty(toCart) && toCart != "Cancel")
        {
            // Get the target cart
            var targetCart = App.InventoryViewModel.Carts[toCart];
            
            // Try to find the product in the target cart
            var targetCartItem = targetCart.FirstOrDefault(p => p.Name == product.Name);

            // Add the product to the target cart
            if (targetCartItem != null)
            {
                targetCartItem.Quantity += product.Quantity;
            }
            else
            {
                targetCart.Add(new Product(product.Name, product.Quantity, product.Price));
            }

            // Remove the product from the current cart
            App.InventoryViewModel.ShoppingCart.Remove(product);

            // Notify UI to refresh
            App.InventoryViewModel.OnPropertyChanged(nameof(App.InventoryViewModel.ShoppingCart));
            App.InventoryViewModel.OnPropertyChanged(nameof(App.InventoryViewModel.Carts));
        }
    }
}
}
