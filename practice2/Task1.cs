namespace practice2;

class Task1
{
  static void Main(string[] args)
  {
    int a, b;
    CalculatePositiveNegativeAmount(3, -3, 5, out a, out b);
    Console.WriteLine("Positive {0}, negative {1}", a, b);
  }

  static void CalculatePositiveNegativeAmount(int a, int b, int c,
      out int positive, out int negative)
  {
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
