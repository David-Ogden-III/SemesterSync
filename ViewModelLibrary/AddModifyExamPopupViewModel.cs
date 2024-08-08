using CommunityToolkit.Maui.Alerts;
using DataLibrary;
using ModelLibrary;
using ServiceLibrary;
using System.ComponentModel;

namespace ViewModelLibrary;

public class AddModifyExamPopupViewModel : INotifyPropertyChanged
{
    private string? activeUserEmail;
    public AddModifyExamPopupViewModel()
    {
    }

    public List<ExamType> ExamTypes { get; set; } = [];
    public bool InitialExamTypeIsToggled { get; set; }

    public DetailedExam? SelectedExam { get; set; }

    public string PopupTitle
    {
        get => _popupTitle;
        set
        {
            _popupTitle = value;
            OnPropertyChanged(nameof(PopupTitle));
        }
    }
    public string ExamName
    {
        get => _examName;
        set
        {
            _examName = value;
            OnPropertyChanged(nameof(ExamName));
        }
    }
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));
        }
    }
    public TimeSpan StartTime
    {
        get => _startTime;
        set
        {
            _startTime = value;
            OnPropertyChanged(nameof(StartTime));
        }
    }
    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
        }
    }
    public TimeSpan EndTime
    {
        get => _endTime;
        set
        {
            _endTime = value;
            OnPropertyChanged(nameof(EndTime));
        }
    }
    public bool ExamTypeIsToggled
    {
        get => _examTypeIsToggled;
        set
        {
            _examTypeIsToggled = value;
            OnPropertyChanged(nameof(ExamTypeIsToggled));
        }
    }

    public async Task OnLoading(DetailedExam selectedExam)
    {
        activeUserEmail = await AuthService.RetrieveUserEmailFromSecureStorage();
        ExamTypes = await DbContext.GetAllAsync<ExamType>();
        SelectedExam = selectedExam;
        ExamName = selectedExam.ExamName;
        PopupTitle = selectedExam.ExamName;
        StartDate = selectedExam.StartTime;
        StartTime = selectedExam.StartTime.TimeOfDay;
        EndDate = selectedExam.EndTime;
        EndTime = selectedExam.EndTime.TimeOfDay;
        ExamTypeIsToggled = selectedExam.ExamType == "Performance Assessment";
        InitialExamTypeIsToggled = selectedExam.ExamType == "Performance Assessment";
    }

    public async Task OnLoading()
    {
        activeUserEmail = await AuthService.RetrieveUserEmailFromSecureStorage();
        ExamTypes = await DbContext.GetAllAsync<ExamType>();
    }

    public async Task<DetailedExam?> Save()
    {
        bool inputsAreValid = await ValidateInputs();
        if (!inputsAreValid)
        {
            return null;    
        }

        SelectedExam ??= new(new Exam(), "")
        {
            ExamName = String.Empty,
            CreatedBy = activeUserEmail
        };

        bool examPropertiesChanged = ExamPropertiesChanged();

        if (examPropertiesChanged)
        {
            SelectedExam.ExamName = ExamName;
            SelectedExam.StartTime = new DateTime(DateOnly.FromDateTime(StartDate), TimeOnly.FromTimeSpan(StartTime));
            SelectedExam.EndTime = new DateTime(DateOnly.FromDateTime(EndDate), TimeOnly.FromTimeSpan(EndTime));

            if (ExamTypeIsToggled)
            {
                SelectedExam.ExamType = "Performance Assessment";
            }
            else
            {
                SelectedExam.ExamType = "Objective Assessment";
            }

            SelectedExam.ExamTypeId = (ExamTypes.Find(examType => examType.Type == SelectedExam.ExamType)).Id;

            return SelectedExam;

        }
        return null;
    }

    public bool ExamPropertiesChanged()
    {
        if (SelectedExam.ExamName != ExamName) return true;
        if (!SelectedExam.StartTime.Date.Equals(StartDate.Date)) return true;
        if (!SelectedExam.EndTime.Date.Equals(EndDate.Date)) return true;
        if (!SelectedExam.StartTime.TimeOfDay.Equals(StartTime)) return true;
        if (!SelectedExam.EndTime.TimeOfDay.Equals(EndTime)) return true;
        if (ExamTypeIsToggled != InitialExamTypeIsToggled) return true;

        return false;
    }
    public async Task<bool> ValidateInputs()
    {
        string toastText = "End date must be after start date";
        bool inputsAreValid = true;

        DateOnly startDate = DateOnly.FromDateTime(StartDate);
        TimeOnly startTime = TimeOnly.FromTimeSpan(StartTime);
        DateTime startDateTime = new DateTime(startDate, startTime);

        DateOnly endDate = DateOnly.FromDateTime(EndDate);
        TimeOnly endTime = TimeOnly.FromTimeSpan(EndTime);
        DateTime endDateTime = new DateTime(endDate, endTime);

        inputsAreValid = startDateTime < endDateTime;

        if (inputsAreValid && String.IsNullOrWhiteSpace(ExamName))
        {
            inputsAreValid = false;
            toastText = "Exam Name is Required";
        }

        if (!inputsAreValid)
        {
            var toast = Toast.Make(toastText);

            await toast.Show(cancellationTokenSource.Token);
        }

        return inputsAreValid;
    }

    // Other
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    CancellationTokenSource cancellationTokenSource = new();

    // fields
    private string _popupTitle = "Add Exam";
    private string _examName = String.Empty;
    private bool _examTypeIsToggled = false;

    private DateTime _startDate = DateTime.Now;
    private TimeSpan _startTime = new(8, 0, 0);

    private DateTime _endDate = DateTime.Now;
    private TimeSpan _endTime = new(9, 0, 0);
}
