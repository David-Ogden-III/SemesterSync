using CommunityToolkit.Maui.Alerts;
using DataLibrary;
using ModelLibrary;
using ServiceLibrary;
using System.ComponentModel;
using System.Diagnostics;

namespace ViewModelLibrary;

public class ProfileViewModel : INotifyPropertyChanged
{
    private readonly AuthService authService = AuthService.GetInstance();
    public ProfileViewModel()
    {
        LoadCommand = new Command(execute: async () => await Load());
        SaveUserCommand = new Command(execute: async () => await SaveUser());
    }

    public string? ActiveUserEmail
    {
        get => _activeUserEmail;
        set
        {
            _activeUserEmail = value;
            OnPropertyChanged(nameof(ActiveUserEmail));
        }
    }
    public User ActiveUser { get; set; }
    public string FirstName
    {
        get => _fName;
        set
        {
            _fName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }
    public string LastName
    {
        get => _lName;
        set
        {
            _lName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            OnPropertyChanged(nameof(PhoneNumber));
        }
    }
    public string Major
    {
        get => _major;
        set
        {
            _major = value;
            OnPropertyChanged(nameof(Major));
        }
    }
    public DateTime GraduationDate
    {
        get => _graduationDate;
        set
        {
            if (value.Kind == DateTimeKind.Utc)
            {
                _graduationDate = value.ToLocalTime();
            }
            else if (value.Kind == DateTimeKind.Unspecified)
            {
                value = DateTime.SpecifyKind(value, DateTimeKind.Local);
                _graduationDate = value;
            }
            else _graduationDate = value;
            OnPropertyChanged(nameof(GraduationDate));
        }
    }





    // Commands
    public Command LoadCommand { get; }
    public Command SaveUserCommand { get; }


    // Command Definitions
    private async Task Load()
    {
        ActiveUserEmail = await authService.RetrieveUserEmailFromSecureStorage();
        ActiveUser = await DbContext.GetFilteredItemAsync<User>(user => user.Email == ActiveUserEmail);

        FirstName = ActiveUser.FirstName;
        LastName = ActiveUser.LastName;
        PhoneNumber = ActiveUser.PhoneNumber;
        Major = ActiveUser.Major;
        GraduationDate = DateTime.SpecifyKind(ActiveUser.GraduationDate, DateTimeKind.Utc);
    }

    private async Task SaveUser()
    {
        bool inputsContainErrors = await ValidateInputs();

        if (inputsContainErrors) return;

        ActiveUser.FirstName = FirstName;
        ActiveUser.LastName = LastName;
        ActiveUser.PhoneNumber = PhoneNumber;
        ActiveUser.Major = Major;
        ActiveUser.GraduationDate = GraduationDate.ToUniversalTime();
        ActiveUser.DateLastModified = DateTime.UtcNow;

        bool success = await DbContext.UpdateItemAsync(ActiveUser);
        Debug.WriteLineIf(success, "User Updated");
    }

    private async Task<bool> ValidateInputs()
    {
        bool inputsContainErrors = false;

        if (String.IsNullOrWhiteSpace(FirstName)) inputsContainErrors = true;
        if (String.IsNullOrWhiteSpace(LastName)) inputsContainErrors = true;
        if (String.IsNullOrWhiteSpace(PhoneNumber)) inputsContainErrors = true;
        if (String.IsNullOrWhiteSpace(Major)) inputsContainErrors = true;

        if (inputsContainErrors)
        {
            var toast = Toast.Make("All Fields Required");

            await toast.Show(cancellationTokenSource.Token);
        }
        return inputsContainErrors;
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    // Private Fields
    private string _fName = string.Empty;
    private string _lName = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _major = string.Empty;
    private DateTime _graduationDate = DateTime.Now;
    private string? _activeUserEmail = String.Empty;

    readonly CancellationTokenSource cancellationTokenSource = new();
}
