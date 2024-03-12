using GrammarLab.BLL.Entities;
using System.Linq.Expressions;

namespace GrammarLab.BLL.Repositories;

public interface ILevelRepository
{
    Task<int> AddAsync(Level level);

    Task<bool> CheckIsUniqueAsync(Expression<Func<Level, bool>> condition);

    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<Level>> GetAllAsync();

    Task<Level?> GetByIdWithTopicsAsync(int id);

    Task<Level?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(Level level);
}