using C971_Ogden.ViewModel;

namespace C971_Ogden.Pages;

public partial class AllClasses : ContentPage
{
    public AllClasses(AllClassesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}