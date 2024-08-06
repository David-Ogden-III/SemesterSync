using SQLite;

namespace ModelLibrary;


[Table("TermSchedule")]
public class TermSchedule
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Indexed(Name = "TermScheduleUnique", Order = 2, Unique = true)]
    public int TermId { get; set; }

    [Indexed(Name = "TermScheduleUnique", Order = 1, Unique = true)]
    public int ClassId { get; set; }

    [Indexed, Column("createdBy")]
    public string CreatedBy { get; set; }
}
