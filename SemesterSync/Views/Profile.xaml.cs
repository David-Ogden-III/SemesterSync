using ViewModelLibrary;

namespace SemesterSync.Views;

public partial class Profile : ContentPage
{
    public Profile(ProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void Ellipsis_Clicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Select", "Cancel", null, "Status Reports", "Logout");

        switch (action)
        {
            case "Status Reports":
                await Shell.Current.GoToAsync(nameof(Progress));
                break;
            case "Logout":
                bool removed = SecureStorage.Remove("activeUser");
                await Shell.Current.GoToAsync($"//{nameof(Login)}");
                break;
        }
    }
}