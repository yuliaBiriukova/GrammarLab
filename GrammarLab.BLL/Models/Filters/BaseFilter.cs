namespace GrammarLab.BLL.Models;

public class BaseFilter
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
}