using SemesterSync.ViewModel;

namespace SemesterSync.Views;

public partial class AllTerms : ContentPage
{
    public AllTerms(AllTermsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}