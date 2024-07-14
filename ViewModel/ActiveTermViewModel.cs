using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using C971_Ogden.Database;
using C971_Ogden.Pages;

namespace C971_Ogden.ViewModel;

public class ActiveTermViewModel : INotifyPropertyChanged
{
    public ActiveTermViewModel()
    {
        ActiveTerm = null;
        ActiveTermIsNotNull = false;

        LoadCommand = new Command(execute: async () => await LoadActiveTermAsync());
        EllipsisClickedCommand = new Command<Class>(execute: async (Class selectedClass) => await EllipsisClicked(selectedClass));
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


    // Command Definitions
    public async Task LoadActiveTermAsync()
    {
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
    }

    private async Task EllipsisClicked(Class selectedClass)
    {
        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedClass.ClassName, "Cancel", null, "Detailed View");
        Debug.WriteLine("Action: " + action);

        switch (action)
        {
            case "Detailed View":
                DetailedViewCommand.Execute(selectedClass);
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
