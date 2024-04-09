namespace GrammarLab.PL.ViewModels;

public class AddTestResultViewModel
{
    public int TopicId { get; set; }
    public ICollection<AddTestResultExerciseViewModel> TestResultExercises { get; set; }
}