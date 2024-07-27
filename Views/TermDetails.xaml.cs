using C971_Ogden.ViewModel;

namespace C971_Ogden.Views;

public partial class TermDetails : ContentPage
{
    public TermDetails(TermDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}