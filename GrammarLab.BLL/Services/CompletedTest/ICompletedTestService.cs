using GrammarLab.BLL.Models;
using LanguageExt.Common;

namespace GrammarLab.BLL.Services;

public interface ICompletedTestService
{
    Task<Result<int>> AddCompletedtTestAsync(AddCompletedTestDto completedTest, string userId);

    Task<Result<bool>> DeleteCompletedTestAsync(int id);

    Task<CompletedTestDto?> GetByIdWithExercisesAsync(int id);

    Task<IEnumerable<CompletedTestDto>> GetCompletedTestsAsync(CompletedTestFilter filter, string userId);

    Task<IEnumerable<CompletedTestDto>> GetLastTopicsResultsAsync(CompletedTestFilter filter, string userId);

    Task<IEnumerable<CompletedTestDto>> GetBestTopicsResultsAsync(CompletedTestFilter filter, string userId);
}