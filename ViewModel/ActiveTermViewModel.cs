using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using C971_Ogden.Database;

namespace C971_Ogden.ViewModel;

public class ActiveTermViewModel : INotifyPropertyChanged
{
    private readonly SchoolDatabase _db;

    public ActiveTermViewModel(SchoolDatabase db)
    {
        _db = db;
    }

    // Fields and Properties
    private Term _activeTerm;
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

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged(nameof(IsBusy));
        }
    }

    private string _busyText;
    public string BusyText
    {
        get => _busyText;
        set
        {
            _busyText = value;
            OnPropertyChanged(nameof(BusyText));
        }
    }


    public async Task LoadActiveTermAsync()
    {
        await ExecuteAsync(async () =>
        {
            var activeTerm = await _db.GetFileteredItemAsync<Term>((term) => term.StartDate < DateTime.Now && term.EndDate > DateTime.Now);

            if (activeTerm != null)
            {
                ActiveTerm ??= activeTerm;

                IEnumerable<TermSchedule> iFilteredTermSchedules = await _db.GetFileteredListAsync<TermSchedule>((termSchedule) => termSchedule.TermId == activeTerm.Id);

                foreach (TermSchedule termSchedule in iFilteredTermSchedules)
                {
                    Class activeClass = await _db.GetFileteredItemAsync<Class>((classActive) => classActive.Id == termSchedule.ClassId);
                    ActiveClasses.Add(activeClass);
                }
            }
        }, "Fetching Active Term...");
    }

    private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
    {
        IsBusy = true;
        BusyText = busyText ?? "Processing...";
        try
        {
            await operation?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            IsBusy = false;
            BusyText = "Processing...";
        }
    }



    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
