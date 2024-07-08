using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using C971_Ogden.Database;

namespace C971_Ogden.ViewModel;

public class ActiveTermViewModel(SchoolDatabase db) : INotifyPropertyChanged
{
    private readonly SchoolDatabase _db = db;

    // Fields and Properties
    private Term? _activeTerm;
    public Term ActiveTerm
    {
        get => _activeTerm;
        set
        {
            _activeTerm = value;
            OnPropertyChanged(nameof(ActiveTerm));
        }
    }

    private ObservableCollection<Class> _activeClasses = [];
    public ObservableCollection<Class> ActiveClasses
    {
        get => _activeClasses;
        set
        {
            _activeClasses = value;
            OnPropertyChanged(nameof(ActiveClasses));
        }
    }

    private bool _isInitialStartup = true;
    public bool IsInitialStartup
    {
        get => _isInitialStartup;
        set
        {
            _isInitialStartup = value;
            OnPropertyChanged(nameof(IsInitialStartup));
        }
    }

    private bool _activeClassesHasRows;
    public bool ActiveClassesHasRows
    {
        get => _activeClassesHasRows;
        set
        {
            _activeClassesHasRows = value;
            OnPropertyChanged(nameof(ActiveClassesHasRows));
        }
    }


    public async Task LoadActiveTermAsync()
    {
        IsInitialStartup = false;

        var activeTerm = await _db.GetFilteredItemAsync<Term>((term) => term.StartDate < DateTime.Now && term.EndDate > DateTime.Now);

        if (activeTerm != null)
        {
            ActiveTerm ??= activeTerm;

            IEnumerable<TermSchedule> iFilteredTermSchedules = await _db.GetFileteredListAsync<TermSchedule>((termSchedule) => termSchedule.TermId == activeTerm.Id);

            List<Class> activeClassList = [.. ActiveClasses];
            foreach (TermSchedule termSchedule in iFilteredTermSchedules)
            {
                Class activeClass = await _db.GetFilteredItemAsync<Class>((classActive) => classActive.Id == termSchedule.ClassId);
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
