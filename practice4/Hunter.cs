namespace practice4;

class Hunter
{
  private Point _location;

  public Point Location
  {
    get => _location;
    set => _location = value;
  }

  public static void OutputRabbitLocation(Point loc, Point oldLoc)
  {
    System.Console.WriteLine($"Hunter: Rabbit has changed location to: {loc}");
  }
}

