namespace practice4;

class Rabbit : ILocatable
{
  public delegate void RabbitEventHandler(Point NewLoc, Point OldLoc);
  public event RabbitEventHandler ChangedLocation;

  Point _location;

  public Rabbit()
  { }

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

  public Point GetLocation()
  {
    return _location;
  }

  public void ChangeLocation(Point NewLocation)
  {
    Location = NewLocation;
  }
}

