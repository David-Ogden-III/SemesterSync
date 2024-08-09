using ModelLibrary;
using ViewModelLibrary;

namespace SemesterSyncTest;

public class TermDetailsVMTest
{
    [Fact]
    public void SelectionChanged_AllPropsNotNull_SelectedCGNotEmpty()
    {
        TermDetailsViewModel vm = new()
        {
            SelectedCG = new(new Term(), []),
            SelectedClass = new()
        };

        vm.SelectionChangedCommand.Execute(null);

        Assert.NotEmpty(vm.SelectedCG);
    }

    [Fact]
    public void SelectionChanged_SelectCGNull_SelectedCGEmpty()
    {
        TermDetailsViewModel vm = new()
        {
            SelectedCG = null,
            SelectedClass = new()
        };

        vm.SelectionChangedCommand.Execute(null);

        Assert.Null(vm.SelectedCG);
    }

    [Fact]
    public void SelectionChanged_SelectedClassNull_SelectedCGEmpty()
    {
        TermDetailsViewModel vm = new()
        {
            SelectedCG = new(new Term(), []),
            SelectedClass = null
        };

        vm.SelectionChangedCommand.Execute(null);

        Assert.Empty(vm.SelectedCG);
    }

    [Fact]
    public void RemoveClass_SelectedCGNotNull_SelectedCGEmpty()
    {
        Class classToDelete = new();
        TermDetailsViewModel vm = new()
        {
            SelectedCG = new(new Term(), [classToDelete])
        };

        vm.RemoveClassCommand.Execute(classToDelete);

        Assert.Empty(vm.SelectedCG);
    }
}
