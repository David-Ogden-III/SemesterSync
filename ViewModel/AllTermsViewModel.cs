using C971_Ogden.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using C971_Ogden.Database;
using System.Diagnostics;

namespace C971_Ogden.ViewModel;

public class AllTermsViewModel : INotifyPropertyChanged
{
    public AllTermsViewModel()
    {
        Classes = [];
        LoadCommand = new Command(execute: async () => await LoadClasses());
        DeleteTermCommand = new Command<ClassGroup>(execute: async (ClassGroup cg) => await DeleteTerm(cg));
    }

    public ObservableCollection<ClassGroup> Classes { get; set; }
    public Command LoadCommand { get; }
    public Command DeleteTermCommand { get; }

    public Command AddTermCommand { get; set; } = new(
        execute: async () =>
    {
        await Shell.Current.GoToAsync(nameof(TermDetails));
    });

    public Command<ClassGroup> EditTermCommand { get; set; } = new(
        execute: async (ClassGroup selectedCG) =>
    {
        await Shell.Current.GoToAsync(nameof(TermDetails),
            new Dictionary<string, object>
            {
                {"SelectedCG", selectedCG }
            });
    });

    private async Task DeleteTerm(ClassGroup selectedCG)
    {
            Term selectedTerm = selectedCG.Term;
            bool itemDeleted = await SchoolDatabase.DeleteItemAsync(selectedTerm);

        foreach (Class classToDelete in selectedCG)
        {
            string className = classToDelete.ClassName;
            bool classDeleted = await SchoolDatabase.DeleteItemAsync(classToDelete);
            Debug.WriteLineIf(classDeleted, $"Deleted Class: {className}");
        }

        if (itemDeleted)
            Classes.Remove(selectedCG);
    }


    private async Task LoadClasses()
    {
        Classes.Clear();
        List<Term> allTermResults = (await SchoolDatabase.GetAllAsync<Term>());
        ObservableCollection<ClassGroup> cg = [];

        foreach (Term term in allTermResults)
        {            
            List<TermSchedule> termSchedules = (await SchoolDatabase.GetFilteredListAsync<TermSchedule>((ts) => ts.TermId ==  term.Id)).ToList();

            ObservableCollection<Class> classes = [];

            foreach (TermSchedule termSchedule in termSchedules)
            {
                Class selectedClass = await SchoolDatabase.GetFilteredItemAsync<Class>((c) => c.Id == termSchedule.ClassId);
                classes.Add(selectedClass);
            }

            Classes.Add(new ClassGroup (term, classes));
        }

    }


    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}