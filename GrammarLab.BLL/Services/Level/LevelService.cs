using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace GrammarLab.BLL.Services;

public class LevelService : ILevelService
{
    private readonly ILevelRepository _levelRepository;
    private readonly IMapper _mapper;

    public LevelService(ILevelRepository levelRepository, IMapper mapper)
    {
        _levelRepository = levelRepository;
        _mapper = mapper;
    }

    public async Task<Result<int>> AddAsync(AddLevelDto level)
    {
        var validationError = await ValidateCodeAndNameAsync(level.Code, level.Name);
        if (validationError != null)
        {
            return new Result<int>(validationError);
        }

        var newLevel = _mapper.Map<Level>(level);
        var id = await _levelRepository.AddAsync(newLevel);
        return id;
    }

    public async Task<bool> CheckLevelExists(int id)
    {
        var isIdUnique = await _levelRepository.CheckIsUniqueAsync(l => l.Id == id);
        return !isIdUnique;
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var validationError = await ValidateLevelIdAsync(id);
        if(validationError != null)
        {
            return new Result<bool>(validationError);
        }

        var isDeleted = await _levelRepository.DeleteAsync(id);
        return isDeleted;
    }

    public async Task<IEnumerable<LevelDto>> GetAllAsync()
    {
        var levels = await _levelRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<LevelDto>>(levels);
    }

    public async Task<LevelDto?> GetByIdAsync(int id)
    {
        var level = await _levelRepository.GetByIdAsync(id);
        return _mapper.Map<LevelDto>(level);
    }

    public async Task<LevelDto?> GetByIdWithTopicsAsync(int id)
    {
        var level = await _levelRepository.GetByIdWithTopicsAsync(id);
        return _mapper.Map<LevelDto>(level);
    }

    public async Task<Result<bool>> UpdateAsync(LevelDto level)
    {
        var validationError = await ValidateLevelAsync(level);
        if (validationError != null)
        {
            return new Result<bool>(validationError);
        }

        var updatedLevel = _mapper.Map<Level>(level);
        var isUpdated = await _levelRepository.UpdateAsync(updatedLevel);
        return isUpdated;
    }

    public async Task<ValidationException?> ValidateLevelIdAsync(int id)
    {
        var levelExists = await CheckLevelExists(id);
        if (!levelExists)
        {
            return new ValidationException($"Level with id={id} does not exist.");
        }

        return null;
    }

    private async Task<ValidationException?> ValidateCodeAsync(string code)
    {
        var isCodeUnique = await _levelRepository.CheckIsUniqueAsync(l => l.Code.ToLower() == code.ToLower());
        if (!isCodeUnique)
        {
            return new ValidationException($"Level with code {code} exists.");
        }

        return null;
    }

    private async Task<ValidationException?> ValidateNameAsync(string name)
    {
        var isNameUnique = await _levelRepository.CheckIsUniqueAsync(l => l.Name.ToLower() == name.ToLower());
        if (!isNameUnique)
        {
            return new ValidationException($"Level with name {name} exists.");
        }

        return null;
    }

    private async Task<ValidationException?> ValidateCodeAndNameAsync(string code, string name)
    {
        var codeValidationError = await ValidateCodeAsync(code);
        if(codeValidationError != null)
        {
            return codeValidationError;
        }

        return await ValidateNameAsync(name);
    }

    private async Task<ValidationException?> ValidateLevelAsync(LevelDto level)
    {
        var existingLevel = await _levelRepository.GetByIdAsync(level.Id);
        if (existingLevel == null)
        {
            return new ValidationException($"Level with id={level.Id} does not exist.");
        }

        var isCodesMatch = string.Equals(level.Code, existingLevel.Code, StringComparison.OrdinalIgnoreCase);
        var isNamesMatch = string.Equals(level.Name, existingLevel.Name, StringComparison.OrdinalIgnoreCase);

        if(isCodesMatch && isNamesMatch)
        {
            return null;
        }

        if(!isCodesMatch)
        {
            var codeValidationError = await ValidateCodeAsync(level.Code);
            if (codeValidationError != null)
            {
                return codeValidationError;
            }
        }

        if (!isNamesMatch)
        {
            var namesValidationError = await ValidateNameAsync(level.Name);
            if (namesValidationError != null)
            {
                return namesValidationError;
            }
        }

        return null;
    }
}