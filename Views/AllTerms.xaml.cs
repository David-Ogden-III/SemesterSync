using C971_Ogden.ViewModel;

namespace C971_Ogden.Views;

public partial class AllTerms : ContentPage
{
    public AllTerms(AllTermsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}