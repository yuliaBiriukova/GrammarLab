using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Repositories;
using GrammarLab.DAL.Database;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.DAL.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly GrammarLabDbContext _dbContext;

    public TopicRepository(GrammarLabDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(Topic topic)
    {
        _dbContext.Topics.Add(topic);
        await _dbContext.SaveChangesAsync();
        return topic.Id;
    }

    public async Task<bool> CheckExistsAsync(int id)
    {
        return await _dbContext.Topics.AnyAsync(t => t.Id == id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var topicToDelete = new Topic() { Id = id };
        _dbContext.Topics.Remove(topicToDelete);
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<Topic?> GetByIdAsync(int id)
    {
        return await _dbContext.Topics
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Topic>> GetByNameAsync(string nameQuery)
    {
        var keywords = nameQuery.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

        return await _dbContext.Topics
            .Select(x => new
            {
                Topic = x,
                MatchCount = keywords.Count(k => x.Name.ToLower().Contains(k))
            })
            .Where(x => x.MatchCount > 0)
            .OrderByDescending(x => x.MatchCount)
            .Select(x => x.Topic)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Topic topic)
    {
        _dbContext.Topics.Update(topic);
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }
}