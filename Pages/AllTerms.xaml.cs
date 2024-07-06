using C971_Ogden.ViewModel;
using C971_Ogden.Database;
using System.Diagnostics;

namespace C971_Ogden.Pages;

public partial class AllTerms : ContentPage
{
    public AllTerms(AllTermsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void TermEllipsis_Clicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;
        Term selectedTerm = (Term)clickedButton.BindingContext;
        string action = await DisplayActionSheet(selectedTerm.TermName, "Cancel", null, "Edit", "Delete");
        Debug.WriteLine("Action: " + action);

        var vm = (AllTermsViewModel)BindingContext;

        switch (action)
        {
            case "Edit":
                vm.EditTermCommand.Execute(selectedTerm);
                break;
            case "Delete":
                Debug.WriteLine("Delete this term");
                break;
            default:
                Debug.WriteLine("Something went wrong");
                break;
        }
    }
}