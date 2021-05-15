namespace Global
{
  public class Utility
  {
    public static float Abs(float number)
    {
      if(number < 0)
      {
        return number * -1f;
      }
      return number;
    }

    public static float Clamp(float number, float min, float max)
    {
      if(number < min)
      {
        return min;
      }
      else if(number > max)
      {
        return max;
      }

      return number;
    }
  }
}