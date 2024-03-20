namespace practice3;

class Point
{
  double _x;
  double _y;

  public double X
  {
    get => _x;
    set => _x = value;
  }

  public double Y
  {
    get => _y;
    set => _y = value;
  }

  public Point(double X, double Y)
  {
    this._x = X;
    this._y = Y;
  }

  override public string ToString()
  {
    return $"{_x}, {_y}";
  }

  override public bool Equals(object? obj)
  {
    if (obj is null || this.GetType() != obj.GetType()) // if types are different, objects are different
    {
      return false;
    }
    if (Object.ReferenceEquals(this, obj)) // compare by reference (== in java)
    {
      return true;
    }
    Point p = (Point)obj;

    return (_x == p._x) && (_y == p._y);
  }

  override public int GetHashCode()
  {
    return _x.GetHashCode() * 17 + _y.GetHashCode();
  }
}

