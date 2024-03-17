namespace practice2;

class Task6 
{
    public static int OutputNumberNTimes(int a, int b)
    {
        if (a == b+1)
        {
            return 0;
        }
        else
        {
            for (int i = 0; i < a; i++)
            {
                Console.Write(a + " ");
            }
            return OutputNumberNTimes(a + 1, b);
        }
    }
}
