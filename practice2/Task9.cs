namespace practice2;

class Task9
{
  public static double Sum()
  {
    System.Console.WriteLine("Enter two numbers separated by space");
    string[] input = Console.ReadLine().Split(" ");
    double a = Convert.ToDouble(input[0]), b = Convert.ToDouble(input[1]);
    return a + b;
  }
}
