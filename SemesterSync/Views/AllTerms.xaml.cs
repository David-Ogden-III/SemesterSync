using System.Diagnostics;
using ModelLibrary;
using ViewModelLibrary;

namespace SemesterSync.Views;

public partial class AllTerms : ContentPage
{
    private readonly AllTermsViewModel viewModel;
    public AllTerms(AllTermsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
    }


    // Term Methods
    private async void TermEllipsisClicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;
        ClassGroup selectedCG = (ClassGroup)clickedButton.BindingContext;

        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedCG.Term.TermName, "Cancel", null, "Edit", "Delete");
        Debug.WriteLine("Action: " + action);

        switch (action)
        {
            case "Edit":
                await NavigateToTermDetails(selectedCG);
                break;
            case "Delete":
                viewModel.DeleteTermCommand.Execute(selectedCG);
                break;
            default:
                Debug.WriteLine("No Action Selected");
                break;
        }
    }

    private async Task NavigateToTermDetails(ClassGroup selectedCG)
    {
        await Shell.Current.GoToAsync(nameof(TermDetails),
            new Dictionary<string, object>
            {
                {"SelectedCG", selectedCG }
            });
    }

    private async void NavigateToTermDetails(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TermDetails));
    }




    // Class Methods
    private async void ClassEllipsisClicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;
        Class selectedClass = (Class)clickedButton.BindingContext;


        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedClass.ClassName, "Cancel", null, "Edit", "Detailed View", "Set Notification", "Remove From Term");
        Debug.WriteLine("Action: " + action);

        switch (action)
        {
            case "Edit":
                await NavigateToUpdateClass(selectedClass);
                break;
            case "Remove From Term":
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
}