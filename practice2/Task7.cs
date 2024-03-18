namespace practice2;

public class Task7
{
  public static bool IsPowerOf3(int number)
  {
    double result = Math.Log(number, 3);
    return result == (int)result;
  }
}
