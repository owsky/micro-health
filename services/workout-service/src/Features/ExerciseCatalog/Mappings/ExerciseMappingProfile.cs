using AutoMapper;
using WorkoutService.Features.ExerciseCatalog.Domain;
using WorkoutService.Features.ExerciseCatalog.Dtos;

namespace WorkoutService.Features.ExerciseCatalog.Mappings;

public class ExerciseMappingProfile : Profile
{
  public ExerciseMappingProfile()
  {
    CreateMap<Exercise, ExerciseResponse>().ReverseMap();
  }
}
