using CandidateManagement.Application;
using CandidateManagement.Infrastructure;
using CandidateManagement_API.Pipeline.Middlewares;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);
 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .AllowAnyOrigin()    
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
 
app.UseCors("Allow");

app.UseAuthorization();
app.UseExceptionHandler();

app.MapControllers();

app.ApplyDbContextMigrations();

app.Run();
