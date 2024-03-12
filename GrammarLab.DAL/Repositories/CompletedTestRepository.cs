using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using GrammarLab.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.DAL.Repositories;

public class CompletedTestRepository : ICompletedTestRepository
{
    private readonly GrammarLabDbContext _dbContext;

    public CompletedTestRepository(GrammarLabDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(CompletedTest completedTest)
    {
        _dbContext.CompletedTests.Add(completedTest);
        await _dbContext.SaveChangesAsync();
        return completedTest.Id;
    }

    public async Task<bool> CheckExistsAsync(int id)
    {
        return await _dbContext.CompletedTests.AnyAsync(t => t.Id == id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var completedTestToDelete = new CompletedTest() { Id = id };
        _dbContext.CompletedTests.Remove(completedTestToDelete);
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<CompletedTest>> GetAsync(CompletedTestFilter filter)
    {
        var query = _dbContext.CompletedTests
            .Include(с => с.Topic)
            .Where(c => c.UserId == filter.UserId)
            .AsQueryable();

        if (filter.TopicId != null)
        {
            query = query.Where(c => c.TopicId == filter.TopicId);
        }

        query = ApplyFilter(query, filter);

        return await query
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<CompletedTest>> GetBestTopicsResultsAsync(CompletedTestFilter filter)
    {
        var query = _dbContext.CompletedTests
             .Include(c => c.Topic)
             .Where(c => c.UserId == filter.UserId);

        if (filter.LevelId != null)
        {
            query = query.Where(c => c.Topic.LevelId == filter.LevelId);
        }

        var groupedCompletedTests = await query
            .GroupBy(c => c.TopicId)
            .Select(group => group
                .OrderByDescending(c => c.Percentage)
                .First())
            .AsNoTracking()
            .ToListAsync();

        var filteredResults = ApplyFilter(groupedCompletedTests.AsQueryable(), filter);

        return filteredResults.ToList();
    }

    public async Task<CompletedTest?> GetByIdWithExercisesAsync(int id)
    {
        return await _dbContext.CompletedTests
            .Include(c =>  c.Topic)
            .Include(c => c.CompletedTestExercises)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<CompletedTest>> GetByTopicIdAsync(int topicId, string userId)
    {
        return await _dbContext.CompletedTests
            .Include(с => с.Topic)
            .Where(c => c.UserId == userId && c.TopicId == topicId)
            .OrderByDescending(c => c.DateCompleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<CompletedTest>> GetLastTopicsResultsAsync(CompletedTestFilter filter)
    {
        var query = _dbContext.CompletedTests
            .Include(c => c.Topic)
            .Where(c => c.UserId == filter.UserId);

        if (filter.LevelId != null)
        {
            query = query.Where(c => c.Topic.LevelId == filter.LevelId);
        }
        
        var groupedCompletedTests = await query
            .GroupBy(c => c.TopicId)
            .Select(group => group
                .OrderByDescending(c => c.DateCompleted)
                .First())
            .AsNoTracking()
            .ToListAsync();

        var filteredResults = ApplyFilter(groupedCompletedTests.AsQueryable(), filter);

        return filteredResults.ToList();
    }

    private IQueryable<CompletedTest> ApplyFilter(IQueryable<CompletedTest> query, BaseFilter filter)
    {
        if (string.Equals(filter.SortBy, Constants.PercentageProperty, StringComparison.OrdinalIgnoreCase))
        {
            query = string.Equals(filter.SortOrder, Constants.SortAscending, StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy(c => c.Percentage)
                : query.OrderByDescending(c => c.Percentage);
        }
        else if (string.Equals(filter.SortBy, Constants.DateCompletedProperty, StringComparison.OrdinalIgnoreCase))
        {
            query = string.Equals(filter.SortOrder, Constants.SortAscending, StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy(c => c.DateCompleted)
                : query.OrderByDescending(c => c.DateCompleted);
        }
        else
        {
            query = query.OrderByDescending(c => c.DateCompleted);
        }

        if (filter.PageNumber != null && filter.PageSize != null)
        {
            var totalTestsCount = query.Count();
            var totalPagesCount = totalTestsCount / filter.PageSize ?? 1;
            var pageNumber = Math.Max(1, Math.Min(filter.PageNumber.Value, totalPagesCount));
            var skip = (pageNumber - 1) * filter.PageSize;
            query = query.Skip(skip.Value).Take(filter.PageSize.Value);
        }

        return query;
    }
}