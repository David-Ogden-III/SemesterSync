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
        LoginCommand = new Command(async () => await Login());
        LoadCommand = new Command(async () => await Load());
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

    public bool IsLoggedIn { get; set; } = false;

    public Command LoginCommand { get; }
    public Command LoadCommand { get; }

    private async Task Login()
    {
        string enteredEmail = Email.ToLower();
        string enteredPassword = Password;
        UserDTO enteredDTO = new(enteredEmail, enteredPassword);

        bool loginSuccess = await AuthService.AuthenticateUser(enteredDTO);

        if (loginSuccess)
        {
            await Shell.Current.GoToAsync($"//{nameof(ActiveTerm)}");
        }
    }

    private async Task Load()
    {

    }



    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



    private string _email;
    private string _password;
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