namespace GrammarLab.BLL.Models;

public class AddCompletedTestExerciseDto
{
    public int ExerciseId { get; set; }
    public string Task { get; set; }
    public string Answer { get; set; }
    public string UserAnswer { get; set; }
}