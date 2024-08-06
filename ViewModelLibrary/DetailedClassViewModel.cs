using DataLibrary;
using ModelLibrary;
using ServiceLibrary;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModelLibrary;

[QueryProperty(nameof(SelectedClass), "SelectedClass")]
public class DetailedClassViewModel : INotifyPropertyChanged
{

    private string? activeUserEmail;
    public DetailedClassViewModel()
    {
        LoadCommand = new Command(execute: async () => await Load());
        BackCommand = new Command(execute: async () => await Back());
        ShareCommand = new Command(execute: async () => await ShareTask());
        activeUserEmail = Task.Run(() => AuthService.RetrieveUserEmailFromSecureStorage()).Result;
    }

    // Helper Class


    // Collections
    public ObservableCollection<DetailedExam> ExamList { get; set; } = [];

    // Objects
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

    private Instructor _instructor = new();
    public Instructor Instructor
    {
        get => _instructor;
        set
        {
            _instructor = value;
            OnPropertyChanged(nameof(Instructor));
        }
    }

    private bool _notesHasText = false;
    public bool NotesHasText
    {
        get => _notesHasText;
        set
        {
            _notesHasText = value;
            OnPropertyChanged(nameof(NotesHasText));
        }
    }


    // Commands
    public Command LoadCommand { get; }
    public Command BackCommand { get; }
    public Command ShareCommand { get; }


    // Command Definitions
    private async Task Load()
    {
        if (SelectedClass == null)
        {
            await Back();
        }
        else
        {
            NotesHasText = SelectedClass.Notes.Length > 0;
            List<Exam> exams = (await DbContext.GetFilteredListAsync<Exam>(exam => exam.ClassId == SelectedClass.Id && exam.CreatedBy == activeUserEmail)).ToList();

            if (exams.Count > 0)
            {
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

            Instructor = await DbContext.GetFilteredItemAsync<Instructor>(instructor => instructor.Id == SelectedClass.InstructorId && instructor.CreatedBy == activeUserEmail);
        }
    }

    private static async Task Back()
    {
        await Shell.Current.GoToAsync("..");
    }

    private async Task ShareTask()
    {
        if (SelectedClass != null && SelectedClass.Notes.Length > 0)
        {
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = SelectedClass.Notes,
                Title = $"{SelectedClass.ClassName}: Share Class Notes"
            });
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
