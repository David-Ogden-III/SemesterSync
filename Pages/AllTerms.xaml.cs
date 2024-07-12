using C971_Ogden.ViewModel;

namespace C971_Ogden.Pages;

public partial class AllTerms : ContentPage
{
    public AllTerms(AllTermsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}