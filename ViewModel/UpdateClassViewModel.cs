using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using SemesterSync.Data;
using SemesterSync.Models;
using SemesterSync.Services;
using SemesterSync.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace SemesterSync.ViewModel;

[QueryProperty(nameof(SelectedClass), "SelectedClass")]

public class UpdateClassViewModel : INotifyPropertyChanged
{
    private readonly IPopupService popupService;
    private string? activeUserEmail;
    public UpdateClassViewModel(IPopupService popupService)
    {
        this.popupService = popupService;
        LoadCommand = new Command(execute: async () => await Load());
        BackCommand = new Command(execute: async () => await Back());
        SaveCommand = new Command(execute: async () => await Save());
        activeUserEmail = Task.Run(() => AuthService.RetrieveUserEmailFromSecureStorage()).Result;
        ExamEllipsisClickedCommand = new Command<DetailedExam>(execute: async (DetailedExam selectedExam) => await ExamEllipsisClicked(selectedExam));
        EditExamCommand = new Command<DetailedExam>(execute: async (DetailedExam selectedExam) => await EditExam(selectedExam));
        AddExamCommand = new Command(execute: async () => await AddExam());
        DeleteExamCommand = new Command<DetailedExam>(execute: (DetailedExam selectedExam) => DeleteExam(selectedExam));
    }


    // Collections
    public List<Exam> ExistingExamList { get; set; } = [];
    public List<string> StatusOptions { get; set; } = ["Active", "Failed", "Passed", "Upcoming", "Withdrawn"];


    // Objects
    public Class? SelectedClass
    {
        get => _selectedClass;
        set
        {
            _selectedClass = value;
            OnPropertyChanged(nameof(SelectedClass));
        }
    }
    public Instructor Instructor
    {
        get => _instructor;
        set
        {
            _instructor = value;
            OnPropertyChanged(nameof(Instructor));
        }
    }


