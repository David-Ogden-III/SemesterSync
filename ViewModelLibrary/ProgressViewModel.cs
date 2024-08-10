using CommunityToolkit.Maui.Core.Extensions;
using DataLibrary;
using ModelLibrary;
using ServiceLibrary;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModelLibrary;

public class ProgressViewModel : INotifyPropertyChanged
{
    private readonly AuthService authService = AuthService.GetInstance();
    public ProgressViewModel()
    {
        LoadCommand = new Command(execute: async () => await Load());
        BackCommand = new Command(execute: async () => await Back());
        SelectionChangedCommand = new Command(execute: () => SelectionChanged());

    }

    public static string? ActiveUserEmail { get; set; }
    public List<Class> Classes { get; set; } = [];
    public List<TermSchedule> TermSchedules { get; set; } = [];
    public List<Term> Terms { get; set; } = [];
    public List<string> Statuses { get; set; } = ["Active", "Failed", "Passed", "Upcoming", "Withdrawn"];
    public ObservableCollection<ReportGroup> ReportGroups
    {
        get => _reportGroups;
        set
        {
            _reportGroups = value;
            OnPropertyChanged(nameof(ReportGroups));
        }
    }
    public List<ReportGroup> AllTermsReportGroup { get; set; } = [];
    public Term? SelectedTerm
    {
        get => _selectedTerm;
        set
        {
            _selectedTerm = value;
            OnPropertyChanged(nameof(SelectedTerm));
        }
    }
    public List<Term> PickerOptions
    {
        get => _pickerOptions;
        set
        {
            _pickerOptions = value;
            OnPropertyChanged(nameof(PickerOptions));
        }
    }
    public string ReportTitle
    {
        get => _reportTitle;
        set
        {
            _reportTitle = value;
            OnPropertyChanged(nameof(ReportTitle));
        }
    }
    public DateTime ReportTime
    {
        get => _reportTime;
        set
        {
            _reportTime = value;
            OnPropertyChanged(nameof(ReportTime));
        }
    }


    public Command BackCommand { get; }
    public Command LoadCommand { get; }
    public Command SelectionChangedCommand { get; }

    private async Task Load()
    {
        Classes.Clear();
        TermSchedules.Clear();
        Terms.Clear();
        AllTermsReportGroup.Clear();
        ActiveUserEmail = await authService.RetrieveUserEmailFromSecureStorage();
        Classes = (await DbContext.GetFilteredListAsync<Class>(c => c.CreatedBy == ActiveUserEmail)).ToList();
        TermSchedules = (await DbContext.GetFilteredListAsync<TermSchedule>(ts => ts.CreatedBy == ActiveUserEmail)).ToList();
        Terms = (await DbContext.GetFilteredListAsync<Term>(term => term.CreatedBy == ActiveUserEmail)).ToList();

        Term defaultPickerTermOption = new()
        {
            TermName = "All Terms",
            CreatedBy = ActiveUserEmail,
            Id = -1,
        };
        PickerOptions = [defaultPickerTermOption, .. Terms];

        foreach (string status in Statuses)
        {
            ObservableCollection<Class> selectedClasses = Classes.Where(c => c.Status == status).ToObservableCollection();
            ReportGroup newRG = new(status, selectedClasses);
            AllTermsReportGroup.Add(newRG);
        }

        SelectedTerm = PickerOptions[0];
    }

    private static async Task Back()
    {
        await Shell.Current.GoToAsync("..");
    }

    private void SelectionChanged()
    {
        if (SelectedTerm.Id == -1)
        {
            ReportGroups = AllTermsReportGroup.ToObservableCollection();
        }
        else
        {
            List<TermSchedule> tempSchedules = TermSchedules.Where(ts => ts.TermId == SelectedTerm.Id).ToList();
            ObservableCollection<ReportGroup> selectedReport = [];

            foreach (string status in Statuses)
            {
                ObservableCollection<Class> selectedClasses = Classes.Where(c => c.Status == status && tempSchedules.Exists(ts => ts.ClassId == c.Id)).ToObservableCollection();
                ReportGroup newRG = new(status, selectedClasses);
                selectedReport.Add(newRG);
            }
            ReportGroups = selectedReport;
        }
        ReportTitle = SelectedTerm.TermName;
        ReportTime = DateTime.Now;
    }



    private List<Term> _pickerOptions = [];
    private Term? _selectedTerm;
    private string _reportTitle = "Choose a Term";
    private ObservableCollection<ReportGroup> _reportGroups = [];
    private DateTime _reportTime;


    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));






}
