using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WorkoutService.Common.Auth;
using WorkoutService.Common.Enums;
using WorkoutService.Features.WorkoutTemplates.Dtos;
using WorkoutService.Features.WorkoutTemplates.Services;

namespace WorkoutService.Features.WorkoutTemplates.Endpoints;

public class GetAllWorkoutTemplates : WorkoutTemplatesEndpointBase
{
  protected override void Map(RouteGroupBuilder group)
  {
    group
      .MapGet(
        "",
        async Task<Ok<List<WorkoutTemplateResponse>>> (
          IWorkoutTemplatesService service,
          UserInfo userInfo,
          [FromQuery(Name = "exercises")] long[]? exercises = null,
          [FromQuery] bool mine = false,
          [FromQuery] string? name = null,
          [FromQuery] Difficulty? overallDifficulty = null,
          [FromQuery] int pageSize = 15,
          [FromQuery] int pageNumber = 1
        ) =>
        {
          var exercisesList = exercises?.ToList();
          var templates = await service.GetAllWorkoutTemplates(
            pageSize,
            pageNumber,
            userInfo,
            name,
            overallDifficulty,
            exercisesList,
            mine
          );
          return TypedResults.Ok(templates);
        }
      )
      .WithName("GetAllWorkoutTemplates")
      .WithSummary("Get all workout templates, with optional filtering and pagination");
  }
}
