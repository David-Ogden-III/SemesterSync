﻿using CommunityToolkit.Maui.Alerts;
using ModelLibrary;
using ServiceLibrary;
using System.ComponentModel;

namespace ViewModelLibrary;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly AuthService authService = AuthService.GetInstance();
    public LoginViewModel()
    {
        LoadCommand = new Command(() => Load());
        ChangeSelectedOperationCommand = new Command(() => ChangeSelectedOperation());
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
    public Command LoadCommand { get; }
    public Command ChangeSelectedOperationCommand { get; }

    public async Task<bool> Submit()
    {
        bool inputsContainError = await ValidateInputs();
        if (inputsContainError) return false;

        string enteredEmail = Email.ToLower();
        string enteredPassword = Password;
        UserDTO enteredDTO = new(enteredEmail, enteredPassword);

        bool loginSuccess = false;
        bool registerSuccess = false;

        if (!RegisterSelected)
        {
            loginSuccess = await authService.AuthenticateUser(enteredDTO);
        }
        else
        {
            enteredDTO.Email = enteredDTO.Email.Trim();
            enteredDTO.Password = ConfirmPassword;
            enteredDTO.FirstName = FirstName;
            enteredDTO.LastName = LastName;
            enteredDTO.PhoneNumber = PhoneNumber;
            enteredDTO.Major = Major;
            enteredDTO.GraduationDate = GraduationDate.ToUniversalTime();

            registerSuccess = await authService.CreateUser(enteredDTO);
        }

        if (loginSuccess || registerSuccess)
        {
            await authService.AddUserEmailToSecureStorage(enteredDTO.Email);
            ClearInputs();
            if (RegisterSelected) ChangeSelectedOperationCommand.Execute(null);
        }
        else if (!loginSuccess)
        {
            string toastText = "Incorrect Username or Password";
            var toast = Toast.Make(toastText);

            await toast.Show(cancellationTokenSource.Token);
        }
        else if (!registerSuccess)
        {
            string toastText = "There was an issue creating your account";
            var toast = Toast.Make(toastText);

            await toast.Show(cancellationTokenSource.Token);
        }
        return loginSuccess || registerSuccess;
    }

    private void Load()
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

    private void ClearInputs()
    {
        Email = String.Empty;
        Password = String.Empty;
        FirstName = String.Empty;
        LastName = String.Empty;
        PhoneNumber = String.Empty;
        Major = String.Empty;
        ConfirmPassword = String.Empty;
        GraduationDate = DateTime.Now;
    }

    public async Task<bool> ValidateInputs()
    {
        string toastText = "All Fields Required";
        bool inputsContainError = false;
        if (Email.Length == 0) inputsContainError = true;
        if (Password.Length == 0) inputsContainError = true;

        if (RegisterSelected)
        {
            if (ConfirmPassword.Length == 0) inputsContainError = true;
            if (FirstName.Length == 0) inputsContainError = true;
            if (LastName.Length == 0) inputsContainError = true;
            if (PhoneNumber.Length == 0) inputsContainError = true;
            if (Major.Length == 0) inputsContainError = true;


            if (Password != ConfirmPassword && inputsContainError == false)
            {
                inputsContainError = true;
                toastText = "Passwords Do Not Match";
            }
        }

        if (inputsContainError)
        {
            var toast = Toast.Make(toastText);

            await toast.Show(cancellationTokenSource.Token);
        }

        return inputsContainError;
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