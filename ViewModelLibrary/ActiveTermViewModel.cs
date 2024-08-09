using CommunityToolkit.Maui.Core;
using DataLibrary;
using ModelLibrary;
using ServiceLibrary;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModelLibrary;

public class ActiveTermViewModel : INotifyPropertyChanged
{
    private readonly IPopupService popupService;
    private string? activeUserEmail;
    private readonly AuthService authService = AuthService.GetInstance();
    public ActiveTermViewModel(IPopupService popupService)
    {
        this.popupService = popupService;
        ActiveTerm = null;
        ActiveTermIsNotNull = false;
        LoadCommand = new Command(execute: async () => await LoadActiveTermAsync());
        DeleteClassCommand = new Command<Class>(execute: async (Class c) => await DeleteClass(c));
    }

    // Collections
    public ObservableCollection<Class> ActiveClasses { get; set; } = [];
    public Command LoadCommand { get; }


    // Objects
    public Term? ActiveTerm
    {
        get => _activeTerm;
        set
        {
            _activeTerm = value;
            OnPropertyChanged(nameof(ActiveTerm));
        }
    }
    public bool ActiveTermIsNotNull
    {
        get => _activeTermIsNotNull;
        set
        {
            _activeTermIsNotNull = value;
            OnPropertyChanged(nameof(ActiveTermIsNotNull));
        }
    }


    // Commands

    public Command DeleteClassCommand { get; }


    // Command Definitions
    public async Task LoadActiveTermAsync()
    {
        activeUserEmail = await authService.RetrieveUserEmailFromSecureStorage();

        ActiveClasses.Clear();
        ActiveTerm = await DbContext.GetFilteredItemAsync<Term>((term) => term.StartDate < DateTime.Now && term.EndDate > DateTime.Now && term.CreatedBy == activeUserEmail);

        ActiveTermIsNotNull = ActiveTerm != null;

        if (ActiveTermIsNotNull)
        {
            IEnumerable<TermSchedule> filteredTermSchedules = await DbContext.GetFilteredListAsync<TermSchedule>((termSchedule) => termSchedule.TermId == ActiveTerm.Id && termSchedule.CreatedBy == activeUserEmail);
            foreach (TermSchedule termSchedule in filteredTermSchedules)
            {
                Class activeClass = await DbContext.GetFilteredItemAsync<Class>((classActive) => classActive.Id == termSchedule.ClassId && classActive.CreatedBy == activeUserEmail);
                ActiveClasses.Add(activeClass);
            }
        }
    }

    private async Task DeleteClass(Class selectedClass)
    {
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
        ActiveClasses.Remove(selectedClass);
    }

    public void ShowPopup(Class selectedClass)
    {
        popupService.ShowPopup<NotificationPopupViewModel>(viewmodel => viewmodel.OnAppearing(selectedClass));
    }

    // Helpers
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    private bool _activeTermIsNotNull;
    private Term? _activeTerm;
}
