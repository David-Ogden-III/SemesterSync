using SQLite;
using System.Collections.ObjectModel;

namespace C971_Ogden.Database;

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
}

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

    [Column("instructorId"),Indexed]
    public int InstructorId { get; set; }
}

[Table("TermSchedule")]
public class TermSchedule
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Indexed(Name = "TermScheduleUnique", Order = 2, Unique = true)]
    public int TermId { get; set; }

    [Indexed(Name = "TermScheduleUnique", Order = 1, Unique = true)]
    public int ClassId { get; set; }
}

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
}

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
}

[Table("ExamTypes")]
public class ExamType
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Column("examType"), Unique, NotNull]
    public string Type { get; set; }
}


// Non Table Helper Classes
public class ClassGroup(Term term, ObservableCollection<Class> classes) : ObservableCollection<Class>(classes)
{
    public Term Term { get; set; } = term;
}

public class DetailedExam(Exam exam, string examType)
{
    public int ExamId { get; set; } = exam.Id;
    public string ExamName { get; set; } = exam.ExamName;
    public DateTime StartTime { get; set; } = exam.StartTime;
    public DateTime EndTime { get; set; } = exam.EndTime;
    public int ClassId { get; set; } = exam.ClassId;
    public int ExamTypeId { get; set; } = exam.ExamTypeId;
    public string ExamType { get; set; } = examType;
}