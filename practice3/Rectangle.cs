namespace practice3;

class Rectangle : Shape
{
  Point _topLeft; // composition examples!
  Point _bottomRight;

  public Rectangle(Point TopLeft, Point BottomRight)
  {
    this._topLeft = TopLeft;
    this._bottomRight = BottomRight;
  }

  public Point TopLeft
  {
    get => _topLeft;
    set => _topLeft = value;
  }

  public Point BottomRight
  {
    get => _bottomRight;
    set => _bottomRight = value;
  }

  override public string ToString()
  {
    return $"Rectangle with top left point at ({this._topLeft}) and bottom right at ({this._bottomRight})";
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
    Rectangle r = (Rectangle)obj;

    return _topLeft.Equals(r._topLeft) && _bottomRight.Equals(r._bottomRight);
  }

  override public int GetHashCode()
  {
    return _topLeft.GetHashCode() * 17 + _bottomRight.GetHashCode();
  }
}

