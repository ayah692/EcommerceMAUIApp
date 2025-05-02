using System.ComponentModel;
using EcommerceMAUIApp.Models;

namespace EcommerceMAUIApp.Models
{
    public class Product : INotifyPropertyChanged
    {
        private string _name;
        private int _quantity;
        private double _price;
        private int _userEnteredQuantity = 1;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public double Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public int UserEnteredQuantity // ✅ New Property
        {
            get => _userEnteredQuantity;
            set
            {
                if (_userEnteredQuantity != value)
                {
                    _userEnteredQuantity = value;
                    OnPropertyChanged(nameof(UserEnteredQuantity));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Product(string name, int quantity, double price)
        {
            _name = name;
            _quantity = quantity;
            _price = price;
        }
    }
}
