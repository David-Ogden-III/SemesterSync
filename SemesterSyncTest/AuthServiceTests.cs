using DataLibrary;
using ModelLibrary;
using ServiceLibrary;

namespace SemesterSyncTest;

public class AuthServiceTests
{
    [Fact]
    public async Task CreateUser_UserDTOValid_ReturnUser()
    {
        MockDbContext.db = null;
        string testEmail = "TestEmail@email.com";
        string testPassword = "password";
        UserDTO userDTO = new(testEmail, testPassword);
        FakeAuthService fakeAuthService = FakeAuthService.GetInstance();


        bool userCreated = await fakeAuthService.CreateUser(userDTO);


        Assert.True(userCreated);

        await MockDbContext.db.CloseAsync();
    }

    [Fact]
    public async Task AuthenticateUser_UserDTOValid_ReturnTrue()
    {
        MockDbContext.db = null;
        string testEmail = "TestEmail@email.com";
        string testPassword = "password";
        UserDTO userDTO = new(testEmail, testPassword);
        FakeAuthService mockAuthService = FakeAuthService.GetInstance();
        await mockAuthService.CreateUser(userDTO);

        bool userAuthenticated = await mockAuthService.AuthenticateUser(userDTO);

        Assert.True(userAuthenticated);

        await MockDbContext.db.CloseAsync();
    }
}
