using GrammarLab.BLL.Models;
using LanguageExt.Common;

namespace GrammarLab.BLL.Services;

public interface IExerciseService
{
    Task<Result<int>> AddAsync(AddExerciseDto exercise);

    Task<Result<bool>> DeleteAsync(int id);

    Task<ExerciseDto?> GetByIdAsync(int id);

    Task<IEnumerable<ExerciseDto>> GetByTopicIdAsync(int topicId);

    Task<Result<bool>> UpdateAsync(ExerciseDto exercise);
}