namespace practice2;

class Task1
{
  public static void CalculatePositiveNegativeAmount(int a, int b, int c,
      out int positive, out int negative)
  {
    Console.WriteLine($"Input numbers: {a}, {b}, {c}");
    positive = 0;
    negative = 0;
    int[] input = { a, b, c };
    for (int i = 0; i < 3; i++)
    {
      if (input[i] >= 0)
      {
        positive++;
      }
      else
      {
        negative++;
      }
    }
  }
}
