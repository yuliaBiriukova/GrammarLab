using GrammarLab.BLL.Entities;

namespace GrammarLab.PL.ViewModels;

public class AddExerciseViewModel
{
    public ExerciseType Type { get; set; }
    public string Task { get; set; }
    public string Answer { get; set; }
    public int TopicId { get; set; }
}