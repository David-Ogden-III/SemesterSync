using System.Collections.ObjectModel;

namespace SemesterSync.Models;


public class ClassGroup(Term term, ObservableCollection<Class> classes) : ObservableCollection<Class>(classes)
{
    public Term Term { get; set; } = term;
}
