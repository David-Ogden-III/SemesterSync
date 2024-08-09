using ViewModelLibrary;
using ModelLibrary;

namespace SemesterSyncTest;

public class UpdateClassVMTest
{
    [Fact]
    public void ClassDetailsChanged_NoChangesMade_returnFalse()
    {
        UpdateClassViewModel vm = new(null)
        {
            SelectedClass = new()
            {
                ClassName = "Class Name",
                Status = "Status",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Notes = "Notes",
            },
            Instructor = new()
            {
                InstructorName = "Name",
                PhoneNumber = "555-555-555",
                Email = "test@email.com"
            }
        };
        vm.ClassName = vm.SelectedClass.ClassName;
        vm.SelectedStatus = vm.SelectedClass.Status;
        vm.StartDate = vm.SelectedClass.StartDate;
        vm.EndDate = vm.SelectedClass.EndDate;
        vm.Notes = vm.SelectedClass.Notes;
        vm.InstructorName = vm.Instructor.InstructorName;
        vm.InstructorPhoneNum = vm.Instructor.PhoneNumber;
        vm.InstructorEmail = vm.Instructor.Email;

        bool detailsChanged = vm.ClassDetailsChanged();

        Assert.False(detailsChanged);
    }

    [Fact]
    public void ClassDetailsChanged_ClassNameChanged_returnTrue()
    {
        UpdateClassViewModel vm = new(null)
        {
            SelectedClass = new()
            {
                ClassName = "Class Name",
                Status = "Status",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Notes = "Notes",
            },
            Instructor = new()
            {
                InstructorName = "Name",
                PhoneNumber = "555-555-555",
                Email = "test@email.com"
            },
            ClassName = "New Class Name"
        };
        vm.SelectedStatus = vm.SelectedClass.Status;
        vm.StartDate = vm.SelectedClass.StartDate;
        vm.EndDate = vm.SelectedClass.EndDate;
        vm.Notes = vm.SelectedClass.Notes;
        vm.InstructorName = vm.Instructor.InstructorName;
        vm.InstructorPhoneNum = vm.Instructor.PhoneNumber;
        vm.InstructorEmail = vm.Instructor.Email;

        bool detailsChanged = vm.ClassDetailsChanged();

        Assert.True(detailsChanged);
    }

    [Fact]
    public void ClassDetailsChanged_InstructorNameChanged_returnTrue()
    {
        UpdateClassViewModel vm = new(null)
        {
            SelectedClass = new()
            {
                ClassName = "Class Name",
                Status = "Status",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Notes = "Notes",
            },
            Instructor = new()
            {
                InstructorName = "Name",
                PhoneNumber = "555-555-555",
                Email = "test@email.com"
            },
            InstructorName = "New Instructor Name"
        };
        vm.ClassName = vm.SelectedClass.ClassName;
        vm.SelectedStatus = vm.SelectedClass.Status;
        vm.StartDate = vm.SelectedClass.StartDate;
        vm.EndDate = vm.SelectedClass.EndDate;
        vm.Notes = vm.SelectedClass.Notes;
        vm.InstructorPhoneNum = vm.Instructor.PhoneNumber;
        vm.InstructorEmail = vm.Instructor.Email;

        bool detailsChanged = vm.ClassDetailsChanged();

        Assert.True(detailsChanged);
    }

    [Fact]
    public void ClassDetailsChanged_SelectedClassNull_returnTrue()
    {
        UpdateClassViewModel vm = new(null)
        {
            SelectedClass = null,
            Instructor = new()
            {
                InstructorName = "Name",
                PhoneNumber = "555-555-555",
                Email = "test@email.com"
            },
            InstructorName = "New Instructor Name"
        };

        bool detailsChanged = vm.ClassDetailsChanged();

        Assert.True(detailsChanged);
    }

    [Fact]
    public async Task InputsAreValid_AllInputsValid_ReturnTrue()
    {
        UpdateClassViewModel vm = new(null)
        {
            ClassName = "class name",
            SelectedStatus = "status",
            InstructorName = "instructor name",
            InstructorPhoneNum = "5555555555",
            InstructorEmail = "Instructor@email.com",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1)
        };

        bool allInputsValid = await vm.InputsAreValid();

        Assert.True(allInputsValid);
    }

    [Fact]
    public async Task InputsAreValid_ClassNameEmpty_ReturnFalse()
    {
        UpdateClassViewModel vm = new(null)
        {
            ClassName = String.Empty,
            SelectedStatus = "status",
            InstructorName = "instructor name",
            InstructorPhoneNum = "5555555555",
            InstructorEmail = "Instructor@email.com",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1)
        };

        bool allInputsValid = await vm.InputsAreValid();

        Assert.False(allInputsValid);
    }

    [Fact]
    public async Task InputsAreValid_EndDateBeforeStartDate_ReturnFalse()
    {
        UpdateClassViewModel vm = new(null)
        {
            ClassName = "class name",
            SelectedStatus = "status",
            InstructorName = "instructor name",
            InstructorPhoneNum = "5555555555",
            InstructorEmail = "Instructor@email.com",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(-1)
        };

        bool allInputsValid = await vm.InputsAreValid();

        Assert.False(allInputsValid);
    }

    [Fact]
    public async Task InputsAreValid_StatusIsNull_ReturnFalse()
    {
        UpdateClassViewModel vm = new(null)
        {
            ClassName = "class name",
            SelectedStatus = null,
            InstructorName = "instructor name",
            InstructorPhoneNum = "5555555555",
            InstructorEmail = "Instructor@email.com",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1)
        };

        bool allInputsValid = await vm.InputsAreValid();

        Assert.False(allInputsValid);
    }
}
