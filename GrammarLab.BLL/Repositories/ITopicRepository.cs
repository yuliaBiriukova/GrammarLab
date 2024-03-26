using GrammarLab.BLL.Entities;

namespace GrammarLab.BLL.Repositories;

public interface ITopicRepository
{
    Task<int> AddAsync(Topic topic);

    Task<bool> CheckExistsAsync(int id);

    Task<bool> DeleteAsync(int id);

    Task<Topic?> GetByIdAsync(int id);

    Task<IEnumerable<Topic>> GetByNameAsync(string nameQuery);

    Task<bool> UpdateAsync(Topic topic);
}