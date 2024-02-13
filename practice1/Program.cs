Console.WriteLine("TASK 1");
int x1 = 123; // some value for x1
Console.WriteLine(x1);

Console.WriteLine("TASK 2");
string str1;
Console.WriteLine("What is your name");
str1 = Console.ReadLine();
string str2 = "Good afternoon, " + str1;
Console.WriteLine(str2);

Console.WriteLine("TASK 3");
var v1 = 'v';
v1 = 'e';
Console.WriteLine(v1);

Console.WriteLine("TASK 4");
int squareSideCm = 4;
System.Console.WriteLine("Perimeter of a square is: " + squareSideCm * 4);

Console.WriteLine("TASK 5");
Console.WriteLine("Please input x and y in following format `x,y`");
string[] xAndY = Console.ReadLine().Split(','); // undefined type, but we belive in user's intelligence
int x = Convert.ToInt32(xAndY[0]);
int y = Convert.ToInt32(xAndY[1]);
Console.WriteLine("Mean of x and y is: " + (x + y) / 2.0);

Console.WriteLine("TASK 6");
Console.WriteLine("Please input r1 and r2 in following format `r1,r2`");
string[] r1AndR2 = Console.ReadLine().Split(','); // undefined type, but we belive in user's intelligence
double r1 = Convert.ToDouble(r1AndR2[0]);
double r2 = Convert.ToDouble(r1AndR2[1]);
const double pi = 3.14;

double s1 = pi * Math.Pow(r1, 2);
double s2 = pi * Math.Pow(r2, 2);
double s3 = s1 > s2 ? s1 - s2 : s2 - s1;
Console.WriteLine(s3);

Console.WriteLine("TASK 7");
int twoDigitNumber = 37;
Console.WriteLine("First digit: " + twoDigitNumber / 10);
Console.WriteLine("Second digit: " + twoDigitNumber % 10);

Console.WriteLine("TASK 8");
int nSecondSinceDayStart = 8000;
Console.WriteLine("Since the start of a day " + nSecondSinceDayStart / 3600 + " full hours have passed");

Console.WriteLine("TASK 9");
int a = 3;
int b = 10;
int c = 14;
bool result = b > a ? b < c : b > c;
Console.WriteLine(result);

Console.WriteLine("TASK 10");
int positiveInteger = 684;
int positiveInteger1 = 6849;
int positiveInteger2 = 339;

bool checkIfOdd3DigitInteger(int number)
{
  int lastNumber, digitCounter = 0;
  lastNumber = Math.Abs(number % 10);

  while (number >= 10)
  {
    number /= 10;
    digitCounter++;
  }
  return lastNumber % 2 == 1 && digitCounter % 2 == 0;
}
Console.WriteLine(checkIfOdd3DigitInteger(positiveInteger));
Console.WriteLine(checkIfOdd3DigitInteger(positiveInteger1));
Console.WriteLine(checkIfOdd3DigitInteger(positiveInteger2));

Console.WriteLine("TASK 11");
int integer1 = 15432;
int integer2 = 53845123;
long sum = integer1 + integer2;
Console.WriteLine(sum);

Console.WriteLine("TASK 12");
long long1 = 234958123;
long long2 = 5342352345;
byte product = (byte)(long1 * long2);
Console.WriteLine(product);
