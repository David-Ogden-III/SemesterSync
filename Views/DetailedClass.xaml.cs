using C971_Ogden.ViewModel;

namespace C971_Ogden.Views;

public partial class DetailedClass : ContentPage
{
    public DetailedClass(DetailedClassViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}