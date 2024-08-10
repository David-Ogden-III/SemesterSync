using ModelLibrary;
using ViewModelLibrary;

namespace SemesterSyncTest;

public class AllClassesVMTest
{
    [Fact]
    public void FilterClasses_SearchParamNull_ClassesNotNull()
    {
        AllClassesViewModel vm = new(null)
        {
            SearchParams = String.Empty,
            ClassesSourceOfTruth = [new Class(), new Class()]
        };
        vm.Classes.Clear();


        vm.FilterClassesCommand.Execute(null);

        Assert.NotEmpty(vm.Classes);
    }

    [Fact]
    public void FilterClasses_SearchParamNotNull_ClassesCountEqualTwo()
    {
        AllClassesViewModel vm = new(null)
        {
            SearchParams = "test",
            ClassesSourceOfTruth = [new Class() { ClassName = "test" }, new Class() { ClassName = "anotherTeSt" }, new Class() { ClassName = "DoesntContainSearchParam" }]
        };
        vm.Classes.Clear();


        vm.FilterClassesCommand.Execute(null);

        Assert.Equal(2, vm.Classes.Count);
    }

    [Fact]
    public void FilterClasses_SearchParamNotNullCharacterRemoved_ClassesCountEqualTwo()
    {
        AllClassesViewModel vm = new(null)
        {
            SearchParams = "test1",
            ClassesSourceOfTruth = [new Class() { ClassName = "test1" }, new Class() { ClassName = "anotherTeSt" }, new Class() { ClassName = "DoesntContainSearchParam" }]
        };
        vm.Classes.Clear();
        vm.FilterClassesCommand.Execute(null);
        vm.SearchParams = "test";
        vm.FilterClassesCommand.Execute(null);

        Assert.Equal(2, vm.Classes.Count);
    }
}
