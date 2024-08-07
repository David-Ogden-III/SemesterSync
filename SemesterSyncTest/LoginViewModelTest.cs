using ViewModelLibrary;

namespace SemesterSyncTest;

public class LoginViewModelTest
{
    // ValidateInputs Method Tests
    [Fact]
    public async Task ValidateInputs_AllPropsHaveData_RegisterSelectedTrue_ReturnsFalse()
    {
        LoginViewModel vm = new()
        {
            RegisterSelected = true,

            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            Password = "password",
            ConfirmPassword = "password",
            PhoneNumber = "1234567890",
            Major = "Computer Science"
        };

        bool inputsContainError = await vm.ValidateInputs();

        Assert.False(inputsContainError);
    }

    [Fact]
    public async Task ValidateInputs_EmailPasswordHaveData_RegisterSelectedFalse_ReturnsFalse()
    {
        LoginViewModel vm = new()
        {
            RegisterSelected = false,

            Email = "johndoe@email.com",
            Password = "password"
        };

        bool inputsContainError = await vm.ValidateInputs();

        Assert.False(inputsContainError);
    }

    [Fact]
    public async Task ValidateInputs_PasswordsDontMatch_RegisterSelectedTrue_ReturnsTrue()
    {
        LoginViewModel vm = new()
        {
            RegisterSelected = true,

            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            Password = "password",
            ConfirmPassword = "drowssap",
            PhoneNumber = "1234567890",
            Major = "Computer Science"
        };

        bool inputsContainError = await vm.ValidateInputs();

        Assert.True(inputsContainError);
    }

    [Fact]
    public async Task ValidateInputs_EmailNull_RegisterSelectedFalse_ReturnsTrue()
    {
        LoginViewModel vm = new()
        {
            RegisterSelected = false,

            FirstName = "John",
            LastName = "Doe",
            Email = String.Empty,
            Password = "password",
            ConfirmPassword = "drowssap",
            PhoneNumber = "1234567890",
            Major = "Computer Science"
        };

        bool inputsContainError = await vm.ValidateInputs();

        Assert.True(inputsContainError);
    }


    // ChangeSelectedOperation Command Tests
    [Fact]
    public void ChangeSelectedOperation_RegisterSelectedStartsFalse_EndsTrue()
    {
        LoginViewModel vm = new()
        {
            RegisterSelected = false
        };

        vm.ChangeSelectedOperationCommand.Execute(null);

        Assert.True(vm.RegisterSelected);
    }

    [Fact]
    public void ChangeSelectedOperation_SelectorTextStartsRegister_EndsLogin()
    {
        LoginViewModel vm = new()
        {
            RegisterSelected = false,
            SelectorText = "Register"
        };

        vm.ChangeSelectedOperationCommand.Execute(null);

        Assert.Equal("Login", vm.SelectorText);
    }

    [Fact]
    public void ChangeSelectedOperation_ActionLabelStartsRegister_EndsLogin()
    {
        LoginViewModel vm = new()
        {
            RegisterSelected = false,
            ActionLabel = "Login"
        };

        vm.ChangeSelectedOperationCommand.Execute(null);

        Assert.Equal("Register", vm.ActionLabel);
    }

    [Fact]
    public void ChangeSelectedOperation_RegisterSelectedStartTrue_EndsFalse()
    {
        LoginViewModel vm = new()
        {
            RegisterSelected = true
        };

        vm.ChangeSelectedOperationCommand.Execute(null);

        Assert.False(vm.RegisterSelected);
    }

    [Fact]
    public void ChangeSelectedOperation_ActionLabelStartsLogin_EndsRegister()
    {
        LoginViewModel vm = new()
        {
            RegisterSelected = true,
            ActionLabel = "Register"
        };

        vm.ChangeSelectedOperationCommand.Execute(null);

        Assert.Equal("Login", vm.ActionLabel);
    }

    [Fact]
    public void ChangeSelectedOperation_SelectorTextStartsLogin_EndsRegister()
    {
        LoginViewModel vm = new()
        {
            RegisterSelected = true,
            SelectorText = "Login"
        };

        vm.ChangeSelectedOperationCommand.Execute(null);

        Assert.Equal("Register", vm.SelectorText);
    }

    //[Fact]
    //public async Task Submit_InputsValid_ReturnTrue()
    //{
    //    LoginViewModel vm = new()
    //    {
    //        RegisterSelected = true,

    //        FirstName = "John",
    //        LastName = "Doe",
    //        Email = "johndoe@email.com",
    //        Password = "password",
    //        ConfirmPassword = "password",
    //        PhoneNumber = "1234567890",
    //        Major = "Computer Science"
    //    };

    //    bool newUserCreated = await vm.Submit();

    //    Assert.True(newUserCreated);
    }
}
