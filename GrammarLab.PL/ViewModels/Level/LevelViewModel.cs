namespace GrammarLab.PL.ViewModels;

public class LevelViewModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public ICollection<LevelTopicViewModel>? Topics { get; set; }
}