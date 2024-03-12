using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;

namespace GrammarLab.BLL.Repositories;

public interface ICompletedTestRepository
{
    Task<int> AddAsync(CompletedTest completedTest);

    Task<bool> CheckExistsAsync(int id);

    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<CompletedTest>> GetAsync(CompletedTestFilter filter);

    Task<CompletedTest?> GetByIdWithExercisesAsync(int id);

    Task<IEnumerable<CompletedTest>> GetLastTopicsResultsAsync(CompletedTestFilter filter);

    Task<IEnumerable<CompletedTest>> GetBestTopicsResultsAsync(CompletedTestFilter filter);
}