using System.ComponentModel;
using System.Diagnostics;
using C971_Ogden.Database;

namespace C971_Ogden.ViewModel;
[QueryProperty("SelectedTerm", "SelectedTerm")]
public class TermDetailsViewModel : INotifyPropertyChanged
{
    Term? selectedTerm = null;

    public Term? SelectedTerm
    {
        get => selectedTerm;
        set
        {
            selectedTerm = value;
            OnPropertyChanged(nameof(SelectedTerm));
        }
    }

    public Command CancelClickedCommand { get; set; } = new(
        execute: async () =>
        {
            Debug.WriteLine("I was clicked");
            await Shell.Current.GoToAsync("..");
        });

    public Command SaveClickedCommand { get; set; } = new(
        execute: async () =>
        {
            Debug.WriteLine("I was clicked");
            await Shell.Current.GoToAsync("..");
        });

    public Command RemoveClassCommand { get; set; } = new(
        execute: (object classToDelete) =>
        {
            Class classs = (Class)classToDelete;
            Debug.WriteLine(classs.ClassName);
        });



    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
