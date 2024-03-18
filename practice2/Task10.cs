namespace practice2;

class Task10
{
  public static uint InvertDigits(uint number)
  {
    uint tempNumber = number, result = 0;
    while (tempNumber >= 10)
    {
      uint remainder = tempNumber % 10;
      tempNumber /= 10;
      result = result * 10 + remainder;
    }
    result = result * 10 + tempNumber;

    return result;
  }
}

