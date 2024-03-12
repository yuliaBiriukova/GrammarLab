namespace GrammarLab.BLL.Models;

public class LevelDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public ICollection<TopicDto>? Topics { get; set; }
}