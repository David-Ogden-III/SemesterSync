using SemesterSync.Database;
using SemesterSync.ViewModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace SemesterSync.Views;

public partial class AllClasses : ContentPage
{
    private readonly AllClassesViewModel viewModel;
    public AllClasses(AllClassesViewModel vm)
    {
        InitializeComponent();
        viewModel = vm;
        BindingContext = vm;

    }
}