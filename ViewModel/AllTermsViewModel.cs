using C971_Ogden.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using C971_Ogden.Database;
using System.Diagnostics;
using CommunityToolkit.Maui.Views;

namespace C971_Ogden.ViewModel;

public class AllTermsViewModel : INotifyPropertyChanged
{
    public AllTermsViewModel()
    {
        LoadCommand = new Command(execute: async () => await LoadClasses());
        DeleteTermCommand = new Command<ClassGroup>(execute: async (ClassGroup cg) => await DeleteTerm(cg));
        EditClassCommand = new Command<Class>(execute: async (Class c) => await EditClass(c));
        TermEllipsisClickedCommand = new Command<ClassGroup>(execute: async (ClassGroup selectedCG) => await TermEllipsisClicked(selectedCG));
        ClassEllipsisClickedCommand = new Command<Class>(execute: async (Class selectedClass) => await ClassEllipsisClicked(selectedClass));
    }
    

    // Collections
    public ObservableCollection<ClassGroup> Classes { get; set; } = [];


    // Commands
    public Command LoadCommand { get; }
    public Command DeleteTermCommand { get; }

    public Command TermEllipsisClickedCommand { get; }
    public Command ClassEllipsisClickedCommand { get; }

    public Command AddTermCommand { get; set; } = new(
        execute: async () =>
    {
        await Shell.Current.GoToAsync(nameof(TermDetails));
    });

    public Command<Class> EditClassCommand { get; }

    public Command<Class> DetailedClassCommand { get; set; } = new(
        execute: async (Class selectedClass) =>
        {
            await Shell.Current.GoToAsync(nameof(DetailedClass),
                new Dictionary<string, object>
                {
                {"SelectedClass", selectedClass }
                });
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


    // Command Definitions
    private async Task DeleteTerm(ClassGroup selectedCG)
    {
        // Delete Term from Terms table
            Term selectedTerm = selectedCG.Term;
            bool itemDeleted = await SchoolDatabase.DeleteItemAsync(selectedTerm);

        // Delete TermSchedules associated to terms in TermSchedules table
        foreach (Class classToDelete in selectedCG)
        {
            string className = classToDelete.ClassName;
            TermSchedule termSchedule = (await SchoolDatabase.GetFilteredItemAsync<TermSchedule>((ts) => ts.ClassId == classToDelete.Id));
            bool classDeleted = await SchoolDatabase.DeleteItemAsync(termSchedule);
            Debug.WriteLineIf(classDeleted, $"Deleted Class: {className}");
        }

        if (itemDeleted)
            Classes.Remove(selectedCG);
    }

    private async Task LoadClasses()
    {
        LoadingPopup loadingPopup = new();
        Shell.Current.CurrentPage.ShowPopup(loadingPopup);
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
        await Task.Delay(1000);
        loadingPopup.Close();
    }

    private async Task EditClass(Class selectedClass)
    {
        await Shell.Current.GoToAsync(nameof(UpdateClass),
            new Dictionary<string, object>
            {
                {"SelectedClass", selectedClass }
            });
    }

    private async Task TermEllipsisClicked(ClassGroup selectedCG)
    {
        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedCG.Term.TermName, "Cancel", null, "Edit", "Delete");
        Debug.WriteLine("Action: " + action);

        switch (action)
        {
            case "Edit":
                EditTermCommand.Execute(selectedCG);
                break;
            case "Delete":
                DeleteTermCommand.Execute(selectedCG);
                break;
            default:
                Debug.WriteLine("No Action Selected");
                break;
        }
    }

    private async Task ClassEllipsisClicked(Class selectedClass)
    {
        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedClass.ClassName, "Cancel", null, "Edit", "Detailed View");
        Debug.WriteLine("Action: " + action);

        switch (action)
        {
            case "Edit":
                EditClassCommand.Execute(selectedClass);
                break;
            case "Detailed View":
                DetailedClassCommand.Execute(selectedClass);
                break;
            default:
                Debug.WriteLine("No Action Selected");
                break;
        }
    }



    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}