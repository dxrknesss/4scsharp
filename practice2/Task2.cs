namespace practice2;

class Task2
{
  public static void PerformDoubleTask(double a, double b, double c)
  {
    double[] input = { a, b, c };
    if ((a < b && b < c) || (a > b && b > c))
    {
      for (int i = 0; i < input.Length; i++)
      {
        input[i] *= 2;
      }
    }
    else
    {
      for (int i = 0; i < input.Length; i++)
      {
        input[i] *= -1;
      }
    }

    Console.WriteLine($"a: {input[0]}, b: {input[1]}, c: {input[2]}");
  }
}
