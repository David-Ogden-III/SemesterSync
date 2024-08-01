using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using SemesterSync.Data;
using SemesterSync.Models;
using SemesterSync.Services;
using SemesterSync.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace SemesterSync.ViewModel;

public class ProfileViewModel : INotifyPropertyChanged
{
    private string? activeUserEmail;

    public ProfileViewModel()
    {
        activeUserEmail = Task.Run(() => AuthService.RetrieveUserEmailFromSecureStorage()).Result;
    }













    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
