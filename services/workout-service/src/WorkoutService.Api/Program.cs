using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Exceptions;
using WorkoutService.Common.Logging;
using WorkoutService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddWorkoutInfrastructure();
builder.AddWorkoutMessaging();
builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.ConfigureHttpJsonOptions(options =>
  options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
);
builder.Services.Configure<JsonOptions>(options =>
  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);
builder.Services.AddWorkoutAuth();
builder.Services.AddWorkoutServices();
builder.Services.AddHealthChecks();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpoints();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<WorkoutServiceDbContext>();
  await db.Database.MigrateAsync();
}

app.MapOpenApi();
if (app.Environment.IsDevelopment())
{
  app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<UserInfoMiddleware>();
app.MapHealthChecks("/healthz");
app.MapEndpoints();

app.Run();
