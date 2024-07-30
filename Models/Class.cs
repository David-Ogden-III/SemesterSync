using SQLite;

namespace SemesterSync.Models;

[Table("Classes")]
public class Class
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Column("className")]
    public string ClassName { get; set; }

    [Column("status")]
    public string Status { get; set; }

    [Column("startDate")]
    public DateTime StartDate { get; set; }


    [Column("endDate")]

    public DateTime EndDate { get; set; }


    [Column("notes")]
    public string Notes { get; set; }

    [Column("instructorId"), Indexed]
    public int InstructorId { get; set; }

    [Indexed, Column("createdBy")]
    public string CreatedBy { get; set; }
}
