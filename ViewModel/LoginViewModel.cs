using Android.App;
using Android.Content;
using SemesterSync.Database;
using SemesterSync.Views;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using Plugin.Maui.Biometric;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

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

    public bool IsLoggedIn { get; set; } = false;

    public Command LoginCommand { get; }
    public Command LoadCommand { get; }

    private async Task Login()
    {
        //SHA256 algorithm = SHA256.Create();

        //string pwS = "password123";
        //string pwTest = "password123";
        //byte[] pw = Encoding.UTF8.GetBytes(pwS);
        //byte[] pwT = Encoding.UTF8.GetBytes(pwTest);
        //byte[] salty = RandomNumberGenerator.GetBytes(128 / 8);

        //byte[] pwAndSalt = pw.Concat(salty).ToArray();
        //byte[] pwAndSalt2 = pwT.Concat(salty).ToArray();
        //byte[] hashed = SHA256.HashData(pwAndSalt);
        //byte[] hashed2 = SHA256.HashData(pwAndSalt2);

        //Debug.WriteLine(Convert.ToBase64String(hashed));
        //Debug.WriteLine(Convert.ToBase64String(pwAndSalt));
        //Debug.WriteLine(Convert.ToBase64String(salty));
        //if (hashed.Length != hashed2.Length)
        //{
        //    Debug.WriteLine("Length does not match");
        //    return;
        //}
        //for (int i = 0; i < hashed.Length; i++)
        //{
        //    if (hashed[i] != hashed2[i])
        //    {
        //        Debug.WriteLine("Do not equal");
        //        return;
        //    }
        //}
        //Debug.WriteLine("passwords match");
        //return;

        var result = await biometric.AuthenticateAsync(new
            AuthenticationRequest()
        {
            Title = "Login",
            NegativeText = "Cancel",
            AllowPasswordAuth = true
        }, CancellationToken.None);


        if (result.Status == BiometricResponseStatus.Success)
        {
            IsLoggedIn = true;
            await Shell.Current.GoToAsync($"//{nameof(ActiveTerm)}");
        }
    }

    private async Task Load()
    {
        bool deviceHasPin = true;

#if ANDROID23_0_OR_GREATER
        var kg = Context.KeyguardService;
        if (MauiApplication.Current.GetSystemService(kg) is KeyguardManager keyGuardService)
        {
            deviceHasPin = keyGuardService.IsDeviceSecure;
        }
#endif

        if (!deviceHasPin || IsLoggedIn)
        {
            await Shell.Current.GoToAsync($"//{nameof(ActiveTerm)}");
        }

        List<BiometricType> types = await biometric.GetEnrolledBiometricTypesAsync();

        if (types.Contains(BiometricType.Face))
        {

        }
        else if (types.Contains(BiometricType.Fingerprint))
        {
            LoginImage = "fingerprint.png";
        }
        else
        {
            LoginImage = "keypad.png";
        }



    }





    public ImageSource LoginImage
    {
        get => _loginImage;
        set
        {
            _loginImage = value;
            OnPropertyChanged(nameof(LoginImage));
        }
    }

    private ImageSource _loginImage = "face_id.png";


    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
