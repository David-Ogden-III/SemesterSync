using ModelLibrary;
using System.Security.Cryptography;
using System.Text;

namespace ServiceLibrary;

public abstract class AuthServiceTemplate
{
    public async Task<bool> AuthenticateUser(UserDTO userDTO)
    {
        User storedUser = await GetUser(userDTO);
        if (storedUser == null) return false;

        byte[] storedPwHash = storedUser.PasswordHash;
        byte[] storedSalt = storedUser.Salt;

        string enteredPassword = userDTO.Password;
        byte[] enteredPasswordHash = HashPassword(enteredPassword, storedSalt);

        if (storedPwHash.Length != enteredPasswordHash.Length) return false;

        for (int i = 0; i < storedPwHash.Length; i++)
        {
            if (storedPwHash[i] != enteredPasswordHash[i]) return false;
        }

        return true;
    }

    public async Task<bool> CreateUser(UserDTO userDTO)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(128 / 8);
        string plainTextPassword = userDTO.Password;

        byte[] hashedPassword = HashPassword(plainTextPassword, saltBytes);

        User user = new(userDTO)
        {
            DateCreated = DateTime.UtcNow,
            PasswordHash = hashedPassword,
            Salt = saltBytes,
            DateLastModified = DateTime.UtcNow,
        };

        bool userCreated = await AddUserToDB(user);

        return userCreated;
    }

    private static byte[] HashPassword(string plainTextPassword, byte[] salt)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(plainTextPassword);
        byte[] saltedPassword = [.. passwordBytes, .. salt];
        byte[] hashedPassword = SHA256.HashData(saltedPassword);

        return hashedPassword;
    }

    public async Task<string?> RetrieveUserEmailFromSecureStorage()
    {
        string? userEmail = await SecureStorage.GetAsync("activeUser");

        return userEmail;
    }

    public async Task AddUserEmailToSecureStorage(string userEmail)
    {
        await SecureStorage.SetAsync("activeUser", userEmail);
    }

    protected abstract Task<User> GetUser(UserDTO userDTO);

    protected abstract Task<bool> AddUserToDB(User newUser);
}
