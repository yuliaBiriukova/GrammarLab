namespace GrammarLab.PL.ViewModels;

public class BaseFilterViewModel
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
}