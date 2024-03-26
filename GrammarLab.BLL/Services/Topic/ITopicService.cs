using GrammarLab.BLL.Models;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace GrammarLab.BLL.Services;

public interface ITopicService
{
    Task<Result<int>> AddAsync(AddTopicDto topic);

    Task<Result<bool>> DeleteAsync(int id);

    Task<TopicDto?> GetByIdAsync(int id);

    Task<IEnumerable<TopicDto>> SearchByNameAsync(string searchQuery);

    Task<Result<bool>> UpdateAsync(TopicDto topic);

    Task<ValidationException?> ValidateTopicIdAsync(int id);
}