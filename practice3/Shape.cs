namespace practice3;

class Shape
{
  protected Point _centerPoint;

  public Shape() { }

  public Shape(double CenterX, double CenterY)
  {
    this._centerPoint = new Point(CenterX, CenterY);
  }

  public virtual void Draw() { }
}

