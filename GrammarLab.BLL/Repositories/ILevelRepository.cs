using GrammarLab.BLL.Entities;

namespace GrammarLab.BLL.Repositories;

public interface ILevelRepository
{
    Task<int> AddAsync(Level level);

    Task<bool> CheckIsCodeUniqueAsync(string code);

    Task<bool> CheckIsNameUniqueAsync(string name);

    Task<bool> CheckLevelExistsAsync(int id);

    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<Level>> GetAllAsync();

    Task<Level?> GetByIdWithTopicsAsync(int id);

    Task<Level?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(Level level);
}