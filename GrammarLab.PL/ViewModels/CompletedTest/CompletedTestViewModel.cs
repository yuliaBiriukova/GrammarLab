namespace GrammarLab.PL.ViewModels;

public class CompletedTestViewModel
{
    public int Id { get; set; }
    public int Percentage { get; set; }
    public DateTime DateCompleted { get; set; }
    public int TopicId { get; set; }
    public string TopicName { get; set; }
    public ICollection<CompletedTestExerciseViewModel> CompletedTestExercises { get; set; }
}