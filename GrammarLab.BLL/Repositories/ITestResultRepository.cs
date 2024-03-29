using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;

namespace GrammarLab.BLL.Repositories;

public interface ITestResultRepository
{
    Task<int> AddAsync(TestResult testResult);

    Task<bool> CheckExistsAsync(int id);

    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<TestResult>> GetAsync(TestResultFilter filter);

    Task<TestResult?> GetByIdWithExercisesAsync(int id);

    Task<IEnumerable<TestResult>> GetLastTopicsResultsAsync(TestResultFilter filter);

    Task<IEnumerable<TestResult>> GetBestTopicsResultsAsync(TestResultFilter filter);
}