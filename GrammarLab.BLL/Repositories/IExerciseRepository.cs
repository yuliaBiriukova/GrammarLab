using GrammarLab.BLL.Entities;

namespace GrammarLab.BLL.Repositories;

public interface IExerciseRepository
{
    Task<int> AddAsync(Exercise exercise);

    Task<bool> CheckExistsAsync(int id);

    Task<bool> DeleteAsync(int id);

    Task<Exercise?> GetByIdAsync(int id);

    Task<IEnumerable<Exercise>> GetByTopicIdAsync(int topicId);

    Task<bool> UpdateAsync(Exercise exercise);
}