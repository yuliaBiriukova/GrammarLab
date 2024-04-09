namespace GrammarLab.PL.ViewModels;

public class TestResultExerciseViewModel
{
    public int Id { get; set; }
    public string Task { get; set; }
    public string Answer { get; set; }
    public string UserAnswer { get; set; }
    public int TestResultId { get; set; }
    public bool IsCorrect {  get; set; }
}