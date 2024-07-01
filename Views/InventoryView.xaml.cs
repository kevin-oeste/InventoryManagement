using InventoryManagement.ViewModels;
using System.Runtime.CompilerServices;
namespace InventoryManagement.views;

public partial class InventoryView : ContentPage
{
    public InventoryView()
    {
        InitializeComponent();

        BindingContext = new InventoryViewModel();
    }
    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void AddClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Product");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as ShopViewModel).Refresh();
    }
}
