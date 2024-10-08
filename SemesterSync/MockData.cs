﻿using DataLibrary;
using ModelLibrary;
using ServiceLibrary;
using System.Diagnostics;

namespace SemesterSync;

public static class MockData
{
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
    private static List<string> ClassNames { get; set; } =
        ["US History", "Chemistry", "Biology", "Health", "Communications", "Art",
        "Trigonometry", "Algebra", "Spanish", "Intro to Python", "Web Fundamentals", "Hardware and OS"];
    private static List<Class> ClassList { get; set; } =
        [Class1, Class2, Class3, Class4, Class5, Class6,
        Class7, Class8, Class9, Class10, Class11, Class12];

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
    private static List<TermSchedule> TermScheduleList { get; set; } =
        [TermSchedule1, TermSchedule2, TermSchedule3, TermSchedule4, TermSchedule5, TermSchedule6,
        TermSchedule7, TermSchedule8, TermSchedule9, TermSchedule10, TermSchedule11, TermSchedule12];

    private static Exam OA1 { get; set; } = new();
    private static Exam OA2 { get; set; } = new();
    private static Exam OA3 { get; set; } = new();
    private static Exam OA4 { get; set; } = new();
    private static Exam OA5 { get; set; } = new();
    private static Exam OA6 { get; set; } = new();
    private static Exam OA7 { get; set; } = new();
    private static Exam OA8 { get; set; } = new();
    private static Exam OA9 { get; set; } = new();
    private static Exam OA10 { get; set; } = new();
    private static Exam OA11 { get; set; } = new();
    private static Exam OA12 { get; set; } = new();
    private static Exam PA1 { get; set; } = new();
    private static Exam PA2 { get; set; } = new();
    private static Exam PA3 { get; set; } = new();
    private static Exam PA4 { get; set; } = new();
    private static Exam PA5 { get; set; } = new();
    private static Exam PA6 { get; set; } = new();
    private static Exam PA7 { get; set; } = new();
    private static Exam PA8 { get; set; } = new();
    private static Exam PA9 { get; set; } = new();
    private static Exam PA10 { get; set; } = new();
    private static Exam PA11 { get; set; } = new();
    private static Exam PA12 { get; set; } = new();
    private static List<Exam> OAList { get; set; } = [OA1, OA2, OA3, OA4, OA5, OA6, OA7, OA8, OA9, OA10, OA11, OA12];
    private static List<string> OANameList { get; set; } =
        ["US History Final", "Chemistry Final", "Biology Final", "Health Final", "Communications Final", "Art Final",
        "Trigonometry Final", "Algebra Final", "Spanish Final", "Intro to Python Final", "Web Fundamentals Final", "Hardware and OS Final"];
    private static List<Exam> PAList { get; set; } = [PA1, PA2, PA3, PA4, PA5, PA6, PA7, PA8, PA9, PA10, PA11, PA12];
    private static List<string> PANameList { get; set; } =
        ["US History PA", "Chemistry PA", "Biology PA", "Health PA", "Communications PA", "Art PA",
        "Trigonometry PA", "Algebra PA", "Spanish PA", "Intro to Python PA", "Web Fundamentals PA", "Hardware and OS PA"];

    private static Instructor Instructor1 { get; set; } = new();

    private static ExamType ExamType1 { get; set; } = new();
    private static ExamType ExamType2 { get; set; } = new();

    private static UserDTO User1 { get; set; } = new("David", "Ogden", "davidogden@email.com", "555-555-555", "software engineering", DateTime.SpecifyKind(new DateTime(2024, 08, 31), DateTimeKind.Utc), "password");
    private static UserDTO User2 { get; set; } = new("David", "Ogden", "ogden@email.com", "555-555-555", "software engineering", DateTime.SpecifyKind(new DateTime(2024, 08, 31), DateTimeKind.Utc), "password");

    public static async Task CreateAllMockData()
    {
        await CreateTerms();
        await CreateInstructors();
        await CreateClasses();
        await CreateTermSchedules();
        await CreateExamTypes();
        await CreateExams();
        await CreateUser();
    }

    private static async Task CreateTerms()
    {
        await DbContext.DeleteAllItemsAsync<Term>();

        Term1.TermName = "Spring Term";
        Term1.StartDate = new DateTime(2024, 01, 01);
        Term1.EndDate = new DateTime(2024, 06, 30);
        Term1.CreatedBy = "davidogden@email.com";

        Term2.TermName = "Fall Term";
        Term2.StartDate = new DateTime(2024, 07, 01);
        Term2.EndDate = new DateTime(2024, 12, 31);
        Term2.CreatedBy = "davidogden@email.com";

        List<Term> terms = [Term1, Term2];
        int rowsAdded = await DbContext.AddAllItemsAsync(terms);
        Debug.WriteLine($"Added {rowsAdded} rows to Term Table.");
    }

