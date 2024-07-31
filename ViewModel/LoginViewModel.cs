using CommunityToolkit.Maui.Alerts;
using Plugin.Maui.Biometric;
using SemesterSync.Models;
using SemesterSync.Services;
using SemesterSync.Views;
using System.ComponentModel;

namespace SemesterSync.ViewModel;

public class LoginViewModel : INotifyPropertyChanged
{
    private IBiometric biometric;
    public LoginViewModel(IBiometric bio)
    {
        SubmitCommand = new Command(async () => await Submit());
        LoadCommand = new Command(async () => await Load());
        ChangeSelectedOperationCommand = new Command(() => ChangeSelectedOperation());
        biometric = bio;
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged(nameof(ConfirmPassword));
        }
    }
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
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
            _graduationDate = DateTime.SpecifyKind(value, DateTimeKind.Local);
            OnPropertyChanged(nameof(GraduationDate));
        }
    }

    public string SelectorText
    {
        get => _selectorText;
        set
        {
            _selectorText = value;
            OnPropertyChanged(nameof(SelectorText));
        }
    }
    public string ActionLabel
    {
        get => _actionLabel;
        set
        {
            _actionLabel = value;
            OnPropertyChanged(nameof(ActionLabel));
        }
    }
    public bool RegisterSelected
    {
        get => _registerSelected;
        set
        {
            _registerSelected = value;
            OnPropertyChanged(nameof(RegisterSelected));
        }
    }


    public Command SubmitCommand { get; }
    public Command LoadCommand { get; }
    public Command ChangeSelectedOperationCommand { get; }

    private async Task Submit()
    {
        string errorMessage = ValidateInputs();
        if (!string.IsNullOrWhiteSpace(errorMessage))
        {
            var toast = Toast.Make(errorMessage);

            await toast.Show(cancellationTokenSource.Token);

            return;
        }

        string enteredEmail = Email.ToLower();
        string enteredPassword = Password;
        UserDTO enteredDTO = new(enteredEmail, enteredPassword);

        bool loginSuccess = false;
        bool registerSuccess = false;

        if (!RegisterSelected)
        {
            loginSuccess = await AuthService.AuthenticateUser(enteredDTO);
        }
        else
        {
            enteredDTO.Password = ConfirmPassword;
            enteredDTO.FirstName = FirstName;
            enteredDTO.LastName = LastName;
            enteredDTO.PhoneNumber = PhoneNumber;
            enteredDTO.Major = Major;
            enteredDTO.GraduationDate = GraduationDate.ToUniversalTime();
            registerSuccess = await AuthService.CreateUser(enteredDTO);
        }

        if (loginSuccess || registerSuccess)
        {
            await Shell.Current.GoToAsync($"//{nameof(ActiveTerm)}");
        }
    }

    private async Task Load()
    {

    }

    private void ChangeSelectedOperation()
    {
        if (RegisterSelected)
        {
            RegisterSelected = false;
            ActionLabel = "Login";
            SelectorText = "Register";
        }
        else
        {
            RegisterSelected = true;
            ActionLabel = "Register";
            SelectorText = "Login";
        }
    }

    private string ValidateInputs()
    {
        string toastText = string.Empty;
        string requiredField = "All Fields Required";
        if (Email.Length == 0) toastText = requiredField;
        if (Password.Length == 0) toastText = requiredField;

        if (RegisterSelected)
        {
            if (ConfirmPassword.Length == 0) toastText = requiredField;
            if (FirstName.Length == 0) toastText = requiredField;
            if (LastName.Length == 0) toastText = requiredField;
            if (PhoneNumber.Length == 0) toastText = requiredField;
            if (Major.Length == 0) toastText = requiredField;


            if (Password != ConfirmPassword && toastText.Length == 0) toastText = "Password Does Not Match";
        }
        return toastText;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



    private string _email = String.Empty;
    private string _password = String.Empty;
    private string _confirmPassword = String.Empty;
    private string _firstName = String.Empty;
    private string _lastName = String.Empty;
    private string _phoneNumber = String.Empty;
    private string _major = String.Empty;
    private DateTime _graduationDate = DateTime.Now;

    private string _selectorText = "Register";
    private string _actionLabel = "Login";
    private bool _registerSelected = false;
    CancellationTokenSource cancellationTokenSource = new();
}




//    private async Task Load()
//    {
//        bool deviceHasPin = true;

//#if ANDROID23_0_OR_GREATER
//        var kg = Context.KeyguardService;
//        if (MauiApplication.Current.GetSystemService(kg) is KeyguardManager keyGuardService)
//        {
//            deviceHasPin = keyGuardService.IsDeviceSecure;
//        }
//#endif

//        if (!deviceHasPin || IsLoggedIn)
//        {
//            await Shell.Current.GoToAsync($"//{nameof(ActiveTerm)}");
//        }

//        List<BiometricType> types = await biometric.GetEnrolledBiometricTypesAsync();

//        if (types.Contains(BiometricType.Face))
//        {

//        }
//        else if (types.Contains(BiometricType.Fingerprint))
//        {
//            LoginImage = "fingerprint.png";
//        }
//        else
//        {
//            LoginImage = "keypad.png";
//        }



//    }
//    public ImageSource LoginImage
//    {
//        get => _loginImage;
//        set
//        {
//            _loginImage = value;
//            OnPropertyChanged(nameof(LoginImage));
//        }
//    }

//    private ImageSource _loginImage = "face_id.png";

//var result = await biometric.AuthenticateAsync(new
//            AuthenticationRequest()
//{
//    Title = "Login",
//    NegativeText = "Cancel",
//    AllowPasswordAuth = true
//}, CancellationToken.None);


//        if (result.Status == BiometricResponseStatus.Success)
//        {
//            IsLoggedIn = true;
//            await Shell.Current.GoToAsync($"//{nameof(ActiveTerm)}");
//        }