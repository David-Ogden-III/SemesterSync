using SQLite;

namespace ModelLibrary;


[Table("ExamTypes")]
public class ExamType
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Column("examType"), Unique, NotNull]
    public string Type { get; set; }
}
