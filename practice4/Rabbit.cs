namespace practice4;

class Rabbit
{
  public delegate void RabbitEventHandler(Point NewLoc, Point OldLoc);
  public event RabbitEventHandler ChangedLocation;
  byte _carrotCount = 0;

  Point _location;

  public Rabbit()
  { }

  public byte Carrots
  {
    get => _carrotCount;
    set => _carrotCount = value;
  }

  public Point Location
  {
    get => _location;
    set
    {
      Point temp = _location;
      if (ChangedLocation != null)
      {
        ChangedLocation(value, _location);
      }
      _location = value;
    }
  }
}

