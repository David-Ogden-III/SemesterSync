using SemesterSync.ViewModel;

namespace SemesterSync.Views;

public partial class UpdateClass : ContentPage
{
    public UpdateClass(UpdateClassViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}