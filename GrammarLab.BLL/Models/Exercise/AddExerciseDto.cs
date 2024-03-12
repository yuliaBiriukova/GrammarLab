using GrammarLab.BLL.Entities;

namespace GrammarLab.BLL.Models;

public class AddExerciseDto
{
    public ExerciseType Type { get; set; }
    public string Task { get; set; }
    public string Answer { get; set; }
    public int TopicId { get; set; }
}