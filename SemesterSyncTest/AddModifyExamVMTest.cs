using ModelLibrary;
using ViewModelLibrary;

namespace SemesterSyncTest;

public class AddModifyExamVMTest
{
    [Fact]
    public void ExamPropsChanged_ReturnTrue()
    {
        AddModifyExamPopupViewModel vm = new();

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
        vm.SelectedExam = newDetailedExam;
        vm.ExamName = "Exam123";

        bool inputsChanged = vm.ExamPropertiesChanged();

        Assert.True(inputsChanged);
    }

    [Fact]
    public void ExamPropsChanged_NoPropsChanged_ReturnFalse()
    {
        AddModifyExamPopupViewModel vm = new();

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
        vm.SelectedExam = newDetailedExam;
        vm.StartDate = vm.SelectedExam.StartTime;
        vm.EndDate = vm.SelectedExam.EndTime;
        vm.StartTime = vm.SelectedExam.StartTime.TimeOfDay;
        vm.EndTime = vm.SelectedExam.EndTime.TimeOfDay;
        vm.InitialExamTypeIsToggled = true;
        vm.ExamTypeIsToggled = true;
        vm.ExamName = "Test";

        bool inputsChanged = vm.ExamPropertiesChanged();

        Assert.False(inputsChanged);
    }

    [Fact]
    public async Task ValidateInputs_EndDateBeforeStart_ReturnsFalse()
    {
        AddModifyExamPopupViewModel vm = new()
        {
            StartDate = new DateTime(2024, 06, 01),
            StartTime = new TimeSpan(12, 0, 0),
            EndDate = new DateTime(2024, 05, 01),
            EndTime = new TimeSpan(12, 0, 0),
            ExamName = "TestExam"
        };

        bool inputsAreValid = await vm.ValidateInputs();

        Assert.False(inputsAreValid);
    }

    [Fact]
    public async Task ValidateInputs_ExamNameEmpty_ReturnsFalse()
    {
        AddModifyExamPopupViewModel vm = new()
        {
            StartDate = new DateTime(2024, 05, 01),
            StartTime = new TimeSpan(12, 0, 0),
            EndDate = new DateTime(2024, 06, 01),
            EndTime = new TimeSpan(12, 0, 0),
            ExamName = String.Empty
        };

        bool inputsAreValid = await vm.ValidateInputs();

        Assert.False(inputsAreValid);
    }

    [Fact]
    public async Task ValidateInputs_InputsValid_ReturnsTrue()
    {
        AddModifyExamPopupViewModel vm = new()
        {
            StartDate = new DateTime(2024, 05, 01),
            StartTime = new TimeSpan(12, 0, 0),
            EndDate = new DateTime(2024, 06, 01),
            EndTime = new TimeSpan(12, 0, 0),
            ExamName = "TestExam"
        };

        bool inputsAreValid = await vm.ValidateInputs();

        Assert.True(inputsAreValid);
    }

    [Fact]
    public async Task Save_ExamNameChanged_ReturnDetailedExam()
    {
        AddModifyExamPopupViewModel vm = new();
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
        vm.SelectedExam = newDetailedExam;
        vm.StartDate = vm.SelectedExam.StartTime;
        vm.EndDate = vm.SelectedExam.EndTime;
        vm.StartTime = vm.SelectedExam.StartTime.TimeOfDay;
        vm.EndTime = vm.SelectedExam.EndTime.TimeOfDay;
        vm.InitialExamTypeIsToggled = false;
        vm.ExamTypeIsToggled = false;
        vm.ExamName = "NewExamName";
        vm.ExamTypes = [new ExamType() {
            Id = 2,
            Type = "Objective Assessment"
        }];

        DetailedExam? newExam = await vm.Save();

        Assert.NotNull(newExam);
    }

    [Fact]
    public async Task Save_SelectedExamIsNull_ReturnDetailedExam()
    {
        AddModifyExamPopupViewModel vm = new();
        vm.SelectedExam = null;
        vm.StartDate = new DateTime(2024,06,01);
        vm.EndDate = new DateTime(2025, 06, 01);
        vm.StartTime = vm.StartDate.TimeOfDay;
        vm.EndTime = vm.EndDate.TimeOfDay;
        vm.InitialExamTypeIsToggled = false;
        vm.ExamTypeIsToggled = false;
        vm.ExamName = "NewExamName";
        vm.ExamTypes = [new ExamType() {
            Id = 2,
            Type = "Objective Assessment"
        }];

        DetailedExam? newExam = await vm.Save();

        Assert.NotNull(newExam);
    }

    [Fact]
    public async Task Save_NoPropsChanged_ReturnNull()
    {
        AddModifyExamPopupViewModel vm = new();
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
        vm.SelectedExam = newDetailedExam;
        vm.StartDate = vm.SelectedExam.StartTime;
        vm.EndDate = vm.SelectedExam.EndTime;
        vm.StartTime = vm.SelectedExam.StartTime.TimeOfDay;
        vm.EndTime = vm.SelectedExam.EndTime.TimeOfDay;
        vm.InitialExamTypeIsToggled = false;
        vm.ExamTypeIsToggled = false;
        vm.ExamName = vm.SelectedExam.ExamName;
        vm.ExamTypes = [new ExamType() {
            Id = 2,
            Type = "Objective Assessment"
        }];

        DetailedExam? newExam = await vm.Save();

        Assert.Null(newExam);
    }

    [Fact]
    public async Task Save_InputInvalid_ReturnNull()
    {
        AddModifyExamPopupViewModel vm = new();
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
        vm.SelectedExam = newDetailedExam;
        vm.StartDate = vm.SelectedExam.StartTime;
        vm.EndDate = vm.SelectedExam.EndTime;
        vm.StartTime = vm.SelectedExam.StartTime.TimeOfDay;
        vm.EndTime = vm.SelectedExam.EndTime.TimeOfDay;
        vm.InitialExamTypeIsToggled = false;
        vm.ExamTypeIsToggled = false;
        vm.ExamName = String.Empty;
        vm.ExamTypes = [new ExamType() {
            Id = 2,
            Type = "Objective Assessment"
        }];

        DetailedExam? newExam = await vm.Save();

        Assert.Null(newExam);
    }
}
