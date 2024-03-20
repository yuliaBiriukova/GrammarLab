using GrammarLab.BLL.Models;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace GrammarLab.BLL.Services;

public interface ILevelService
{
    Task<Result<int>> AddLevelAsync(AddLevelDto level);

    Task<Result<bool>> DeleteLevelAsync(int id);

    Task<IEnumerable<LevelDto>> GetAllLevelsAsync();

    Task<LevelDto?> GetLevelByIdAsync(int id);

    Task<LevelDto?> GetLevelByIdWithTopicsAsync(int id);

    Task<Result<bool>> UpdateLevelAsync(LevelDto level);

    Task<ValidationException?> ValidateLevelIdAsync(int id);
}