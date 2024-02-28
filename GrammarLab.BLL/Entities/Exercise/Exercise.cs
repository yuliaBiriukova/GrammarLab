namespace GrammarLab.BLL.Entities;

public class Exercise
{
    public int Id { get; set; }
    public ExerciseType Type { get; set; }
    public string Task { get; set; }
    public string Answer { get; set; }
    public int TopicId { get; set; }
    public Topic Topic { get; set; }
}