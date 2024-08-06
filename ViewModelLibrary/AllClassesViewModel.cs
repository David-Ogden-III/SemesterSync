using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using DataLibrary;
using ModelLibrary;
using ServiceLibrary;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModelLibrary;

public class AllClassesViewModel : INotifyPropertyChanged
{
    private readonly IPopupService popupService;
    private readonly string? activeUserEmail;
    public AllClassesViewModel(IPopupService popupService)
    {
        this.popupService = popupService;
        ClassesSourceOfTruth = [];
        LoadCommand = new Command(execute: async () => await LoadClasses());
        DeleteClassCommand = new Command<Class>(execute: async (Class c) => await DeleteClass(c));
        FilterClassesCommand = new Command(execute: () => FilterClasses());
        activeUserEmail = Task.Run(() => AuthService.RetrieveUserEmailFromSecureStorage()).Result;
    }

    // Collections
    private ObservableCollection<Class> _classes = [];
    public ObservableCollection<Class> Classes
    {
        get => _classes;
        set
        {
            _classes = value;
            OnPropertyChanged(nameof(Classes));
        }
    }
    public List<Class> ClassesSourceOfTruth { get; set; }


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
    public Command DeleteClassCommand { get; }
    public Command FilterClassesCommand { get; }



    // Command Definitions
    private async Task DeleteClass(Class selectedClass)
    {
        // Delete class from Class table
        await DbContext.DeleteItemAsync(selectedClass);

        // Delete related term schedules from TermSchedule table
        TermSchedule termSchedule = await DbContext.GetFilteredItemAsync<TermSchedule>((ts) => ts.ClassId == selectedClass.Id && ts.CreatedBy == activeUserEmail);
        await DbContext.DeleteItemAsync(termSchedule);

        // Delete related exams from Exam table
        IEnumerable<Exam> examsToDelete = await DbContext.GetFilteredListAsync<Exam>(exam => exam.ClassId == selectedClass.Id && exam.CreatedBy == activeUserEmail);
        foreach (Exam exam in examsToDelete)
        {
            await DbContext.DeleteItemAsync(exam);
        }

        // Remove from Classes list (UI)
        Classes.Remove(selectedClass);
        ClassesSourceOfTruth.Remove(selectedClass);
    }

    private async Task LoadClasses()
    {
        Classes.Clear();
        ClassesSourceOfTruth.Clear();

        List<Class> dbClasses = (await DbContext.GetFilteredListAsync<Class>(c => c.CreatedBy == activeUserEmail)).ToList();

        foreach (Class dbClass in dbClasses)
        {
            Classes.Add(dbClass);
            ClassesSourceOfTruth.Add(dbClass);
        }
    }

    private void FilterClasses()
    {
        if (String.IsNullOrWhiteSpace(SearchParams))
        {
            Classes = ClassesSourceOfTruth.ToObservableCollection<Class>();
        }
        else
        {
            string param = SearchParams.ToLower().Trim();
            var result = Classes.Where(c => c.ClassName.ToLower().Trim().Contains(param));
            Classes = result.ToObservableCollection<Class>();
        }
    }

    public void ShowPopup(Class selectedClass)
    {
        popupService.ShowPopup<NotificationPopupViewModel>(viewmodel => viewmodel.OnAppearing(selectedClass));
    }



    // Other
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
