using SemesterSync.Database;
using Plugin.LocalNotification;
using System.ComponentModel;
using System.Diagnostics;

namespace SemesterSync.ViewModel;

public class NotificationPopupViewModel : INotifyPropertyChanged
{

    // Methods
    public void Save()
    {
        string title;
        string description;
        int id = 0;
        if (SelectedClass != null)
        {
            title = SelectedClass.ClassName;
            description = SelectedClass.StartDate.ToString("h:mm tt - M/d/yy");
            id = SelectedClass.Id;
        }
        else if (SelectedExam != null)
        {
            title = SelectedExam.ExamName;
            description = SelectedExam.StartTime.ToString("h:mm tt - M/d/yy");
            id = SelectedExam.ExamId;
        }
        else return;


        DateTime notifyTime = new(DateOnly.FromDateTime(Date), TimeOnly.FromTimeSpan(Time), DateTimeKind.Local);

        var request = new NotificationRequest();
        Debug.WriteLine(notifyTime.Kind);

        request.NotificationId = id;
        request.Title = title;
        request.Description = description;
        request.Schedule = new NotificationRequestSchedule() { NotifyTime = notifyTime };

        LocalNotificationCenter.Current.Show(request);
    }

    public void OnAppearing(Class selectedClass)
    {
        SelectedClass = selectedClass;
        Title = selectedClass.ClassName;
    }

    public void OnAppearing(DetailedExam selectedExam)
    {
        SelectedExam = selectedExam;
        Title = selectedExam.ExamName;
    }

    // Properties
    public Class? SelectedClass { get; set; }
    public DetailedExam? SelectedExam { get; set; }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }

    }
    public DateTime Date
    {
        get => _date;
        set
        {
            _date = value;
            OnPropertyChanged(nameof(Date));
        }
    }
    public TimeSpan Time
    {
        get => _time;
        set
        {
            _time = value;
            OnPropertyChanged(nameof(Time));
        }
    }


    // Other
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    // fields
    private string _title = string.Empty;
    private DateTime _date = DateTime.Now;
    private TimeSpan _time = new(8, 0, 0);
}
