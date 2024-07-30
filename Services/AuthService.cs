using System.Data;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SemesterSync.Data;
using SemesterSync.Models;

namespace SemesterSync.Services;

public static class AuthService
{

    private static byte[] GenerateSalt()
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(128/8);

        return saltBytes;
    }

    private static byte[] HashPassword(string plainTextPassword, byte[] salt)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(plainTextPassword);
        byte[] saltedPassword = passwordBytes.Concat(salt).ToArray();
        byte[] hashedPassword = SHA256.HashData(saltedPassword);
            
        return hashedPassword;
    }

    public static async Task<bool> CreateUser(UserDTO userDTO)
    {
        byte[] saltBytes = GenerateSalt();
        string plainTextPassword = userDTO.Password;

        byte[] hashedPassword = HashPassword(plainTextPassword, saltBytes);

        User user = new(userDTO)
        {
            DateCreated = DateTime.UtcNow,
            PasswordHash = hashedPassword,
            Salt = saltBytes,
            DateLastModified = DateTime.UtcNow,
        };

        bool userCreated = await DbContext.AddItemAsync(user);

        if (userCreated)
        {
            await AddUserToSecureStorage(user);
        }

        return userCreated;
    }

    public static async Task<bool> AuthenticateUser(UserDTO userDTO)
    {
        User storedUser = await DbContext.GetFilteredItemAsync<User>(user => user.Email == userDTO.Email);
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

        await AddUserToSecureStorage(storedUser);
        
        return true;
    }

    public static async Task AddUserToSecureStorage(User user)
    {
        string userEmail = user.Email;
        await SecureStorage.SetAsync("activeUser", userEmail);
    }

    public static async Task<string?> RetrieveUserFromSecureStorage()
    {
        string? userEmail = await SecureStorage.GetAsync("activeUser");

        return userEmail;
    }
}
