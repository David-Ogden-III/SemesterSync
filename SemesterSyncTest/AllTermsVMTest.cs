using ModelLibrary;
using ViewModelLibrary;

namespace SemesterSyncTest;

public class AllTermsVMTest
{
    [Fact]
    public void FilterTerms_SearchParamNull_ClassesNotNull()
    {
        AllTermsViewModel vm = new(null)
        {
            SearchParams = String.Empty,
            ClassesSourceOfTruth = [new ClassGroup(new Term(), []),new ClassGroup(new Term(), [])]
        };
        vm.Classes.Clear();


        vm.FilterTermsCommand.Execute(null);

        Assert.NotEmpty(vm.Classes);
    }

    [Fact]
    public void FilterTerms_SearchParamNotNull_ClassesCountEqualTwo()
    {
        AllTermsViewModel vm = new(null)
        {
            SearchParams = "test",
            ClassesSourceOfTruth = [new ClassGroup(new Term() { TermName = "testTermName" }, [new Class() { ClassName = "Doesnt Contain Search Param" }]),
                new ClassGroup(new Term() { TermName = "Doesnt Contain Search Param" }, [new Class() { ClassName = "anotherTeSt" }]),
                new ClassGroup(new Term() { TermName = "Doesnt Contain Search Param" }, [new Class() { ClassName = "Doesnt Contain Search Param" }])]
        };
        vm.Classes.Clear();


        vm.FilterTermsCommand.Execute(null);

        Assert.Equal(2, vm.Classes.Count);
    }

    [Fact]
    public void FilterTerms_SearchParamNotNullCharacterRemoved_ClassesCountEqualTwo()
    {
        AllTermsViewModel vm = new(null)
        {
            SearchParams = "testTerm",
            ClassesSourceOfTruth = [new ClassGroup(new Term() { TermName = "testTermName" }, [new Class() { ClassName = "Doesnt Contain Search Param" }]),
                new ClassGroup(new Term() { TermName = "Doesnt Contain Search Param" }, [new Class() { ClassName = "anotherTeSt" }]),
                new ClassGroup(new Term() { TermName = "Doesnt Contain Search Param" }, [new Class() { ClassName = "Doesnt Contain Search Param" }])]
        };
        vm.Classes.Clear();
        vm.FilterTermsCommand.Execute(null);
        vm.SearchParams = "test";
        vm.FilterTermsCommand.Execute(null);

        Assert.Equal(2, vm.Classes.Count);
    }
}
