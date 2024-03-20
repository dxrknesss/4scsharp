namespace practice3;

class Triangle : Shape
{
  /* awful practice, where do each side start or end?
   * for the sake of being mathematically correct or plain logical,
   * a class representing a "line" should be created.
   * it should store two "points" (additional class needs to be created as well!),
   * each one representing start and the end of the line.
   * then, compose triangle of 3 lines instead of 3 numbers.
   * Oleksii Orlov, 20.03.2024 23:15 :)
   */
  double[] _sideLengths;

  public double[] SideLengths
  {
    get => _sideLengths;
    set
    {
      if (value.Length != 3)
      {
        throw new Exception("Only array of size 3 can be assigned to this field!");
      }
      _sideLengths = value;
    }
  }

  public Triangle()
  {
    this._sideLengths = new double[3] { 0, 0, 0 };
  }

  public Triangle(double CenterX, double CenterY, double[] SideLengths) : base(CenterX, CenterY)
  {
    this._sideLengths = SideLengths;
  }

  override public void Draw()
  {
    System.Console.WriteLine($"Drawing triangle with center cords: ({this._centerPoint}) and sides {{{String.Join(", ", this._sideLengths)}}}");
  }
}

