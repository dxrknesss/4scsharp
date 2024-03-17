namespace practice2;

class Task3
{
  public static int[] ArrayOfOdds(int n)
  {
    if (n < 0)
    {
      throw new Exception("negative array size!");
    }

    int[] odds = new int[n];

    int oddNumber = 1;
    for (int i = 0; i < n; i++)
    {
      odds[i] = oddNumber;
      oddNumber += 2;
    }
    return odds;
  }
}
