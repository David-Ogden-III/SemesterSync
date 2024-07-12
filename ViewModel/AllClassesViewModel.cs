using C971_Ogden.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using C971_Ogden.Database;
using System.Diagnostics;

namespace C971_Ogden.ViewModel;

public class AllClassesViewModel : INotifyPropertyChanged
{
    public AllClassesViewModel()
    {
        Classes = [];
        LoadCommand = new Command(execute: async () => await LoadClasses());
        DeleteClassCommand = new Command<Class>(execute: async (Class c) => await DeleteClass(c));
        EllipsisClickedCommand = new Command<Class>(execute: async (Class selectedClass) => await EllipsisClicked(selectedClass));
    }

    // Collections
    public ObservableCollection<Class> Classes { get; set; }


    // Commands
    public Command LoadCommand { get; }
    public Command DeleteClassCommand { get; }
    public Command EllipsisClickedCommand { get; }
    public Command AddClassCommand { get; set; } = new(
        execute: async () =>
        {
            await Shell.Current.GoToAsync(nameof(UpdateClass));
        });

    public Command<Class> EditClassCommand { get; set; } = new(
        execute: async (Class selectedClass) =>
        {
            await Shell.Current.GoToAsync(nameof(UpdateClass),
                new Dictionary<string, object>
                {
                {"SelectedClass", selectedClass }
                });
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
        bool itemDeleted = await SchoolDatabase.DeleteItemAsync(selectedClass);


        TermSchedule termSchedule = (await SchoolDatabase.GetFilteredItemAsync<TermSchedule>((ts) => ts.ClassId == selectedClass.Id));
        bool termScheduleDeleted = await SchoolDatabase.DeleteItemAsync(termSchedule);
        Debug.WriteLineIf(termScheduleDeleted, $"Deleted Class: {selectedClass.ClassName}");

        if (itemDeleted)
            Classes.Remove(selectedClass);
    }


    private async Task LoadClasses()
    {
        List<Class> dbClasses = await SchoolDatabase.GetAllAsync<Class>();
        List<Class> existingClasses = [.. Classes];
        foreach (Class dbClass in dbClasses)
        {
            if (!existingClasses.Exists(existingClass => existingClass.Id == dbClass.Id))
            {
                Classes.Add(dbClass);
            }
        }

        foreach (Class existingClass in existingClasses)
        {
            if (!dbClasses.Exists((dbClass) => dbClass.Id == existingClass.Id))
            {
                Classes.Remove(existingClass);
            }
        }
    }

    private async Task EllipsisClicked(Class selectedClass)
    {
        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedClass.ClassName, "Cancel", null, "Edit", "Detailed View", "Delete");
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
            default:
                Debug.WriteLine("No Action Selected");
                break;
        }
    }




    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
