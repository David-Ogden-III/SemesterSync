using C971_Ogden.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using C971_Ogden.Database;

namespace C971_Ogden.ViewModel;

public class AllTermsViewModel : INotifyPropertyChanged
{
    private static SchoolDatabase Db { get; set; } = new();
    // public ObservableCollection<Term> AllTerms { get; set; } = 

    public Command AddTermCommand { get; set; } = new(
        execute: async () =>
    {
        MockData.CreateAllMockData();
        await Shell.Current.GoToAsync(nameof(TermDetails));
    });

    public Command<Term> EditTermCommand { get; set; } = new(
        execute: async (Term selectedTerm) =>
    {
        await Shell.Current.GoToAsync(nameof(TermDetails),
            new Dictionary<string,object>
            {
                {"SelectedTerm", selectedTerm }
            });
    });


    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}