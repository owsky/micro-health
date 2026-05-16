using AutoMapper;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.WorkoutTemplates.Domain;
using WorkoutService.Features.WorkoutTemplates.Dtos;

namespace WorkoutService.Features.WorkoutTemplates.Mappings;

public class WorkoutTemplateMappingProfile : Profile
{
  public WorkoutTemplateMappingProfile()
  {
    CreateMap<WorkoutTemplate, WorkoutTemplateResponse>();
    CreateMap<WorkoutTemplateExercise, Exercise>();
  }
}
