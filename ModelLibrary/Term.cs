using SQLite;

namespace ModelLibrary;

[Table("Terms")]
public class Term
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Column("termName")]
    public string TermName { get; set; }

    [Column("startDate")]
    public DateTime StartDate { get; set; }

    [Column("endDate")]
    public DateTime EndDate { get; set; }

    [Indexed, Column("createdBy")]
    public string CreatedBy { get; set; }
}
