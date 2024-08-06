using ViewModelLibrary;
namespace SemesterSync.Views;


public partial class Login : ContentPage
{
    private readonly LoginViewModel viewModel;
    public Login(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
    }

    private async void SubmitClicked(object sender, EventArgs e)
    {
        bool success = await viewModel.Submit();
        if (success)
        {
            await Shell.Current.GoToAsync($"//{nameof(ActiveTerm)}");
        }
    }
}