namespace GrammarLab.BLL.Models;

public class TestResultExerciseDto
{
    public int Id { get; set; }
    public string Task { get; set; }
    public string Answer { get; set; }
    public string UserAnswer { get; set; }
    public int TestResultId { get; set; }
    public bool IsCorrect { get; set; }
}