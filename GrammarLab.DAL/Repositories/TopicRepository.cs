using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Repositories;
using GrammarLab.DAL.Database;
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

    public async Task<IEnumerable<Topic>> GetByLevelIdAsync(int levelId)
    {
        return await _dbContext.Topics
            .Where(t => t.LevelId == levelId)
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