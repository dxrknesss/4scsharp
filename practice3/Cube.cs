namespace practice3;

class Cube : Square // !!!!!! DON'T DO LIKE THIS, BECAUSE IT VIOLATES LISKOV SUBST. PRINCIPLE !!!!!!!!!
{
  public Cube(double Side) : base(Side) { }

  override public double Perimeter()
  {
    return _side * 4 * 6;
  }
}

