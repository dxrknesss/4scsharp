namespace practice2;

using Ops = Task15.Operations;

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
    Task7Example();
    Task8Example();
    Task9Example();
    Task10Example();
    Task11Example();
    Task12Example();
    Task13Example();
    Task14Example();
    Task15Example();
    Task16Example();
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
    Console.WriteLine($"Is number 12 a power of 3? {Task7.IsPowerOf3(12)}");
    Console.WriteLine($"Is number 9 a power of 3? {Task7.IsPowerOf3(9)}");
  }

  static void Task8Example()
  {
    int days;
    double total;
    Task8.SolveProblem(35, out days, out total);
    System.Console.WriteLine($"With 35% increase each day, skier will reach 200km in {days} days and the total distance will be {total}km");
  }

  static void Task9Example()
  {
    var res = Task9.Sum();
    System.Console.WriteLine($"Sum of numbers is {res}");
  }

  static void Task10Example()
  {
    System.Console.WriteLine($"Number 12345 reversed: {Task10.InvertDigits(12345)}");
  }

  static void Task11Example()
  {
    double a, p, s;
    Console.WriteLine("Please enter side of an equilateral triangle");
    a = Convert.ToDouble(Console.ReadLine());
    Task11.TrinaglePS(a, out p, out s);
    Console.WriteLine("P is {0} and S is {1}", p, s);
  }

  static void Task12Example()
  {
    double a = 3, b = 5;
    Task12.MinMax(ref a, ref b);
    System.Console.WriteLine($"{a} is greater than {b}");
  }

  static void Task13Example()
  {
    System.Console.WriteLine($"Sum of all params will be {Task13.SumOfAllArgs(3, 56, 1, 2, 5, 1, 2, 4, 6)}");
  }

  static void Task14Example()
  {
    System.Console.WriteLine($"Sum of all digits in number 12345 is: {Task14.DigitSum(12345)}");
  }

  static void Task15Example()
  {
    System.Console.WriteLine($"Result of 12345 + 67890: {Task15.Calculate(12345, 67890, Ops.add)}");
    System.Console.WriteLine($"Result of 12345 - 67890: {Task15.Calculate(12345, 67890, Ops.sub)}");
    System.Console.WriteLine($"Result of 12345 * 67890: {Task15.Calculate(12345, 67890, Ops.mul)}");
    try
    {
      System.Console.WriteLine($"Result of 12345 / 0: {Task15.Calculate(12345, 0, Ops.div)}");
    }
    catch (Exception e)
    {
      System.Console.WriteLine(e.Message);
    }
    System.Console.WriteLine($"Result of 12345 / 10000: {Task15.Calculate(12345, 10000, Ops.div)}");
  }

  static void Task16Example()
  {
    Product[] products = new Product[3];
    products[0].ProductName = "carrots";
    products[0].AdditionDate = DateOnly.FromDateTime(DateTime.Now);
    products[0].WeightKg = 400.5;
    products[0].PricePer1KgUSD = 3.4m;
    products[0].SupplierName = "BestCarrots";
    products[0].MaxStoragePeriodDays = 100;

    products[1] = new Product
    (
        "tomatoes",
        DateOnly.FromDateTime(DateTime.Now),
        900,
        2.5m,
        "RedMates",
        10
    );

    products[2] = new Product
    (
        "apples",
        DateOnly.FromDateTime(DateTime.Now),
        2000,
        1000m,
        "Apple Inc.",
        0
    );

    foreach (Product p in products)
    {
      System.Console.WriteLine(p);
    }
  }
}
