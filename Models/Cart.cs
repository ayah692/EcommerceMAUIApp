// Models/Cart.cs
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EcommerceMAUIApp.Models
{
    public class Cart : INotifyPropertyChanged
    {
        private string _name;
        private ObservableCollection<Product> _items;
        
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
        
        public ObservableCollection<Product> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public Cart(string name)
        {
            _name = name;
            _items = new ObservableCollection<Product>();
        }
    }
}