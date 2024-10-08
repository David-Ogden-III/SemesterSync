﻿using DataLibrary;
using ModelLibrary;

namespace ServiceLibrary;

public class AuthService : AuthServiceTemplate
{
    private AuthService() { }

    private static AuthService? _instance;

    private static readonly object _instanceLock = new();

    public static AuthService GetInstance()
    {
        if (_instance == null)
        {
            lock (_instanceLock)
            {
                _instance ??= new AuthService();
            }
        }
        return _instance;
    }

    protected override async Task<User> GetUser(UserDTO userDTO)
    {
        User user = await DbContext.GetFilteredItemAsync<User>(user => user.Email == userDTO.Email);

        return user;
    }

    protected override async Task<bool> AddUserToDB(User newUser)
    {
        bool userCreated = await DbContext.AddItemAsync(newUser);
        return userCreated;
    }
}