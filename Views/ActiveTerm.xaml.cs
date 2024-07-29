using SemesterSync.ViewModel;

namespace SemesterSync.Views;

public partial class ActiveTerm : ContentPage
{
    public ActiveTerm(ActiveTermViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
