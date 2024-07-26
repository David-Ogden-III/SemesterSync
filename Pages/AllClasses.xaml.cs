using C971_Ogden.Database;
using C971_Ogden.ViewModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace C971_Ogden.Pages;

public partial class AllClasses : ContentPage
{
    private readonly AllClassesViewModel viewModel;
    public AllClasses(AllClassesViewModel vm)
    {
        InitializeComponent();
        viewModel = vm;
        BindingContext = vm;

    }
}