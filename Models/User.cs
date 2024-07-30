using SQLite;

namespace SemesterSync.Models;

[Table("Users")]
public class User
{
    [PrimaryKey, Column("email")]
    public string Email { get; set; }

    [Column("firstName")]
    public string FirstName { get; set; }

    [Column("lastName")]
    public string LastName { get; set; }

    [Column("phoneNumber")]
    public string PhoneNumber { get; set; }

    [Column("major")]
    public string Major {  get; set; }

    [Column("salt")]
    public byte[] Salt { get; set; }

    [Column("passwordHash")]
    public byte[] PasswordHash { get; set; }

    [Column("graduationDate")]
    public DateTime GraduationDate { get; set; }

    [Column("dateCreated")]
    public DateTime DateCreated { get; set; }

    [Column("dateLastModified")]
    public DateTime DateLastModified { get; set; }



    public User (UserDTO userDTO)
    {
        Email = userDTO.Email;
        FirstName = userDTO.FirstName;
        LastName = userDTO.LastName;
        PhoneNumber = userDTO.PhoneNumber;
        Major = userDTO.Major;
        GraduationDate = userDTO.GraduationDate;
    }
    public User(string email, string firstName, string lastName, string phoneNumber, string major, byte[] salt, byte[] passwordHash, DateTime graduationDate, DateTime dateCreated, DateTime dateLastModified)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Major = major;
        Salt = salt;
        PasswordHash = passwordHash;
        GraduationDate = graduationDate;
        DateCreated = dateCreated;
        DateLastModified = dateLastModified;
    }

    public User()
    { }
}