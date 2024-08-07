using System.Collections.ObjectModel;

namespace ModelLibrary;

public class ReportGroup(string status, ObservableCollection<Class> classes) : ObservableCollection<Class>(classes)
{
    public string Status { get; set; } = status;
}
