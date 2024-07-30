using SQLite;

namespace SemesterSync.Models;


public class DetailedExam(Exam exam, string examType)
{
    public int ExamId { get; set; } = exam.Id;
    public string ExamName { get; set; } = exam.ExamName;
    public DateTime StartTime { get; set; } = exam.StartTime;
    public DateTime EndTime { get; set; } = exam.EndTime;
    public int ClassId { get; set; } = exam.ClassId;
    public int ExamTypeId { get; set; } = exam.ExamTypeId;
    public string ExamType { get; set; } = examType;
    public string CreatedBy { get; set; } = exam.CreatedBy;
}
