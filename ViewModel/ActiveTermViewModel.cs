using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using C971_Ogden.Database;

namespace C971_Ogden.ViewModel;

public class ActiveTermViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Class> ActiveClasses { get; set; }
    public Command LoadCommand { get; }

    public ActiveTermViewModel()
    {
        ActiveClasses = [];
        LoadCommand = new Command(execute: async () => await LoadActiveTermAsync());
        ActiveTerm = null;
        ActiveTermIsNotNull = false;
    }

    // Fields and Properties
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


    public async Task LoadActiveTermAsync()
    {
        var activeTerm = await SchoolDatabase.GetFilteredItemAsync<Term>((term) => term.StartDate < DateTime.Now && term.EndDate > DateTime.Now);

        if (activeTerm != null)
        {
            ActiveTerm = activeTerm;
            ActiveTermIsNotNull = ActiveTerm != null;

            IEnumerable<TermSchedule> iFilteredTermSchedules = await SchoolDatabase.GetFileteredListAsync<TermSchedule>((termSchedule) => termSchedule.TermId == activeTerm.Id);

            List<Class> activeClassList = [.. ActiveClasses];
            foreach (TermSchedule termSchedule in iFilteredTermSchedules)
            {
                Class activeClass = await SchoolDatabase.GetFilteredItemAsync<Class>((classActive) => classActive.Id == termSchedule.ClassId);
                if (activeClass != null && !activeClassList.Exists((c) => c.Id == activeClass.Id))
                    ActiveClasses.Add(activeClass);
            }

            List<TermSchedule> filteredTSList = [.. iFilteredTermSchedules];
            foreach (Class c in ActiveClasses)
            {
                if (!filteredTSList.Exists((ts) => ts.ClassId == c.Id))
                {
                    int indexToRemove = activeClassList.FindIndex((classToRemove) => classToRemove.Id == c.Id);
                    ActiveClasses.RemoveAt(indexToRemove);
                }
            }
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
