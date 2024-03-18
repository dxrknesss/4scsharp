namespace practice2;

class Task14
{
  public static int DigitSum(int number)
  {
    if (number < 0)
    {
      number *= -1;
    }

    if (number < 10) // base case
    {
      return number;
    }

    return DigitSum(number % 10) + DigitSum(number / 10); // recursive case
  }
}

