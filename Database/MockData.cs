using System.Diagnostics;

namespace C971_Ogden.Database;

public static class MockData
{
    private static SchoolDatabase Db { get; set; } = new();


    private static Term Term1 { get; set; } = new();
    private static Term Term2 { get; set; } = new();

    private static Class Class1 { get; set; } = new();
    private static Class Class2 { get; set; } = new();
    private static Class Class3 { get; set; } = new();
    private static Class Class4 { get; set; } = new();
    private static Class Class5 { get; set; } = new();
    private static Class Class6 { get; set; } = new();
    private static Class Class7 { get; set; } = new();
    private static Class Class8 { get; set; } = new();
    private static Class Class9 { get; set; } = new();
    private static Class Class10 { get; set; } = new();
    private static Class Class11 { get; set; } = new();
    private static Class Class12 { get; set; } = new();
    private static List<string> ClassNames { get; set; } = ["US History", "Chemistry", "Biology", "Health", "Communications", "Art", "Trigenometry", "Algebra", "Spanish", "Intro to Python", "Web Fundamentals", "Hardware and OS"];
    private static List<Class> ClassList { get; set; } = [Class1, Class2, Class3, Class4, Class5, Class6, Class7, Class8, Class9, Class10, Class11, Class12];

    private static TermSchedule TermSchedule1 { get; set; } = new();
    private static TermSchedule TermSchedule2 { get; set; } = new();
    private static TermSchedule TermSchedule3 { get; set; } = new();
    private static TermSchedule TermSchedule4 { get; set; } = new();
    private static TermSchedule TermSchedule5 { get; set; } = new();
    private static TermSchedule TermSchedule6 { get; set; } = new();
    private static TermSchedule TermSchedule7 { get; set; } = new();
    private static TermSchedule TermSchedule8 { get; set; } = new();
    private static TermSchedule TermSchedule9 { get; set; } = new();
    private static TermSchedule TermSchedule10 { get; set; } = new();
    private static TermSchedule TermSchedule11 { get; set; } = new();
    private static TermSchedule TermSchedule12 { get; set; } = new();
    private static List<TermSchedule> TermScheduleList { get; set; } = [TermSchedule1, TermSchedule2, TermSchedule3, TermSchedule4, TermSchedule5, TermSchedule6, TermSchedule7, TermSchedule8, TermSchedule9, TermSchedule10, TermSchedule11, TermSchedule12];

    private static Instructor Instructor1 { get; set; } = new();
    
    private static ExamType ExamType1 { get; set; } = new();
    private static ExamType ExamType2 { get; set; } = new();

    public static async void CreateAllMockData()
    {
        await CreateTerms();
        await CreateInstructors();
        await CreateClasses();
        await CreateTermSchedules();
        await CreateExamTypes();
    }

    private static async Task CreateTerms()
    {
        await Db.DeleteAllTerms();

        Term1.TermName = "Spring Term";
        Term1.StartDate = new DateTime(2024, 01, 01);
        Term1.EndDate = new DateTime(2024, 06, 30);
        Term1.Id = 0;

        Term2.TermName = "Fall Term";
        Term2.StartDate = new DateTime(2024, 07, 01);
        Term2.EndDate = new DateTime(2024, 12, 31);

        List<Term> terms = [Term1, Term2];
        int rowsAdded = await Db.InsertAllTerms(terms);
        Debug.WriteLine($"Added {rowsAdded} rows to Term Table.");
    }

    private static async Task CreateInstructors()
    {
        await Db.DeleteAllInstructors();

        Instructor1.InstructorName = "Anika Patel";
        Instructor1.Email = "anika.patel@strimeuniversity.edu";
        Instructor1.PhoneNumber = "555-123-4567";

        int rowsAdded = await Db.InsertInstructor(Instructor1);
        Debug.WriteLine($"Added {rowsAdded} rows to Instructor Table.");
    }

    private static async Task CreateClasses()
    {
        await Db.DeleteAllClasses();

        for (int i = 0; i < ClassList.Count; i++)
        {
            ClassList[i].ClassName = ClassNames[i];
            ClassList[i].Notes = "The teacher is great!";
            ClassList[i].InstructorId = Instructor1.Id;
            if (i <= 5)
            {
                ClassList[i].StartDate = new DateTime(2024, 01, 01);
                ClassList[i].EndDate = new DateTime(2024, 06, 30);
            }
            else
            {
                ClassList[i].StartDate = new DateTime(2024, 07, 01);
                ClassList[i].EndDate = new DateTime(2024, 12, 31);
            }
        }

        int rowsAdded = await Db.InsertAllClasses(ClassList);
        Debug.WriteLine($"Added {rowsAdded} rows to Class Table.");
    }

    private static async Task CreateTermSchedules()
    {
        await Db.DeleteAllTermSchedules();

        for (int i = 0; i < TermScheduleList.Count;i++)
        {
            TermScheduleList[i].ClassId = ClassList[i].Id;
            if (i <= 5)
            {
                TermScheduleList[i].TermId = Term1.Id;
            }
            else
            {
                TermScheduleList[i].TermId = Term2.Id;
            }
        }

        int rowsAdded = await Db.InsertAllTermSchedules(TermScheduleList);
        Debug.WriteLine($"Added {rowsAdded} rows to TermSchedule Table.");
    }

    private static async Task CreateExamTypes()
    {
        await Db.DeleteAllExamTypes();

        ExamType1.Type = "Objective Assessment";
        ExamType2.Type = "Performance Assessment";

        List<ExamType> examList = [ExamType1,  ExamType2];
        int rowsAdded = await Db.InsertAllExamTypes(examList);
        Debug.WriteLine($"Added {rowsAdded} rows to ExamType Table.");
    }
}
