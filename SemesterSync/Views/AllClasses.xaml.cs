using ModelLibrary;
using System.Diagnostics;
using ViewModelLibrary;

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

    private async void EllipsisClicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;
        Class selectedClass = (Class)clickedButton.BindingContext;

        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedClass.ClassName, "Cancel", null, "Edit", "Detailed View", "Set Notification", "Delete");
        Debug.WriteLine("Action: " + action);

        switch (action)
        {
            case "Edit":
                await NavigateToUpdateClass(selectedClass);
                break;
            case "Delete":
                viewModel.DeleteClassCommand.Execute(selectedClass);
                break;
            case "Detailed View":
                await NavigateToDetailedView(selectedClass);
                break;
            case "Set Notification":
                viewModel.ShowPopup(selectedClass);
                break;
            default:
                Debug.WriteLine("No Action Selected");
                break;
        }
    }

    private async Task NavigateToUpdateClass(Class selectedClass)
    {
        await Shell.Current.GoToAsync(nameof(UpdateClass),
                new Dictionary<string, object>
                {
                {"SelectedClass", selectedClass }
                });
    }

    private async Task NavigateToDetailedView(Class selectedClass)
    {
        await Shell.Current.GoToAsync(nameof(DetailedClass),
                new Dictionary<string, object>
                {
                {"SelectedClass", selectedClass }
                });
    }

    private async void NavigateToUpdateClass(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UpdateClass));
    }
}