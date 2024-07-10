using System.ComponentModel;
using System.Diagnostics;
using C971_Ogden.Database;

namespace C971_Ogden.ViewModel;
[QueryProperty(nameof(SelectedCG), "SelectedCG")]
public class TermDetailsViewModel : INotifyPropertyChanged
{
    public TermDetailsViewModel()
    {
        RemoveClassCommand = new Command<Class>(execute: async (Class classToDelete) => await RemoveClass(classToDelete));
    }
    public Command<Class> RemoveClassCommand { get; }



    ClassGroup? _selectedCG;

    public ClassGroup? SelectedCG
    {
        get => _selectedCG;
        set
        {
            _selectedCG = value;
            OnPropertyChanged(nameof(SelectedCG));
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

    private async Task RemoveClass(Class classToDelete)
    {
        TermSchedule ts = await SchoolDatabase.GetFilteredItemAsync<TermSchedule>((ts) => ts.ClassId == classToDelete.Id);

        bool rowDeleted = await SchoolDatabase.DeleteItemAsync(ts);

        if (rowDeleted)
            SelectedCG.Remove(classToDelete);
    }



    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
