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

            List<TermSchedule> filteredTermSchedules = (await SchoolDatabase.GetFilteredListAsync<TermSchedule>((termSchedule) => termSchedule.TermId == activeTerm.Id)).ToList();
            List<Class> activeClassList = [.. ActiveClasses];

            if (filteredTermSchedules.Count == 0)
            {
                ActiveClasses.Clear();
                return;
            }

            foreach (TermSchedule termSchedule in filteredTermSchedules)
            {
                if (!activeClassList.Exists((c) => c.Id == termSchedule.ClassId))
                {
                    Class activeClass = await SchoolDatabase.GetFilteredItemAsync<Class>((classActive) => classActive.Id == termSchedule.ClassId);
                    ActiveClasses.Add(activeClass);
                }
            }

            if (ActiveClasses.Count != 0)
            {
                foreach (Class c in ActiveClasses.ToList())
                {
                    if (!filteredTermSchedules.Exists((ts) => ts.ClassId == c.Id))
                    {
                        ActiveClasses.Remove(c);
                    }
                }
            }
        }
        else
        {
            ActiveTerm = null;
            ActiveTermIsNotNull = ActiveTerm != null;
            ActiveClasses.Clear();
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
