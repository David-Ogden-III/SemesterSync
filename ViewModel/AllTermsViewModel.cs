using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using SemesterSync.Data;
using SemesterSync.Models;
using SemesterSync.Services;
using SemesterSync.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace SemesterSync.ViewModel;

public class AllTermsViewModel : INotifyPropertyChanged
{
    private readonly IPopupService popupService;
    private string? activeUserEmail;
    public AllTermsViewModel(IPopupService popupService)
    {
        this.popupService = popupService;
        LoadCommand = new Command(execute: async () => await LoadClasses());
        DeleteTermCommand = new Command<ClassGroup>(execute: async (ClassGroup cg) => await DeleteTerm(cg));
        EditClassCommand = new Command<Class>(execute: async (Class c) => await EditClass(c));
        DeleteClassCommand = new Command<Class>(execute: async (Class c) => await DeleteClass(c));
        FilterTermsCommand = new Command(execute: () => FilterTerms());
        TermEllipsisClickedCommand = new Command<ClassGroup>(execute: async (ClassGroup selectedCG) => await TermEllipsisClicked(selectedCG));
        ClassEllipsisClickedCommand = new Command<Class>(execute: async (Class selectedClass) => await ClassEllipsisClicked(selectedClass));
        activeUserEmail = Task.Run(() => AuthService.RetrieveUserFromSecureStorage()).Result;
    }


    // Collections
    private ObservableCollection<ClassGroup> _classes = [];
    public ObservableCollection<ClassGroup> Classes
    {
        get => _classes;
        set
        {
            _classes = value;
            OnPropertyChanged(nameof(Classes));
        }
    }
    public List<ClassGroup> ClassesSourceOfTruth { get; set; } = [];


    private string _searchParams = "";
    public string SearchParams
    {
        get => _searchParams;
        set
        {
            _searchParams = value;
            OnPropertyChanged(nameof(SearchParams));
        }
    }

    // Commands
    public Command LoadCommand { get; }
    public Command DeleteTermCommand { get; }
    public Command DeleteClassCommand { get; }

    public Command TermEllipsisClickedCommand { get; }
    public Command ClassEllipsisClickedCommand { get; }
    public Command FilterTermsCommand { get; }

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
        bool itemDeleted = await DbContext.DeleteItemAsync(selectedTerm);

        // Delete TermSchedules associated to terms in TermSchedules table
        foreach (Class classToDelete in selectedCG)
        {
            string className = classToDelete.ClassName;
            TermSchedule termSchedule = (await DbContext.GetFilteredItemAsync<TermSchedule>((ts) => ts.ClassId == classToDelete.Id && ts.CreatedBy == activeUserEmail));
            bool classDeleted = await DbContext.DeleteItemAsync(termSchedule);
            Debug.WriteLineIf(classDeleted, $"Deleted Class: {className}");
        }

        if (itemDeleted)
        {
            Classes.Remove(selectedCG);

            ClassGroup? cgToDelete = ClassesSourceOfTruth.Find(cg => cg.Term.Id == selectedCG.Term.Id);
            if (cgToDelete != null)
            {
                ClassesSourceOfTruth.Remove(cgToDelete);
            }
        }
    }

    private async Task LoadClasses()
    {
        await Task.Delay(50);
        LoadingPopup loadingPopup = new();
        Shell.Current.CurrentPage.ShowPopup(loadingPopup);
        Classes.Clear();
        ClassesSourceOfTruth.Clear();
        List<Term> allTermResults = (await DbContext.GetFilteredListAsync<Term>(term => term.CreatedBy == activeUserEmail)).ToList();
        ObservableCollection<ClassGroup> cg = [];

        foreach (Term term in allTermResults)
        {
            List<TermSchedule> termSchedules = (await DbContext.GetFilteredListAsync<TermSchedule>((ts) => ts.TermId == term.Id && ts.CreatedBy == activeUserEmail)).ToList();

            ObservableCollection<Class> classes = [];

            foreach (TermSchedule termSchedule in termSchedules)
            {
                Class selectedClass = await DbContext.GetFilteredItemAsync<Class>((c) => c.Id == termSchedule.ClassId && c.CreatedBy == activeUserEmail);
                classes.Add(selectedClass);
            }

            Classes.Add(new ClassGroup(term, classes));
            ClassesSourceOfTruth.Add(new ClassGroup(term, classes));
        }
        await Task.Delay(250);
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

    private async Task DeleteClass(Class selectedClass)
    {

        foreach (ClassGroup cg in Classes)
        {
            Class? foundClass = cg.ToList().Find(c => c.Id == selectedClass.Id);
            if (foundClass != null)
            {
                TermSchedule termSchedule = await DbContext.GetFilteredItemAsync<TermSchedule>(ts => ts.ClassId == foundClass.Id && ts.CreatedBy == activeUserEmail);
                await DbContext.DeleteItemAsync(termSchedule);
                cg.Remove(foundClass);
                break;
            }
        }
        foreach (ClassGroup cg in ClassesSourceOfTruth)
        {
            Class? foundClass = cg.ToList().Find(c => c.Id == selectedClass.Id);
            if (foundClass != null)
            {
                cg.Remove(foundClass);
                break;
            }
        }
    }

    private void FilterTerms()
    {
        if (String.IsNullOrWhiteSpace(SearchParams))
        {
            Classes = ClassesSourceOfTruth.ToObservableCollection<ClassGroup>();
        }
        else
        {
            string param = SearchParams.ToLower().Trim();
            var result = Classes.Where(c => c.Term.TermName.ToLower().Trim().Contains(param) || c.ToList().Exists(cl => cl.ClassName.ToLower().Trim().Contains(param)));
            Classes = result.ToObservableCollection<ClassGroup>();
        }
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
                DetailedClassCommand.Execute(selectedClass);
                break;
            case "Set Notification":
                popupService.ShowPopup<NotificationPopupViewModel>(viewmodel => viewmodel.OnAppearing(selectedClass));
                break;
            default:
                Debug.WriteLine("No Action Selected");
                break;
        }
    }



    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}