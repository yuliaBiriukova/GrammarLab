namespace GrammarLab.PL.ViewModels;

public class CompletedTestExerciseViewModel
{
    public int Id { get; set; }
    public string Task { get; set; }
    public string Answer { get; set; }
    public string UserAnswer { get; set; }
    public int CompletedTestId { get; set; }
}