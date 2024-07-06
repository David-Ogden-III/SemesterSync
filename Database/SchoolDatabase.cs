using SQLite;
using System.Diagnostics;

namespace C971_Ogden.Database;

public class SchoolDatabase
{
    SQLiteAsyncConnection Database;

    public SchoolDatabase()
    {

    }

    public async Task Init()
    {
        if (Database is not null)
        {
            return;
        }

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        var TermResult = await Database.CreateTableAsync<Term>();
        var ClassResult = await Database.CreateTableAsync<Class>();
        var TermScheduleResult = await Database.CreateTableAsync<TermSchedule>();
        var InstructorResult = await Database.CreateTableAsync<Instructor>();
        var ExamResult = await Database.CreateTableAsync<Exam>();
        var ExamTypesResult = await Database.CreateTableAsync<ExamType>();

        Debug.WriteLine($"Term Table: {TermResult}\nClass Table: {ClassResult}\nTermSchedule Table: {TermScheduleResult}\nInstructor Table: {InstructorResult}\nExam Table: {ExamResult}\nExamType Table: {ExamTypesResult}\n");
    }

    // Term Table
    public async Task<List<Term>> GetTermsAsync()
    {
        await Init();
        return await Database.Table<Term>().ToListAsync();
    }

    public async Task<int> InsertAllTerms(List<Term> terms)
    {
        await Init();
        int result = await Database.InsertAllAsync(terms);
        return result;
    }

    // Update or Instert Term
    public async Task<int> SaveTermAsync(Term term)
    {
        await Init();
        if (term.Id != 0)
        {
            return await Database.UpdateAsync(term);
        }
        else
        {
            return await Database.InsertAsync(term);
        }
    }

    public async Task<int> DeleteTermAsync(Term term)
    {
        await Init();
        return await Database.DeleteAsync(term);
    }

    public async Task DeleteAllTerms()
    {
        await Init();
        await Database.DeleteAllAsync<Term>();
    }




    // Instructor Table
    public async Task DeleteAllInstructors()
    {
        await Init();
        await Database.DeleteAllAsync<Instructor>();
    }

    public async Task<int> InsertInstructor(Instructor instructor)
    {
        await Init();
        int result = await Database.InsertAsync(instructor);
        return result;
    }




    // Class Table
    public async Task DeleteAllClasses()
    {
        await Init();
        await Database.DeleteAllAsync<Class>();
    }

    public async Task<int> InsertAllClasses(List<Class> classes)
    {
        await Init();
        int rowsAdded = await Database.InsertAllAsync(classes);
        return rowsAdded;
    }




    // TermSchedule Table
    public async Task DeleteAllTermSchedules()
    {
        await Init();
        await Database.DeleteAllAsync<TermSchedule>();
    }

    public async Task<int> InsertAllTermSchedules(List<TermSchedule> termSchedules)
    {
        await Init();
        int rowsAdded = await Database.InsertAllAsync(termSchedules);
        return rowsAdded;
    }




    // ExamType Table
    public async Task DeleteAllExamTypes()
    {
        await Init();
        await Database.DeleteAllAsync<ExamType>();
    }

    public async Task<int> InsertAllExamTypes(List<ExamType> examTypes)
    {
        await Init();
        int rowsAdded = await Database.InsertAllAsync(examTypes);
        return rowsAdded;
    }




    // Exam Table
    public async Task DeleteAllExams()
    {
        await Init();
        await Database.DeleteAllAsync<Exam>();
    }

    public async Task<int> InsertAllExams(List<Exam> exams)
    {
        await Init();
        int rowsAdded = await Database.InsertAllAsync(exams);
        return rowsAdded;
    }
}
