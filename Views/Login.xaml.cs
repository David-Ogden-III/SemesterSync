using C971_Ogden.ViewModel;
namespace C971_Ogden.Views;


public partial class Login : ContentPage
{
	public Login(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}