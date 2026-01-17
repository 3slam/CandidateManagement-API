using CandidateManagement.Application;
using CandidateManagement.Infrastructure;
using CandidateManagement_API.Pipeline.Middlewares;
using System;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers()
            .Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddApplication()
            .AddInfrastructure(builder.Configuration);

var app = builder.Build();
      
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();

app.ApplyDbContextMigrations();

app.Run();