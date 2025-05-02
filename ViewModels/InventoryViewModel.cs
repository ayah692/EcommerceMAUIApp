using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using EcommerceMAUIApp.Models;
using Microsoft.Maui.Controls;

namespace EcommerceMAUIApp.ViewModels
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Inventory { get; set; }

        private ObservableCollection<Product> _shoppingCart;
        public ObservableCollection<Product> ShoppingCart
        {
            get => _shoppingCart;
            set
            {
                _shoppingCart = value;
                OnPropertyChanged(nameof(ShoppingCart));
                OnPropertyChanged(nameof(Subtotal));
                OnPropertyChanged(nameof(Tax));
                OnPropertyChanged(nameof(Total));
            }
        }

        public Dictionary<string, ObservableCollection<Product>> Carts { get; set; }
        //public ObservableCollection<string> CartNames => new ObservableCollection<string>(Carts.Keys);
private ObservableCollection<string> _cartNames;
public ObservableCollection<string> CartNames
{
    get => _cartNames;
    set
    {
        _cartNames = value;
        OnPropertyChanged(nameof(CartNames));
    }
}


        private double _taxRate = 7.0;
        public double TaxRate
        {
            get => _taxRate;
            set
            {
                if (_taxRate != value)
                {
                    _taxRate = value;
                    OnPropertyChanged(nameof(TaxRate));
                    OnPropertyChanged(nameof(Tax));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        private string _selectedCart;
        public string SelectedCart
        {
            get => _selectedCart;
            set
            {
                if (_selectedCart != value)
                {
                    _selectedCart = value;
                    OnPropertyChanged(nameof(SelectedCart));

                    if (Carts.ContainsKey(value))
                    {
                        ShoppingCart = Carts[value];
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public InventoryViewModel()
        {
            Inventory = new ObservableCollection<Product>
            {
                new Product("Laptop", 10, 1000),
                new Product("Phone", 15, 500),
                new Product("Tablet", 8, 750),
                new Product("Headphones", 20, 150),
                new Product("Mouse", 30, 25)
            };

            Carts = new Dictionary<string, ObservableCollection<Product>>();
            CartNames = new ObservableCollection<string>();
            CreateNewCart("Main Cart");
            SelectedCart = "Main Cart";
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
public void CreateNewCart(string cartName)
{
    if (!Carts.ContainsKey(cartName))
    {
        Carts[cartName] = new ObservableCollection<Product>();
        CartNames.Add(cartName); //  THIS updates the Picker
    }

    SelectedCart = cartName;
}

     
        public void AddToCart(Product product)
        {
            if (product == null || product.Quantity <= 0) return;

            var cartItem = ShoppingCart.FirstOrDefault(p => p.Name == product.Name);
            var inventoryItem = Inventory.FirstOrDefault(p => p.Name == product.Name);

            if (inventoryItem != null && inventoryItem.Quantity > 0)
            {
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    ShoppingCart.Add(new Product(product.Name, 1, product.Price));
                }

                inventoryItem.Quantity--;
                OnPropertyChanged(nameof(ShoppingCart));
                OnPropertyChanged(nameof(Inventory));
            }
        }

        public void AddCustomQuantityToCart(Product product)
        {
            if (product == null || product.UserEnteredQuantity <= 0) return;

            var inventoryItem = Inventory.FirstOrDefault(p => p.Name == product.Name);
            var cartItem = ShoppingCart.FirstOrDefault(p => p.Name == product.Name);

            int qtyToAdd = Math.Min(product.UserEnteredQuantity, inventoryItem.Quantity);

            if (qtyToAdd <= 0) return;

            if (cartItem != null)
            {
                cartItem.Quantity += qtyToAdd;
            }
            else
            {
                ShoppingCart.Add(new Product(product.Name, qtyToAdd, product.Price));
            }

            inventoryItem.Quantity -= qtyToAdd;
            product.UserEnteredQuantity = 1;

            OnPropertyChanged(nameof(ShoppingCart));
            OnPropertyChanged(nameof(Inventory));
        }

        public void RemoveFromCart(Product product)
        {
            if (product == null) return;

            var cartItem = ShoppingCart.FirstOrDefault(p => p.Name == product.Name);
            var inventoryItem = Inventory.FirstOrDefault(p => p.Name == product.Name);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    inventoryItem.Quantity++;
                }
                else
                {
                    ShoppingCart.Remove(cartItem);
                    inventoryItem.Quantity++;
                }
            }

            OnPropertyChanged(nameof(ShoppingCart));
            OnPropertyChanged(nameof(Inventory));
        }

        public void IncreaseCartQuantity(Product product)
        {
            if (product == null) return;

            var cartItem = ShoppingCart.FirstOrDefault(p => p.Name == product.Name);
            var inventoryItem = Inventory.FirstOrDefault(p => p.Name == product.Name);

            if (cartItem != null && inventoryItem != null && inventoryItem.Quantity > 0)
            {
                cartItem.Quantity++;
                inventoryItem.Quantity--;

                OnPropertyChanged(nameof(ShoppingCart));
                OnPropertyChanged(nameof(Inventory));
            }
        }

        public void ReturnAllToInventory(Product product)
        {
            if (product == null) return;

            var cartItem = ShoppingCart.FirstOrDefault(p => p.Name == product.Name);
            var inventoryItem = Inventory.FirstOrDefault(p => p.Name == product.Name);

            if (cartItem != null && inventoryItem != null)
            {
                inventoryItem.Quantity += cartItem.Quantity;
                ShoppingCart.Remove(cartItem);
            }

            OnPropertyChanged(nameof(ShoppingCart));
            OnPropertyChanged(nameof(Inventory));
        }

        public void AddProductToInventory(string name, int quantity, double price)
        {
            if (string.IsNullOrWhiteSpace(name) || quantity <= 0 || price <= 0)
            {
                Application.Current.MainPage?.DisplayAlert("Invalid Input", "Please enter valid product details.", "OK");
                return;
            }

            var existingProduct = Inventory.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (existingProduct != null)
            {
                existingProduct.Quantity += quantity;
            }
            else
            {
                Inventory.Add(new Product(name, quantity, price));
            }

            OnPropertyChanged(nameof(Inventory));
        }

        public void EditProductInInventory(Product product, string newName, int newQuantity, double newPrice)
        {
            if (product == null || string.IsNullOrWhiteSpace(newName) || newQuantity < 0 || newPrice < 0) return;

            var existingProduct = Inventory.FirstOrDefault(p => p.Name == product.Name);

            if (existingProduct != null)
            {
                existingProduct.Name = newName;
                existingProduct.Quantity = newQuantity;
                existingProduct.Price = newPrice;

                OnPropertyChanged(nameof(Inventory));
            }
        }

        public void DeleteProductFromInventory(Product product)
        {
            if (product == null) return;

            var cartItem = ShoppingCart.FirstOrDefault(p => p.Name == product.Name);
            if (cartItem != null)
            {
                ShoppingCart.Remove(cartItem);
                OnPropertyChanged(nameof(ShoppingCart));
            }

            Inventory.Remove(product);
            OnPropertyChanged(nameof(Inventory));
        }

        public double Subtotal => ShoppingCart.Sum(p => p.Price * p.Quantity);
        public double Tax => Math.Round(Subtotal * (TaxRate / 100), 2);
        public double Total => Math.Round(Subtotal + Tax, 2);

        public async Task CompleteCheckout()
        {
            if (ShoppingCart.Count == 0)
            {
                await Application.Current.MainPage?.DisplayAlert("Cart is Empty", "There are no items in the cart to checkout.", "OK");
                return;
            }

            string receipt = "--- Receipt ---\n";
            foreach (var product in ShoppingCart)
            {
                receipt += $"{product.Name} - Quantity: {product.Quantity} - Price: ${product.Price:F2}\n";
            }
            receipt += $"Subtotal: ${Subtotal:F2}\n";
            receipt += $"Tax ({TaxRate}%): ${Tax:F2}\n";
            receipt += $"Total: ${Total:F2}\n";
            receipt += "Thank you for shopping!";

            await Application.Current.MainPage?.DisplayAlert("Checkout", receipt, "OK");

            ShoppingCart.Clear();
            OnPropertyChanged(nameof(ShoppingCart));
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }
public void SwitchCart(string cartName)
{
    if (!string.IsNullOrWhiteSpace(cartName) && Carts.ContainsKey(cartName))
    {
        SelectedCart = cartName;
        ShoppingCart = Carts[cartName];
        OnPropertyChanged(nameof(ShoppingCart));
        OnPropertyChanged(nameof(SelectedCart));
    }
}

        public void SortCart(string criterion)
        {
            var sorted = criterion switch
            {
                "Price" => ShoppingCart.OrderBy(p => p.Price).ToList(),
                _ => ShoppingCart.OrderBy(p => p.Name).ToList()
            };

            ShoppingCart.Clear();
            foreach (var item in sorted)
            {
                ShoppingCart.Add(item);
            }
        }

        public void SortInventory(string criterion)
        {
            var sorted = criterion switch
            {
                "Price" => Inventory.OrderBy(p => p.Price).ToList(),
                _ => Inventory.OrderBy(p => p.Name).ToList()
            };

            Inventory.Clear();
            foreach (var item in sorted)
            {
                Inventory.Add(item);
            }
        }
    }
}
