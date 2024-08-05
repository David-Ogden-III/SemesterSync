using SemesterSync.ViewModel;
using System.Diagnostics;

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
        string action = await DisplayActionSheet("Select", "Cancel", null, "Progress Reports", "Logout");

        switch (action)
        {
            case "Progress Reports":
                Debug.WriteLine("prog repo");
                break;
            case "Logout":
                SecureStorage.Remove("ActiveUser");
                await Shell.Current.GoToAsync($"//{nameof(Login)}");
                break;
        }
    }
}