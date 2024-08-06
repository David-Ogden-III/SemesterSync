namespace ModelLibrary;

public class UserDTO
{
    public UserDTO(string firstName, string lastName, string email, string phoneNumber, string major, DateTime graduationDate, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Major = major;
        GraduationDate = graduationDate;
        Password = password;
    }
    public UserDTO(string email, string password)
    {
        Email = email;
        Password = password;
    }
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string PhoneNumber { get; set; } = String.Empty;
    public string Major { get; set; } = String.Empty;
    public DateTime GraduationDate { get; set; }
    public string Password { get; set; } = String.Empty;
}
