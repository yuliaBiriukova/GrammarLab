using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Repositories;
using GrammarLab.DAL.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GrammarLab.DAL.Repositories;

public class LevelRepository : ILevelRepository
{
    private readonly GrammarLabDbContext _dbContext;

    public LevelRepository(GrammarLabDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(Level level)
    {
        _dbContext.Levels.Add(level);
        await _dbContext.SaveChangesAsync();
        return level.Id;
    }

    public async Task<bool> CheckIsUniqueAsync(Expression<Func<Level, bool>> condition)
    {
        var levelExists = await _dbContext.Levels.AnyAsync(condition);
        return !levelExists;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var levelToDelete = new Level() { Id = id };
        _dbContext.Levels.Remove(levelToDelete);
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Level>> GetAllAsync()
    {
        return await _dbContext.Levels
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Level?> GetByIdAsync(int id)
    {
        return await _dbContext.Levels
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Level?> GetByIdWithTopicsAsync(int id)
    {
        return await _dbContext.Levels
            .Include(l => l.Topics)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> UpdateAsync(Level level)
    {
        _dbContext.Levels.Update(level);
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }
}