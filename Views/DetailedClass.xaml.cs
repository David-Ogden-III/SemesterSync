using SemesterSync.ViewModel;

namespace SemesterSync.Views;

public partial class DetailedClass : ContentPage
{
    public DetailedClass(DetailedClassViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}