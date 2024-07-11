using C971_Ogden.ViewModel;

namespace C971_Ogden.Pages;

public partial class TermDetails : ContentPage
{
    public TermDetails(TermDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}