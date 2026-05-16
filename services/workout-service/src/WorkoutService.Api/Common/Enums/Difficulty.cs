namespace WorkoutService.Common.Enums;

public enum Difficulty
{
  VeryEasy,
  Easy,
  Medium,
  High,
  VeryHigh,
}

internal static class DifficultyMethods
{
  extension(Difficulty difficulty)
  {
    public static Difficulty AggregateDifficulty(Difficulty a, Difficulty b)
    {
      if (a < b)
        return b;
      return a;
    }
  }
}