    private static async Task CreateInstructors()
    {
        await DbContext.DeleteAllItemsAsync<Instructor>();

        Instructor1.InstructorName = "Anika Patel";
        Instructor1.Email = "anika.patel@strimeuniversity.edu";
        Instructor1.PhoneNumber = "5551234567";
        Instructor1.CreatedBy = "davidogden@email.com";

        List<Instructor> instructors = [Instructor1];

        int rowsAdded = await DbContext.AddAllItemsAsync(instructors);
        Debug.WriteLine($"Added {rowsAdded} rows to Instructor Table.");
    }

    private static async Task CreateClasses()
    {
        await DbContext.DeleteAllItemsAsync<Class>();

        for (int i = 0; i < ClassList.Count; i++)
        {
            ClassList[i].ClassName = ClassNames[i];
            ClassList[i].Notes = "The teacher is great!";
            ClassList[i].InstructorId = Instructor1.Id;
            ClassList[i].CreatedBy = "davidogden@email.com";
            if (i <= 5)
            {
                ClassList[i].StartDate = new DateTime(2024, 01, 01);
                ClassList[i].EndDate = new DateTime(2024, 06, 30);
                if (i % 2 == 0) ClassList[i].Status = "Passed";
                else ClassList[i].Status = "Failed";
            }
            else
            {
                ClassList[i].StartDate = new DateTime(2024, 07, 01);
                ClassList[i].EndDate = new DateTime(2024, 12, 31);
                ClassList[i].Status = "Active";
            }
        }

        int rowsAdded = await DbContext.AddAllItemsAsync(ClassList);
        Debug.WriteLine($"Added {rowsAdded} rows to Class Table.");
    }

    private static async Task CreateTermSchedules()
    {
        await DbContext.DeleteAllItemsAsync<TermSchedule>();

        for (int i = 0; i < TermScheduleList.Count; i++)
        {
            TermScheduleList[i].ClassId = ClassList[i].Id;
            TermScheduleList[i].CreatedBy = "davidogden@email.com";
            if (i <= 5)
            {
                TermScheduleList[i].TermId = Term1.Id;
            }
            else
            {
                TermScheduleList[i].TermId = Term2.Id;
            }
        }

        int rowsAdded = await DbContext.AddAllItemsAsync(TermScheduleList);
        Debug.WriteLine($"Added {rowsAdded} rows to TermSchedule Table.");
    }

    private static async Task CreateExamTypes()
    {
        await DbContext.DeleteAllItemsAsync<ExamType>();

        ExamType1.Type = "Objective Assessment";
        ExamType2.Type = "Performance Assessment";

        List<ExamType> examList = [ExamType1, ExamType2];
        int rowsAdded = await DbContext.AddAllItemsAsync(examList);
        Debug.WriteLine($"Added {rowsAdded} rows to ExamType Table.");
    }

    private static async Task CreateExams()
    {
        await DbContext.DeleteAllItemsAsync<Exam>();

        Random random = new Random();


        for (int i = 0; i < ClassList.Count; i++)
        {
            OAList[i].ClassId = ClassList[i].Id;
            PAList[i].ClassId = ClassList[i].Id;

            OAList[i].ExamName = OANameList[i];
            PAList[i].ExamName = PANameList[i];

            OAList[i].ExamTypeId = ExamType1.Id;
            PAList[i].ExamTypeId = ExamType2.Id;

            OAList[i].CreatedBy = "davidogden@email.com";
            PAList[i].CreatedBy = "davidogden@email.com";

            int hourDiff = Math.Abs(ClassList[i].StartDate.TimeOfDay.Hours - ClassList[i].EndDate.TimeOfDay.Hours);

            OAList[i].StartTime = ClassList[i].StartDate.AddHours(random.Next(hourDiff));
            PAList[i].StartTime = ClassList[i].StartDate.AddHours(random.Next(hourDiff));

            OAList[i].EndTime = OAList[i].StartTime.AddHours(random.Next(4));
            PAList[i].EndTime = PAList[i].StartTime.AddHours(random.Next(4));
        }

        int oARowsAdded = await DbContext.AddAllItemsAsync(OAList);
        int pARowsAdded = await DbContext.AddAllItemsAsync(PAList);

        Debug.WriteLine($"Added to Exam Table:\n\t{oARowsAdded} Objective Assessments\n\t{pARowsAdded} Performance Assessments");
    }

    private static async Task CreateUser()
    {
        AuthService authService = AuthService.GetInstance();
        await DbContext.DeleteAllItemsAsync<User>();
        await authService.CreateUser(User2);
        bool userCreated = await authService.CreateUser(User1);
        await authService.AddUserEmailToSecureStorage(User1.Email);
        Debug.WriteLineIf(userCreated, "User Created Succesfully");
    }
}
