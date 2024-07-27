using C971_Ogden.ViewModel;

namespace C971_Ogden.Views;

public partial class UpdateClass : ContentPage
{
    public UpdateClass(UpdateClassViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}