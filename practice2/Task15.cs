namespace practice2;

class Task15
{
  public enum Operations
  {
    add, sub, mul, div
  };

  public static double Calculate(double a, double b, Operations op)
  {
    double result = 0;
    switch (op)
    {
      case Operations.add:
        result = a + b;
        break;
      case Operations.sub:
        result = a - b;
        break;
      case Operations.mul:
        result = a * b;
        break;
      case Operations.div:
        if (b == 0)
        {
          throw new Exception("Division by 0!");
        }
        else
        {
          result = a / b;
        }
        break;
      default:
        throw new Exception("Unsupported operation!");
    }
    return result;
  }
}
