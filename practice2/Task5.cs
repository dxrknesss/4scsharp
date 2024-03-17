namespace practice2;

class Task5
{
    public static int SumOfAllSquaresBetweenInc(int low, int high)
    {
        int result = 0;
        for (int i = low; i <= high; i++)
        {
            result += (int) Math.Pow(i, 2);
        }
        return result;
    }
}
