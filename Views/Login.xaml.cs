using SemesterSync.ViewModel;
namespace SemesterSync.Views;


public partial class Login : ContentPage
{
    public Login(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}