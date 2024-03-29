using GrammarLab.BLL.Repositories;
using GrammarLab.BLL.Services;
using GrammarLab.DAL.Database;
using GrammarLab.DAL.Repositories;
using GrammarLab.PL.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerWithAuth();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<GrammarLabDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthServices(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<ILevelRepository, LevelRepository>();
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<ITestResultRepository, TestResultRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<ITestResultService, TestResultService>();

var app = builder.Build();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();