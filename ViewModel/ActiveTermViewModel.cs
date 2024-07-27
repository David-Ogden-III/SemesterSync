using C971_Ogden.Database;
using C971_Ogden.Views;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace C971_Ogden.ViewModel;

public class ActiveTermViewModel : INotifyPropertyChanged
{
    private readonly IPopupService popupService;
    public ActiveTermViewModel(IPopupService popupService)
    {
        this.popupService = popupService;
        ActiveTerm = null;
        ActiveTermIsNotNull = false;

        LoadCommand = new Command(execute: async () => await LoadActiveTermAsync());
        DeleteClassCommand = new Command<Class>(execute: async (Class c) => await DeleteClass(c));
        EllipsisClickedCommand = new Command<Class>(execute: async (Class selectedClass) => await EllipsisClicked(selectedClass));
        EditClassCommand = new Command<Class>(execute: async (Class c) => await EditClass(c));
    }

    // Collections
    public ObservableCollection<Class> ActiveClasses { get; set; } = [];
    public Command LoadCommand { get; }


    // Objects
    private Term? _activeTerm;
    public Term? ActiveTerm
    {
        get => _activeTerm;
        set
        {
            _activeTerm = value;
            OnPropertyChanged(nameof(ActiveTerm));
        }
    }

    private bool _activeTermIsNotNull;
    public bool ActiveTermIsNotNull
    {
        get => _activeTermIsNotNull;
        set
        {
            _activeTermIsNotNull = value;
            OnPropertyChanged(nameof(ActiveTermIsNotNull));
        }
    }


    // Commands
    public Command EllipsisClickedCommand { get; }

    public Command<Class> DetailedViewCommand { get; set; } = new(
        execute: async (Class selectedClass) =>
        {
            await Shell.Current.GoToAsync(nameof(DetailedClass),
                new Dictionary<string, object>
                {
                {"SelectedClass", selectedClass }
                });
        });
    public Command<Class> EditClassCommand { get; }
    public Command DeleteClassCommand { get; }


    // Command Definitions
    public async Task LoadActiveTermAsync()
    {
        
        await Task.Delay(50);
        LoadingPopup loadingPopup = new();
        Shell.Current.CurrentPage.ShowPopup(loadingPopup);
        ActiveClasses.Clear();
        ActiveTerm = await SchoolDatabase.GetFilteredItemAsync<Term>((term) => term.StartDate < DateTime.Now && term.EndDate > DateTime.Now);

        ActiveTermIsNotNull = ActiveTerm != null;

        if (ActiveTermIsNotNull)
        {
            IEnumerable<TermSchedule> filteredTermSchedules = await SchoolDatabase.GetFilteredListAsync<TermSchedule>((termSchedule) => termSchedule.TermId == ActiveTerm.Id);
            foreach (TermSchedule termSchedule in filteredTermSchedules)
            {
                Class activeClass = await SchoolDatabase.GetFilteredItemAsync<Class>((classActive) => classActive.Id == termSchedule.ClassId);
                ActiveClasses.Add(activeClass);
            }
        }
        await Task.Delay(250);
        loadingPopup.Close();
    }

    private async Task DeleteClass(Class selectedClass)
    {
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
        ActiveClasses.Remove(selectedClass);
    }

    private async Task EditClass(Class selectedClass)
    {
        await Shell.Current.GoToAsync(nameof(UpdateClass),
                new Dictionary<string, object>
                {
                {"SelectedClass", selectedClass }
                });
    }

    private async Task EllipsisClicked(Class selectedClass)
    {
        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedClass.ClassName, "Cancel", null, "Edit", "Detailed View", "Set Notification", "Remove From Term");
        Debug.WriteLine("Action: " + action);

        switch (action)
        {
            case "Edit":
                EditClassCommand.Execute(selectedClass);
                break;
            case "Remove From Term":
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


    // Helpers
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
