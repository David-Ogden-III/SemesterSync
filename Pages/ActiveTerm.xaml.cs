using C971_Ogden.Database;
using C971_Ogden.ViewModel;
using System.Diagnostics;

namespace C971_Ogden.Pages;

public partial class ActiveTerm : ContentPage
{
    public ActiveTerm(ActiveTermViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
