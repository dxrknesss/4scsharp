namespace practice3;

class Square : Shape
{
  // this class could be based on a rectangle, but for this practice, it's just double value
  protected double _side;

  public double Side
  {
    get => _side;
    set
    {
      if (value < 0)
      {
        throw new Exception("Can't assign negative value to the square side!");
      }
      _side = value;
    }
  }

  public Square(double Side)
  {
    this._side = Side;
  }


  public virtual double Perimeter()
  {
    return _side * 4;
  }
}

