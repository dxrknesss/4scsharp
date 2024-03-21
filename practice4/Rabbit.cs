namespace practice4;

class Rabbit
{
  public delegate void RabbitEventHandler(Point p);
  public event RabbitEventHandler ChangedLocation;

  Point _location;

  public Rabbit(Point Location)
  {
    this._location = Location;
  }

  public Point Location
  {
    get => _location;
    set
    {
      if (ChangedLocation != null)
      {
        ChangedLocation(value);
      }
      _location = value;
    }
  }
}

