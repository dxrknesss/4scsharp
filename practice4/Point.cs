namespace practice4;

class Point
{
  int _x;
  int _y;

  public int X
  {
    get => _x;
    set => _x = value;
  }

  public int Y
  {
    get => _y;
    set => _y = value;
  }

  public Point(int X, int Y)
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

