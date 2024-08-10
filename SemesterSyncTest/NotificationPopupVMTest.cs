using ModelLibrary;
using ViewModelLibrary;

namespace SemesterSyncTest;

public class NotificationPopupVMTest
{
    [Fact]
    public void OnAppearing_SelectedClassNotNull()
    {
        Class newClass1 = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };
        NotificationPopupViewModel vm = new();

        vm.OnAppearing(newClass1);

        Assert.NotNull(vm.SelectedClass);
    }

    [Fact]
    public void OnAppearing_TitleEqualsClassName()
    {
        Class newClass1 = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };
        NotificationPopupViewModel vm = new();

        vm.OnAppearing(newClass1);

        Assert.Equal(newClass1.ClassName, vm.Title);
    }

    [Fact]
    public void OnAppearing_SelectedExamNotNull()
    {
        Exam exam = new()
        {
            Id = 1,
            ExamName = "Test",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(1),
            ClassId = 15,
            ExamTypeId = 2,
            CreatedBy = "TestUser"
        };
        DetailedExam newDetailedExam = new(exam, "Objective Assessment");
        NotificationPopupViewModel vm = new();

        vm.OnAppearing(newDetailedExam);

        Assert.NotNull(vm.SelectedExam);
    }

    [Fact]
    public void OnAppearing_TitleEqualClassName()
    {
        Exam exam = new()
        {
            Id = 1,
            ExamName = "Test",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddDays(1),
            ClassId = 15,
            ExamTypeId = 2,
            CreatedBy = "TestUser"
        };
        DetailedExam newDetailedExam = new(exam, "Objective Assessment");
        NotificationPopupViewModel vm = new();

        vm.OnAppearing(newDetailedExam);

        Assert.Equal(newDetailedExam.ExamName, vm.Title);
    }
}
