using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Repositories;
using GrammarLab.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.DAL.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly GrammarLabDbContext _dbContext;

    public ExerciseRepository(GrammarLabDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(Exercise exercise)
    {
        _dbContext.Exercises.Add(exercise);
        await _dbContext.SaveChangesAsync();
        return exercise.Id;
    }

    public async Task<bool> CheckExistsAsync(int id)
    {
        return await _dbContext.Exercises.AnyAsync(e => e.Id == id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var exerciseToDelete = new Exercise() { Id = id};
        _dbContext.Exercises.Remove(exerciseToDelete);
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<Exercise?> GetByIdAsync(int id)
    {
        return await _dbContext.Exercises
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Exercise>> GetByTopicIdAsync(int topicId)
    {
        return await _dbContext.Exercises
           .Where(e => e.TopicId == topicId)
           .AsNoTracking()
           .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Exercise exercise)
    {
        _dbContext.Exercises.Update(exercise);
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }
}