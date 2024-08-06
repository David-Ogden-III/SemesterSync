using System.Diagnostics;
using ViewModelLibrary;
using ModelLibrary;
using CommunityToolkit.Maui.Core;

namespace SemesterSync.Views;

public partial class ActiveTerm : ContentPage
{
    private readonly ActiveTermViewModel viewModel;
    private readonly IPopupService popupService;
    public ActiveTerm(ActiveTermViewModel vm, IPopupService popupService)
    {
        InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
        this.popupService = popupService;
    }

    private async void EllipsisClicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;
        Class selectedClass = (Class)clickedButton.BindingContext;
        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedClass.ClassName, "Cancel", null, "Edit", "Detailed View", "Set Notification", "Remove From Term");
        Debug.WriteLine("Action: " + action);

        switch (action)
        {
            case "Edit":
                await Shell.Current.GoToAsync(nameof(UpdateClass),
                new Dictionary<string, object>
                {
                {"SelectedClass", selectedClass }
                });
                break;
            case "Remove From Term":
                viewModel.DeleteClassCommand.Execute(selectedClass);
                break;
            case "Detailed View":
                await Shell.Current.GoToAsync(nameof(DetailedClass),
                new Dictionary<string, object>
                {
                {"SelectedClass", selectedClass }
                });
                break;
            case "Set Notification":
                viewModel.ShowPopup(selectedClass);
                break;
            default:
                Debug.WriteLine("No Action Selected");
                break;
        }
    }
}
