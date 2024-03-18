namespace practice2;

class Task13
{
  public static int SumOfAllArgs(params int[] args)
  {
    int sum = 0;
    foreach (int num in args)
    {
      sum += num;
    }
    return sum;
  }
}

