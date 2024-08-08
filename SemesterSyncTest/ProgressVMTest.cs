using ModelLibrary;
using ViewModelLibrary;

namespace SemesterSyncTest;

public class ProgressVMTest
{
    [Fact]
    public void SelectionChanged_AllTermsSelected_ReportGroupsNotNull()
    {
        ProgressViewModel vm = new ProgressViewModel()
        {
            SelectedTerm = new Term() { Id = -1 },
            AllTermsReportGroup = [new ReportGroup("Active", []), new ReportGroup("Withdrawn", [])]
        };

        vm.SelectionChangedCommand.Execute(null);

        Assert.NotEmpty(vm.ReportGroups);
    }

    [Fact]
    public void SelectionChanged_SpecificTermSelected_ReportGroupsNotNull()
    {
        ProgressViewModel vm = new()
        {
            SelectedTerm = new Term() { Id = 1 },
            TermSchedules = [new TermSchedule() {TermId = 1, ClassId = 3}, new TermSchedule() { TermId = 1, ClassId = 4 }, new TermSchedule() { TermId = 2, ClassId = 5 }],
            Classes = [new Class() { Id = 3, Status = "Active"}, new Class() { Id = 4, Status = "Active" }, new Class() { Id = 5, Status = "Passed" }]
        };
        
        vm.SelectionChangedCommand.Execute(null);

        int count = vm.ReportGroups[0].Count;

        Assert.Equal(2,count);
    }
}
