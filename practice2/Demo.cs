namespace practice2;

class DemoClass
{
    static void Main(string[] args)
    {
        Task1Example();
        Task2Example();
        Task3Example();
        Task4Example();
        Task5Example();
        Task6Example();
        Task11Example();
        Console.ReadKey();
    }

    static void Task1Example()
    {
        int a, b;
        Task1.CalculatePositiveNegativeAmount(3, -3, 5, out a, out b);
        Console.WriteLine($"Positive: {a} numbers, negative: {b} numbers");
    }

    static void Task2Example()
    {
        double a = 3.4, b = 8.3, c = 12.5;
        Task2.PerformDoubleTask(a, b, c);
    }

    static void Task3Example()
    {
        Console.WriteLine($"Array of odd numbers with size 5: {String.Join(',', Task3.ArrayOfOdds(5))}");
    }

    static void Task4Example()
    {
        int[,] matrix = Task4.CreateSquareMatrix(5);
        Task4.ReadOddRows(matrix);
    }

    static void Task5Example()
    {
        Console.WriteLine($"Sum of all squares between 1 and 5 is: {Task5.SumOfAllSquaresBetweenInc(1, 5)}"); // should return 55
    }

    static void Task6Example()
    {
        Task6.OutputNumberNTimes(1, 6);
        Console.WriteLine();
    }
    
    static void Task7Example()
    {
        
    }

    static void Task11Example()
    {
        double a, p, s;
        Console.WriteLine("Please enter side of an equilateral triangle");
        a = Convert.ToDouble(Console.ReadLine());
        Task11.TrinaglePS(a, out p, out s);
        Console.WriteLine("P is {0} and S is {1}", p, s);
    }
}
