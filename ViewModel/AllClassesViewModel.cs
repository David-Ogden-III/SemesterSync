using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using SemesterSync.Data;
using SemesterSync.Models;
using SemesterSync.Services;
using SemesterSync.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace SemesterSync.ViewModel;

public class AllClassesViewModel : INotifyPropertyChanged
{
    private readonly IPopupService popupService;
    private string? activeUserEmail;
    public AllClassesViewModel(IPopupService popupService)
    {
        this.popupService = popupService;
        ClassesSourceOfTruth = [];
        LoadCommand = new Command(execute: async () => await LoadClasses());
        DeleteClassCommand = new Command<Class>(execute: async (Class c) => await DeleteClass(c));
        EditClassCommand = new Command<Class>(execute: async (Class c) => await EditClass(c));
        FilterClassesCommand = new Command(execute: () => FilterClasses());
        EllipsisClickedCommand = new Command<Class>(execute: async (Class selectedClass) => await EllipsisClicked(selectedClass));
        activeUserEmail = Task.Run(() => AuthService.RetrieveUserFromSecureStorage()).Result;
    }

    // Collections
    private ObservableCollection<Class> _classes = [];
    public ObservableCollection<Class> Classes
    {
        get => _classes;
        set
        {
            _classes = value;
            OnPropertyChanged(nameof(Classes));
        }
    }
    public List<Class> ClassesSourceOfTruth { get; set; }


    private string _searchParams = "";
    public string SearchParams
    {
        get => _searchParams;
        set
        {
            _searchParams = value;
            OnPropertyChanged(nameof(SearchParams));
        }
    }

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
    public Command FilterClassesCommand { get; }


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
        await DbContext.DeleteItemAsync(selectedClass);

        // Delete related term schedules from TermSchedule table
        TermSchedule termSchedule = await DbContext.GetFilteredItemAsync<TermSchedule>((ts) => ts.ClassId == selectedClass.Id && ts.CreatedBy == activeUserEmail);
        await DbContext.DeleteItemAsync(termSchedule);

        // Delete related exams from Exam table
        IEnumerable<Exam> examsToDelete = await DbContext.GetFilteredListAsync<Exam>(exam => exam.ClassId == selectedClass.Id && exam.CreatedBy == activeUserEmail);
        foreach (Exam exam in examsToDelete)
        {
            await DbContext.DeleteItemAsync(exam);
        }

        // Remove from Classes list (UI)
        Classes.Remove(selectedClass);
        ClassesSourceOfTruth.Remove(selectedClass);
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
        await Task.Delay(50);
        LoadingPopup loadingPopup = new();
        Shell.Current.CurrentPage.ShowPopup(loadingPopup);
        Classes.Clear();
        ClassesSourceOfTruth.Clear();

        List<Class> dbClasses = (await DbContext.GetFilteredListAsync<Class>(c => c.CreatedBy == activeUserEmail)).ToList();

        foreach (Class dbClass in dbClasses)
        {
            Classes.Add(dbClass);
            ClassesSourceOfTruth.Add(dbClass);
        }


        await Task.Delay(250);
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
    private void FilterClasses()
    {
        if (String.IsNullOrWhiteSpace(SearchParams))
        {
            Classes = ClassesSourceOfTruth.ToObservableCollection<Class>();
        }
        else
        {
            string param = SearchParams.ToLower().Trim();
            var result = Classes.Where(c => c.ClassName.ToLower().Trim().Contains(param));
            Classes = result.ToObservableCollection<Class>();
        }
    }



    // Other
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