    // UI Inputs
    public string ClassName
    {
        get => _className;
        set
        {
            _className = value;
            OnPropertyChanged(nameof(ClassName));
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
    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
        }
    }
    public DateOnly? StartNotificationDate
    {
        get => _startNotificationDate;
        set
        {
            _startNotificationDate = value;
            OnPropertyChanged(nameof(StartNotificationDate));
        }
    }
    public TimeOnly? StartNotificationTime
    {
        get => _startNotificationTime;
        set
        {
            _startNotificationTime = value;
            OnPropertyChanged(nameof(StartNotificationTime));
        }
    }
    public DateOnly? EndNotificationDate
    {
        get => _endNotificationDate;
        set
        {
            _endNotificationDate = value;
            OnPropertyChanged(nameof(EndNotificationDate));
        }
    }
    public TimeOnly? EndNotificationTime
    {
        get => _endNotificationTime;
        set
        {
            _endNotificationTime = value;
            OnPropertyChanged(nameof(EndNotificationTime));
        }
    }
    public bool StartNotificationEnabled
    {
        get => _startNotificationEnabled;
        set
        {
            _startNotificationEnabled = value;
            OnPropertyChanged(nameof(StartNotificationEnabled));
        }
    }
    public bool EndNotificationEnabled
    {
        get => _endNotificationEnabled;
        set
        {
            _endNotificationEnabled = value;
            OnPropertyChanged(nameof(EndNotificationEnabled));
        }
    }
    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged(nameof(Status));
        }
    }
    public string Notes
    {
        get => _notes;
        set
        {
            _notes = value;
            OnPropertyChanged(nameof(Notes));
        }
    }
    public string InstructorName
    {
        get => _instructorName;
        set
        {
            _instructorName = value;
            OnPropertyChanged(nameof(InstructorName));
        }
    }
    public string InstructorEmail
    {
        get => _instructorEmail;
        set
        {
            _instructorEmail = value;
            OnPropertyChanged(nameof(InstructorEmail));
        }
    }
    public string InstructorPhoneNum
    {
        get => _instructorPhoneNum;
        set
        {
            _instructorPhoneNum = value;
            OnPropertyChanged(nameof(InstructorPhoneNum));
        }
    }
    public ObservableCollection<DetailedExam> ExamList { get; set; } = [];
    public string? SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            _selectedStatus = value;
            OnPropertyChanged(nameof(SelectedStatus));
        }
    }


    // Commands
    public Command LoadCommand { get; }
    public Command BackCommand { get; }
    public Command SaveCommand { get; }
    public Command ExamEllipsisClickedCommand { get; }
    public Command EditExamCommand { get; }
    public Command DeleteExamCommand { get; }
    public Command AddExamCommand { get; }


    // Command Definitions
    private async Task Load()
    {
        await Task.Delay(50);
        LoadingPopup loadingPopup = new();
        Shell.Current.CurrentPage.ShowPopup(loadingPopup);
        if (SelectedClass != null)
        {
            Instructor = await DbContext.GetFilteredItemAsync<Instructor>(instructor => instructor.Id == SelectedClass.InstructorId && instructor.CreatedBy == activeUserEmail);

            ClassName = SelectedClass.ClassName;
            StartDate = SelectedClass.StartDate;
            EndDate = SelectedClass.EndDate;
            Status = SelectedClass.Status;
            Notes = SelectedClass.Notes;
            InstructorName = Instructor.InstructorName;
            InstructorEmail = Instructor.Email;
            InstructorPhoneNum = Instructor.PhoneNumber;
            SelectedStatus = StatusOptions.Find(option => option == SelectedClass.Status);

            List<Exam> exams = (await DbContext.GetFilteredListAsync<Exam>(exam => exam.ClassId == SelectedClass.Id && exam.CreatedBy == activeUserEmail)).ToList();
            if (exams.Count > 0)
            {
                ExistingExamList = exams;
                List<ExamType> allExamTypes = await DbContext.GetAllAsync<ExamType>();
                foreach (Exam exam in exams)
                {
                    if (ExamList.ToList().Exists(existingExam => existingExam.ExamId == exam.Id))
                        break;

                    ExamType? relatedExamType = allExamTypes.Find(type => type.Id == exam.ExamTypeId);

                    if (relatedExamType != null)
                    {
                        ExamList.Add(new DetailedExam(exam, relatedExamType.Type));
                    }
                }
            }


        }

        else
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }
        await Task.Delay(250);
        loadingPopup.Close();
    }

    private static async Task Back()
    {
        await Shell.Current.GoToAsync("..");
    }

    private async Task Save()
    {
        bool allInputsValid = await InputsAreValid();
        if (!allInputsValid)
            return;

        if (SelectedClass == null)
            SelectedClass = new();

        bool classDetailsChanged = ClassDetailsChanged();

        if (classDetailsChanged)
        {
            SelectedClass.ClassName = ClassName;
            SelectedClass.Status = SelectedStatus;
            SelectedClass.StartDate = StartDate;
            SelectedClass.EndDate = EndDate;
            SelectedClass.Notes = Notes;
            SelectedClass.InstructorId = await GetInstructorId();
            SelectedClass.CreatedBy = activeUserEmail;

            if (SelectedClass.Id == 0)
            {
                await DbContext.AddItemAsync(SelectedClass);
            }
            else
            {
                await DbContext.UpdateItemAsync(SelectedClass);
            }
        }


        await AddOrDeleteExams();

        await Shell.Current.GoToAsync("..");

    }

    private async Task ExamEllipsisClicked(DetailedExam selectedExam)
    {
        string action = await Shell.Current.CurrentPage.DisplayActionSheet(selectedExam.ExamName, "Cancel", null, "Edit", "Delete", "Set Notification");
        Debug.WriteLine("Action: " + action);

        switch (action)
        {
            case "Edit":
                EditExamCommand.Execute(selectedExam);
                break;
            case "Delete":
                DeleteExamCommand.Execute(selectedExam);
                break;
            case "Set Notification":
                popupService.ShowPopup<NotificationPopupViewModel>(viewmodel => viewmodel.OnAppearing(selectedExam));
                break;
            default:
                Debug.WriteLine("No Action Selected");
                break;
        }
    }

    private async Task EditExam(DetailedExam selectedExam)
    {
        var result = await popupService.ShowPopupAsync<AddModifyExamPopupViewModel>(async viewmodel => await viewmodel.OnLoading(selectedExam));
        if (result != null)
        {
            DetailedExam updatedExam = (DetailedExam)result;
            if (updatedExam.ExamId == 0)
            {
                ExamList.Add(updatedExam);
            }
            else
            {
                int indexToReplace = ExamList.ToList().FindIndex(e => e.ExamId == updatedExam.ExamId);
                ExamList[indexToReplace] = updatedExam;
            }
        }
    }

    private async Task AddExam()
    {
        var result = await popupService.ShowPopupAsync<AddModifyExamPopupViewModel>(async viewmodel => await viewmodel.OnLoading());
        if (result != null)
        {
            DetailedExam updatedExam = (DetailedExam)result;
            ExamList.Add(updatedExam);
        }
    }

    private void DeleteExam(DetailedExam selectedDetailedExam)
    {
        ExamList.Remove(selectedDetailedExam);
    }


    // Helper Methods
    private async Task<bool> InputsAreValid()
    {
        bool classNameIsValid = ClassName.Length > 0;
        bool selectedStatusIsValid = SelectedStatus != null;
        bool instructorNameIsValid = InstructorName.Length > 0;
        bool instructorPhoneNumIsValid = InstructorPhoneNum.Length > 0;
        bool instructorEmailIsValid = InstructorEmail.Length > 0;
        bool startLessThanEnd = StartDate < EndDate;

        bool allInputsValid = classNameIsValid && selectedStatusIsValid && instructorNameIsValid && instructorPhoneNumIsValid && instructorEmailIsValid;

        if (!startLessThanEnd)
        {
            allInputsValid = false;

            string toastText = "End date must be after start date";
            var toast = Toast.Make(toastText);

            await toast.Show(cancellationTokenSource.Token);
            return allInputsValid;
        }
        if (!allInputsValid)
        {
            string toastText = "Fields with * are required";
            var toast = Toast.Make(toastText);

            await toast.Show(cancellationTokenSource.Token);
            return allInputsValid;
        }

        return allInputsValid;
    }

    private async Task AddOrDeleteExams()
    {
        List<Exam> newExams = [];
        foreach (DetailedExam detailedExam in ExamList)
        {
            Exam newExam = new()
            {
                Id = detailedExam.ExamId,
                ExamName = detailedExam.ExamName,
                StartTime = detailedExam.StartTime,
                EndTime = detailedExam.EndTime,
                ClassId = SelectedClass.Id,
                ExamTypeId = detailedExam.ExamTypeId,
                CreatedBy = detailedExam.CreatedBy,
            };
            if (newExam.Id == 0)
            {
                newExams.Add(newExam);
            }
            else
            {
                await DbContext.UpdateItemAsync(newExam);
            }

        }
        await DbContext.AddAllItemsAsync(newExams);

        foreach (Exam existingExam in ExistingExamList)
        {
            if (ExamList.ToList().Exists(exam => exam.ExamId == existingExam.Id))
                break;

            await DbContext.DeleteItemAsync(existingExam);
        }
    }

    private async Task<int> GetInstructorId()
    {
        Instructor existingInstructor = await DbContext.GetFilteredItemAsync<Instructor>(existingTeacher =>
                existingTeacher.InstructorName == InstructorName && existingTeacher.Email == InstructorEmail && existingTeacher.PhoneNumber == InstructorPhoneNum && existingTeacher.CreatedBy == activeUserEmail);

        if (existingInstructor == null)
        {
            existingInstructor = new()
            {
                InstructorName = InstructorName,
                PhoneNumber = InstructorPhoneNum,
                Email = InstructorEmail,
                CreatedBy = activeUserEmail
            };
            await DbContext.AddItemAsync(existingInstructor);
        }
        return existingInstructor.Id;
    }

    private bool ClassDetailsChanged()
    {
        if (SelectedClass.ClassName != ClassName) return true;
        if (SelectedClass.Status != SelectedStatus) return true;
        if (SelectedClass.StartDate != StartDate) return true;
        if (SelectedClass.EndDate != EndDate) return true;
        if (SelectedClass.Notes != Notes) return true;
        if (Instructor.InstructorName != InstructorName) return true;
        if (Instructor.PhoneNumber != InstructorPhoneNum) return true;
        if (Instructor.Email != InstructorEmail) return true;

        return false;
    }


    // Other
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    CancellationTokenSource cancellationTokenSource = new();

    // fields

    private Class? _selectedClass;
    private Instructor _instructor = new();
    private string _className = "";
    private DateTime _startDate;
    private DateTime _endDate;
    private DateOnly? _startNotificationDate;
    private TimeOnly? _startNotificationTime;
    private DateOnly? _endNotificationDate;
    private TimeOnly? _endNotificationTime;
    private bool _startNotificationEnabled = false;
    private bool _endNotificationEnabled = false;
    private string _status = "";
    private string _notes = "";
    private string _instructorName = "";
    private string _instructorEmail = "";
    private string _instructorPhoneNum = "";
    private string? _selectedStatus = null;
}
