using C971_Ogden.ViewModel;

namespace C971_Ogden.Pages;

public partial class UpdateClass : ContentPage
{
	public UpdateClass(UpdateClassViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}