namespace practice2;

class Task11
{
  public static void TrinaglePS(double a, out double permieter, out double area)
  {
    permieter = a * 3;
    area = (Math.Pow(a, 2) * Math.Sqrt(3)) / Math.Sqrt(4);
  }
}
