﻿namespace GrammarLab.BLL.Models;

public class TestResultDto
{
    public int Id { get; set; }
    public int Percentage { get; set; }
    public DateTime DateCompleted { get; set; }
    public int TopicId { get; set; }
    public string TopicName { get; set; }
    public ICollection<TestResultExerciseDto> TestResultExercises { get; set; }
}