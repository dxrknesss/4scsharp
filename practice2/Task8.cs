namespace practice2;

class Task8
{
  public static void SolveProblem(double percent, out int daysBefore200Km, out double totalDistanceKm)
  {
    totalDistanceKm = 10;
    daysBefore200Km = 1;
    double prevDay = 10;

    while (totalDistanceKm <= 200)
    {
      totalDistanceKm += prevDay * (percent / 100 + 1);
      daysBefore200Km++;
    }
  }
}
