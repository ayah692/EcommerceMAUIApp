using EcommerceMAUIApp.ViewModels;
using Microsoft.Maui.Controls;

namespace EcommerceMAUIApp
{
    public partial class App : Application
    {
        public static InventoryViewModel InventoryViewModel { get; private set; }

        public App()
        {
            InitializeComponent();
            InventoryViewModel = new InventoryViewModel(); //  Shared ViewModel
            MainPage = new AppShell
            {
                BindingContext = InventoryViewModel //  Now shared with every page
            };
        }
    }
}
