using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using GrammarLab.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.DAL.Repositories;

public class TestResultRepository : ITestResultRepository
{
    private readonly GrammarLabDbContext _dbContext;

    public TestResultRepository(GrammarLabDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(TestResult testResult)
    {
        _dbContext.TestResults.Add(testResult);
        await _dbContext.SaveChangesAsync();
        return testResult.Id;
    }

    public async Task<bool> CheckExistsAsync(int id)
    {
        return await _dbContext.TestResults.AnyAsync(t => t.Id == id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var testResultToDelete = new TestResult() { Id = id };
        _dbContext.TestResults.Remove(testResultToDelete);
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<TestResult>> GetAsync(TestResultFilter filter)
    {
        var query = _dbContext.TestResults
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

    public async Task<IEnumerable<TestResult>> GetBestTopicsResultsAsync(TestResultFilter filter)
    {
        var query = _dbContext.TestResults
             .Include(c => c.Topic)
             .Where(c => c.UserId == filter.UserId);

        if (filter.LevelId != null)
        {
            query = query.Where(c => c.Topic.LevelId == filter.LevelId);
        }

        var groupedTestResults = await query
            .GroupBy(c => c.TopicId)
            .Select(group => group
                .OrderByDescending(c => c.Percentage)
                .First())
            .AsNoTracking()
            .ToListAsync();

        var filteredResults = ApplyFilter(groupedTestResults.AsQueryable(), filter);

        return filteredResults.ToList();
    }

    public async Task<TestResult?> GetByIdWithExercisesAsync(int id)
    {
        return await _dbContext.TestResults
            .Include(c =>  c.Topic)
            .Include(c => c.TestResultExercises)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<TestResult>> GetByTopicIdAsync(int topicId, string userId)
    {
        return await _dbContext.TestResults
            .Include(с => с.Topic)
            .Where(c => c.UserId == userId && c.TopicId == topicId)
            .OrderByDescending(c => c.DateCompleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TestResult>> GetLastTopicsResultsAsync(TestResultFilter filter)
    {
        var query = _dbContext.TestResults
            .Include(c => c.Topic)
            .Where(c => c.UserId == filter.UserId);

        if (filter.LevelId != null)
        {
            query = query.Where(c => c.Topic.LevelId == filter.LevelId);
        }
        
        var groupedTestResults = await query
            .GroupBy(c => c.TopicId)
            .Select(group => group
                .OrderByDescending(c => c.DateCompleted)
                .First())
            .AsNoTracking()
            .ToListAsync();

        var filteredResults = ApplyFilter(groupedTestResults.AsQueryable(), filter);

        return filteredResults.ToList();
    }

    private IQueryable<TestResult> ApplyFilter(IQueryable<TestResult> query, BaseFilter filter)
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