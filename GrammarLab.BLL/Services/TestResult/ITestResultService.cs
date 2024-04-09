using GrammarLab.BLL.Models;
using LanguageExt.Common;

namespace GrammarLab.BLL.Services;

public interface ITestResultService
{
    Task<Result<int>> AddTestResultAsync(AddTestResultDto testResult, string userId);

    Task<Result<bool>> DeleteTestResultAsync(int id);

    Task<TestResultDto?> GetByIdWithExercisesAsync(int id);

    Task<IEnumerable<TestResultDto>> GetTestResultsAsync(TestResultFilter filter, string userId);

    Task<IEnumerable<TestResultDto>> GetLastTopicsTestResultsAsync(TestResultFilter filter, string userId);

    Task<IEnumerable<TestResultDto>> GetBestTopicsTestResultsAsync(TestResultFilter filter, string userId);
}