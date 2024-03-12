namespace GrammarLab.BLL.Models;

public class AddCompletedTestDto
{
    public int Percentage { get; set; }
    public DateTime DateCompleted { get; set; }
    public int TopicId { get; set; }
    public ICollection<AddCompletedTestExerciseDto> CompletedTestExercises { get; set; }
}