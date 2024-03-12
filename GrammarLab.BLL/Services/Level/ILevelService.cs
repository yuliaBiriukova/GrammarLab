using GrammarLab.BLL.Models;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace GrammarLab.BLL.Services;

public interface ILevelService
{
    Task<Result<int>> AddAsync(AddLevelDto level);

    Task<bool> CheckLevelExists(int id);

    Task<Result<bool>> DeleteAsync(int id);

    Task<IEnumerable<LevelDto>> GetAllAsync();

    Task<LevelDto?> GetByIdAsync(int id);

    Task<LevelDto?> GetByIdWithTopicsAsync(int id);

    Task<Result<bool>> UpdateAsync(LevelDto level);

    Task<ValidationException?> ValidateLevelIdAsync(int id);
}