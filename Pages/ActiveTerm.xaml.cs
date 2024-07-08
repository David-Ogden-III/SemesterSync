using C971_Ogden.Database;
using C971_Ogden.ViewModel;
using System.Diagnostics;

namespace C971_Ogden.Pages;

public partial class ActiveTerm : ContentPage
{
    private readonly ActiveTermViewModel _vm;
    public ActiveTerm(ActiveTermViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    private async void Ellipsis_Clicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;
        Class selectedClass = (Class)clickedButton.BindingContext;
        string action = await DisplayActionSheet(selectedClass.ClassName, "Cancel", null, "More Details");
        Debug.WriteLine("Action: " + action);

        var vm = (ActiveTermViewModel)BindingContext;

        switch (action)
        {
            case "More Details":
                Debug.WriteLine("Add Class Details Page!");
                break;
            default:
                Debug.WriteLine("No Action Selected");
                break;
        }
    }
}
