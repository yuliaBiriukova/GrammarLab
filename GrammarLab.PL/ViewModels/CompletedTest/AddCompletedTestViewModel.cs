namespace GrammarLab.PL.ViewModels;

public class AddCompletedTestViewModel
{
    public int TopicId { get; set; }
    public ICollection<AddCompletedTestExerciseViewModel> CompletedTestExercises { get; set; }
}