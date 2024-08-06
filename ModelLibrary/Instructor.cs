using SQLite;

namespace ModelLibrary;


[Table("Instructors")]
public class Instructor
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Column("instructorName")]
    public string InstructorName { get; set; }

    [Column("phoneNumber")]
    public string PhoneNumber { get; set; }

    [Column("Email")]
    public string Email { get; set; }

    [Indexed, Column("createdBy")]
    public string CreatedBy { get; set; }
}
