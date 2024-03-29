namespace GrammarLab.BLL.Models;

public class TestResultFilter : BaseFilter
{
    public int? LevelId { get; set; }
    public int? TopicId { get; set; }
    public string UserId { get; set;}
}