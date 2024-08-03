using SemesterSync.ViewModel;

namespace SemesterSync.Views;

public partial class Profile : ContentPage
{
    public Profile(ProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}