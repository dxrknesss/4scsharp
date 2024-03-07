namespace practice2;

class Task11
{
  static void Main(string[] args)
  {
    double a, p, s;
    Console.WriteLine("Please enter side of an equilateral triangle");
    a = Convert.ToDouble(Console.ReadLine());
    TrinaglePS(a, out p, out s);
    Console.WriteLine("P is {0} and S is {1}", p, s);
  }

  static void TrinaglePS(double a, out double permieter, out double area)
  {
    permieter = a * 3;
    area = (Math.Pow(a, 2) * Math.Sqrt(3)) / Math.Sqrt(4);
  }
}
