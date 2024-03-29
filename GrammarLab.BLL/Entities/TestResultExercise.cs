namespace GrammarLab.BLL.Entities;

public class TestResultExercise
{
    public int Id { get; set; }
    public string Task { get; set; }
    public string Answer { get; set; }
    public string UserAnswer { get; set; }
    public int TestResultId { get; set; }
    public TestResult TestResult { get; set; }
}