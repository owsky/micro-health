using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddWorkoutInfrastructure();
builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.ConfigureHttpJsonOptions(options =>
  options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
);
builder.Services.Configure<JsonOptions>(options =>
  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);
builder.Services.AddWorkoutAuth();
builder.Services.AddWorkoutMappings();
builder.Services.AddWorkoutServices();
builder.Services.AddHealthChecks();

var app = builder.Build();

await app.MigrateAllDbContextsAsync();

app.MapOpenApi();
if (app.Environment.IsDevelopment())
{
  app.MapScalarApiReference();
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<UserInfoMiddleware>();
app.MapHealthChecks("/healthz");
app.UseHttpsRedirection();
app.MapFeatureEndpoints();

app.Run();
