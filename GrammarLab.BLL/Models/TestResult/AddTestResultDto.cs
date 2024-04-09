namespace GrammarLab.BLL.Models;

public class AddTestResultDto
{
    public int Percentage { get; set; }
    public DateTime DateCompleted { get; set; }
    public int TopicId { get; set; }
    public ICollection<AddTestResultExerciseDto> TestResultExercises { get; set; }
}