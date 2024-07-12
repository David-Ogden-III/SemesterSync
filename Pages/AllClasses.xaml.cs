using C971_Ogden.ViewModel;
using C971_Ogden.Database;
using System.Diagnostics;

namespace C971_Ogden.Pages;

public partial class AllClasses : ContentPage
{
    public AllClasses(AllClassesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}