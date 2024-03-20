namespace practice3;

class Circle : Shape
{
  double _radius { get; set; }

  public double Radius
  {
    get => _radius; // short syntax for get property
    set
    {
      _radius = value;
    }
  }

  public Circle()
  {
    this._radius = 0;
  }

  public Circle(double CenterX, double CenterY, double Radius) : base(CenterX, CenterY)
  {
    this._radius = Radius < 0 ? 0 : Radius;
  }

  public double GetCircumference(double Radius) // i.e. length of a circle
  {
    if (Radius < 0)
    {
      throw new Exception("Can't calculate circumference with negative radius!");
    }
    return 2 * Math.PI * Radius;
  }

  public double GetCircumference()
  {
    return GetCircumference(this._radius);
  }

  public Circle GetCicleCopy()
  {
    return (Circle)this.MemberwiseClone();
  }

  public Circle GetCicleCopy(double CenterX, double CenterY, double Radius)
  {
    return new Circle(CenterX, CenterY, Radius);
  }

  public bool IsPointOutsideCircle(double PointX, double PointY)
  { // if squared distance from point to center is bigger than radius squared, then the point is outside the circle
    return Math.Pow((PointX - this._centerPoint.X), 2) + Math.Pow((PointY - this._centerPoint.Y), 2)
      > Math.Pow(this._radius, 2);
  }

  override public string ToString()
  {
    return $"Circle with center coordinates: ({this._centerPoint}) and radius {this._radius}";
  }

  override public void Draw()
  {
    System.Console.WriteLine($"Drawing a circle with center cords ({this._centerPoint}) and radius {this._radius}");
  }
}

