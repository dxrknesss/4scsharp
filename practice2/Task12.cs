namespace practice2;

class Task12
{
  public static void MinMax(ref double x, ref double y)
  {
    if (x <= y)
    {
      double temp = x;
      x = y;
      y = temp;
    }
  }
}

