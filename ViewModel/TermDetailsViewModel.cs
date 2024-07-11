using System.ComponentModel;
using System.Diagnostics;
using C971_Ogden.Database;

namespace C971_Ogden.ViewModel;
[QueryProperty(nameof(SelectedCG), "SelectedCG")]
public class TermDetailsViewModel : INotifyPropertyChanged
{
    public TermDetailsViewModel()
    {
        ClassesInDB = [];
        AllClasses = [];
        RemoveClassCommand = new Command<Class>(execute: (Class classToDelete) => RemoveClass(classToDelete));
        LoadCommand = new Command(execute: async () => await Load());
        PickerIndexChangedCommand = new Command(execute: () => SelectionChanged());
    }

    // Collections
    private List<Class> ClassesInDB { get; set; }
    private IEnumerable<Class> _allClasses;
    public IEnumerable<Class> AllClasses
    {
        get => _allClasses;
        set
        {
            _allClasses = value;
            OnPropertyChanged(nameof(AllClasses));
        }
    }
    public Command<Class> RemoveClassCommand { get; }
    public Command LoadCommand { get; }
    public Command PickerIndexChangedCommand { get; }


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

    private Class _selectedClass;
    public Class SelectedClass
    {
        get => _selectedClass;
        set
        {
            _selectedClass = value;
            OnPropertyChanged(nameof(SelectedClass));
        }
    }


    // Commands
    public Command CancelClickedCommand { get; set; } = new(
        execute: async () =>
        {
            Debug.WriteLine("I was clicked");
            await Shell.Current.GoToAsync("..");
        });

    public Command SaveClickedCommand { get; set; } = new(
        execute: async () =>
        {
            Debug.WriteLine("I was clicked");
            await Shell.Current.GoToAsync("..");
        });

    
    // Command Definitions
    private void RemoveClass(Class classToDelete)
    {
        SelectedCG?.Remove(classToDelete);
    }

    private async Task Load()
    {
        var allClasses = await SchoolDatabase.GetAllAsync<Class>();
        AllClasses = allClasses.AsEnumerable();
        if (SelectedCG != null)
        {
            ClassesInDB = [.. SelectedCG];
        }

            
        else SelectedCG = new ClassGroup(new Term(), []);
    }

    public void SelectionChanged()
    {

        if (SelectedCG != null)
        {
            List<Class> allClasses = [.. SelectedCG];
            bool classalreadyExists = allClasses.Exists(c => c.Id == SelectedClass.Id);
            if (!classalreadyExists)
                SelectedCG.Add(SelectedClass);
        }
    }



    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
