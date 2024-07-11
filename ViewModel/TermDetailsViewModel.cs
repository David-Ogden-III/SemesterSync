using System.ComponentModel;
using System.Diagnostics;
using C971_Ogden.Database;

namespace C971_Ogden.ViewModel;
[QueryProperty(nameof(SelectedCG), "SelectedCG")]
public class TermDetailsViewModel : INotifyPropertyChanged
{
    public TermDetailsViewModel()
    {
        LoadCommand = new Command(execute: async () => await Load());
        SelectionChangedCommand = new Command(execute: () => SelectionChanged());
        RemoveClassCommand = new Command<Class>(execute: (Class classToDelete) => RemoveClass(classToDelete));
        SaveCommand = new Command(execute: async () => await Save());
    }

    // Collections
    private List<Class> ClassesInDB { get; set; } = [];
    private IEnumerable<Class> _allClasses = [];
    public IEnumerable<Class> AllClasses
    {
        get => _allClasses;
        set
        {
            _allClasses = value;
            OnPropertyChanged(nameof(AllClasses));
        }
    }
    


    // Objects
    ClassGroup? _selectedCG;
    public ClassGroup? SelectedCG
    {
        get => _selectedCG;
        set
        {
            _selectedCG = value;
            OnPropertyChanged(nameof(SelectedCG));
        }
    }

    private Class? _selectedClass;
    public Class? SelectedClass
    {
        get => _selectedClass;
        set
        {
            _selectedClass = value;
            OnPropertyChanged(nameof(SelectedClass));
        }
    }

    private string _termName = "";
    public string TermName
    {
        get => _termName;
        set
        {
            _termName = value;
            OnPropertyChanged(nameof(TermName));
        }
    }

    private DateTime _startDate;
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));
        }
    }

    private DateTime _endDate;
    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
        }
    }


    // Commands
    public Command LoadCommand { get; }
    public Command SelectionChangedCommand { get; }
    public Command<Class> RemoveClassCommand { get; }
    public Command SaveCommand { get; }
    public Command CancelClickedCommand { get; set; } = new(
        execute: async () =>
        {
            Debug.WriteLine("No changes saved");
            await Shell.Current.GoToAsync("..");
        });


    // Command Definitions
    private async Task Load()
    {
        var allClasses = await SchoolDatabase.GetAllAsync<Class>();
        AllClasses = allClasses.AsEnumerable();
        if (SelectedCG != null)
        {
            Debug.WriteLine($"Term ID: {SelectedCG.Term.Id}");
            ClassesInDB = [.. SelectedCG];
            TermName = SelectedCG.Term.TermName;
            StartDate = SelectedCG.Term.StartDate;
            EndDate = SelectedCG.Term.EndDate;
        }
        else
        {
            SelectedCG = new ClassGroup(new Term(), []);
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);
        }
    }

    private void SelectionChanged()
    {

        if (SelectedCG != null && SelectedClass != null)
        {
            List<Class> allClasses = [.. SelectedCG];
            bool classalreadyExists = allClasses.Exists(c => c.Id == SelectedClass.Id);
            if (!classalreadyExists)
                SelectedCG.Add(SelectedClass);
        }
    }
    private void RemoveClass(Class classToDelete)
    {
        SelectedCG?.Remove(classToDelete);
    }

    private async Task Save()
    {
        if (SelectedCG?.Term.Id == 0) // 0 is default for unassigned int. AKA Add Term clicked, not edit term clicked on last page
        {
            SelectedCG.Term.TermName = TermName;
            SelectedCG.Term.StartDate = StartDate;
            SelectedCG.Term.EndDate = EndDate;
            bool termInserted = await SchoolDatabase.AddItemAsync<Term>(SelectedCG.Term);
            Debug.WriteLine($"Term Inserted: {termInserted} Term ID: {SelectedCG.Term.Id}");

            if (SelectedCG.Count > 0)
            {
                List<TermSchedule> newSchedules = [];

                foreach (Class c in SelectedCG)
                {
                    TermSchedule termSchedule = new()
                    {
                        ClassId = c.Id,
                        TermId = SelectedCG.Term.Id
                    };
                    newSchedules.Add(termSchedule);
                }

                int numClassesAdded = await SchoolDatabase.AddAllItemsAsync<TermSchedule>(newSchedules);
                Debug.WriteLine($"{SelectedCG.Term.TermName}: {numClassesAdded} classes added");
            }
        }
        else
        {
            // Update Term Table
            if (SelectedCG.Term.TermName != TermName || SelectedCG.Term.StartDate != StartDate || SelectedCG.Term.EndDate != EndDate)
            {
                SelectedCG.Term.TermName = TermName;
                SelectedCG.Term.StartDate = StartDate;
                SelectedCG.Term.EndDate = EndDate;

                bool termUpdated = await SchoolDatabase.UpdateItemAsync(SelectedCG.Term);
                Debug.WriteLine($"Term Updated: {termUpdated}");
            }


            // Update TermSchedule Table
                // Only add classes to DB if they dont already exist
            List<TermSchedule> newSchedules = [];
            foreach (Class c in SelectedCG)
            {
                if (!ClassesInDB.Exists(dbClass => dbClass.Id == c.Id))
                {
                    TermSchedule termSchedule = new()
                    {
                        ClassId = c.Id,
                        TermId = SelectedCG.Term.Id
                    };
                    newSchedules.Add(termSchedule);
                }
            }
            int numClassesAdded = await SchoolDatabase.AddAllItemsAsync<TermSchedule>(newSchedules);
            Debug.WriteLine($"{SelectedCG.Term.TermName}: {numClassesAdded} classes added");

                // Remove Classes from DB if they were no longer selected
            List<Class> SelectedCGClasses = [.. SelectedCG];
            foreach (Class c in ClassesInDB)
            {
                if (!SelectedCGClasses.Exists(selectedC => selectedC.Id == c.Id))
                {
                    TermSchedule tSToDelete = await SchoolDatabase.GetFilteredItemAsync<TermSchedule>(ts => ts.ClassId == c.Id);
                    bool tsDeleted = await SchoolDatabase.DeleteItemAsync(tSToDelete);
                    Debug.WriteLine($"Removed Class {c.ClassName} from Table {SelectedCG.Term.TermName}");
                }
            }
        }







        await Shell.Current.GoToAsync("..");
    }

    

    



    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
