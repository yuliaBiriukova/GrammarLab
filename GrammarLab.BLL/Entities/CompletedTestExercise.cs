namespace GrammarLab.BLL.Entities;

public class CompletedTestExercise
{
    public int Id { get; set; }
    public string Task { get; set; }
    public string Answer { get; set; }
    public string UserAnswer { get; set; }
    public int CompletedTestId { get; set; }
    public CompletedTest CompletedTest { get; set; }
}