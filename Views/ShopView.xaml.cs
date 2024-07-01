using InventoryManagement.ViewModels;
using System.Runtime.CompilerServices;
namespace InventoryManagement.views;

public partial class ShopView : ContentPage
{
    public ShopView()
    {
        InitializeComponent();

        BindingContext = new InventoryViewModel();
    }
    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void InventorySearchClicked(object sender, EventArgs e)
    {
        (BindingContext as ShopViewModel).Search();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as ShopViewModel).Refresh();
    }

    private void PlaceInCartClicked(object sender, EventArgs e)
    {
        (BindingContext as ShopViewModel).PlaceInCart();
    }
}
