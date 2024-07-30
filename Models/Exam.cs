using SQLite;

namespace SemesterSync.Models;


[Table("Exams")]
public class Exam
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Column("examName")]
    public string ExamName { get; set; }


    [Column("startTime")]
    public DateTime StartTime { get; set; }

    [Column("endTime")]
    public DateTime EndTime { get; set; }

    [Column("classId"), NotNull, Indexed]
    public int ClassId { get; set; }

    [Column("examTypeId"), NotNull, Indexed]
    public int ExamTypeId { get; set; }

    [Indexed, Column("createdBy")]
    public string CreatedBy { get; set; }
}
