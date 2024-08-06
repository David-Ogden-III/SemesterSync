using ViewModelLibrary;

namespace SemesterSync.Views;

public partial class TermDetails : ContentPage
{
    public TermDetails(TermDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}