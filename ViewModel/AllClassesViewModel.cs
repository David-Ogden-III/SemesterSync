using C971_Ogden.Database;
using C971_Ogden.Pages;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace C971_Ogden.ViewModel;

public class AllClassesViewModel : INotifyPropertyChanged
{
    private readonly IPopupService popupService;
    public AllClassesViewModel(IPopupService popupService)
    {
        this.popupService = popupService;
        Classes = [];
        LoadCommand = new Command(execute: async () => await LoadClasses());
        DeleteClassCommand = new Command<Class>(execute: async (Class c) => await DeleteClass(c));
        EditClassCommand = new Command<Class>(execute: async (Class c) => await EditClass(c));
        EllipsisClickedCommand = new Command<Class>(execute: async (Class selectedClass) => await EllipsisClicked(selectedClass));
    }

    // Collections
    public ObservableCollection<Class> Classes { get; set; }


    // Commands
    public Command LoadCommand { get; }
    public Command DeleteClassCommand { get; }
    public Command EllipsisClickedCommand { get; }
    public Command<Class> EditClassCommand { get; }
    public Command AddClassCommand { get; set; } = new(
        execute: async () =>
        {
            await Shell.Current.GoToAsync(nameof(UpdateClass));
        });


    public Command<Class> DetailedViewCommand { get; set; } = new(
        execute: async (Class selectedClass) =>
        {
            await Shell.Current.GoToAsync(nameof(DetailedClass),
                new Dictionary<string, object>
                {
                {"SelectedClass", selectedClass }
                });
        });


    // Command Definitions
    private async Task DeleteClass(Class selectedClass)
    {
        // Delete class from Class table
        await SchoolDatabase.DeleteItemAsync(selectedClass);

        // Delete related term schedules from TermSchedule table
        TermSchedule termSchedule = await SchoolDatabase.GetFilteredItemAsync<TermSchedule>((ts) => ts.ClassId == selectedClass.Id);
        await SchoolDatabase.DeleteItemAsync(termSchedule);

        // Delete related exams from Exam table
        IEnumerable<Exam> examsToDelete = await SchoolDatabase.GetFilteredListAsync<Exam>(exam => exam.ClassId == selectedClass.Id);
        foreach (Exam exam in examsToDelete)
        {
            await SchoolDatabase.DeleteItemAsync(exam);
        }

        // Remove from Classes list (UI)
        Classes.Remove(selectedClass);
    }

    private async Task EditClass(Class selectedClass)
    {
        await Shell.Current.GoToAsync(nameof(UpdateClass),
                new Dictionary<string, object>
                {
                {"SelectedClass", selectedClass }
                });
    }

    private async Task LoadClasses()
    {
        LoadingPopup loadingPopup = new();
        Shell.Current.CurrentPage.ShowPopup(loadingPopup);
        Classes.Clear();

        List<Class> dbClasses = await SchoolDatabase.GetAllAsync<Class>();

        foreach (Class dbClass in dbClasses)
            Classes.Add(dbClass);

        await Task.Delay(1000);
        loadingPopup.Close();
    }

    private async Task EllipsisClicked(Class selectedClass)
    {
        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedClass.ClassName, "Cancel", null, "Edit", "Detailed View", "Set Notification", "Delete");
        Debug.WriteLine("Action: " + action);

        switch (action)
        {
            case "Edit":
                EditClassCommand.Execute(selectedClass);
                break;
            case "Delete":
                DeleteClassCommand.Execute(selectedClass);
                break;
            case "Detailed View":
                DetailedViewCommand.Execute(selectedClass);
                break;
            case "Set Notification":
                popupService.ShowPopup<NotificationPopupViewModel>(viewmodel => viewmodel.OnAppearing(selectedClass));
                break;
            default:
                Debug.WriteLine("No Action Selected");
                break;
        }
    }



    // Other
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
