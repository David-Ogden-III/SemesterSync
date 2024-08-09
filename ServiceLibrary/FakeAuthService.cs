using DataLibrary;
using ModelLibrary;

namespace ServiceLibrary;

public class FakeAuthService : AuthServiceTemplate
{
    private FakeAuthService() { }

    private static FakeAuthService? _instance;

    private static readonly object _instanceLock = new();

    public static FakeAuthService GetInstance()
    {
        if (_instance == null)
        {
            lock (_instanceLock)
            {
                _instance ??= new FakeAuthService();
            }
        }
        return _instance;
    }

    protected override async Task<User> GetUser(UserDTO userDTO)
    {
        User user = await MockDbContext.GetFilteredItemAsync<User>(user => user.Email == userDTO.Email);

        return user;
    }

    protected override async Task<bool> AddUserToDB(User newUser)
    {
        bool userCreated = await MockDbContext.AddItemAsync(newUser);
        return userCreated;
    }
}
