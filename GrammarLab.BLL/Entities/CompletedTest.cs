﻿namespace GrammarLab.BLL.Entities;

public class CompletedTest
{
    public int Id { get; set; }
    public int Percentage { get; set; }
    public DateTime DateCompleted { get; set; }
    public int TopicId { get; set; }
    public Topic Topic { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public ICollection<CompletedTestExercise>? CompletedTestExercises { get; set; }
}