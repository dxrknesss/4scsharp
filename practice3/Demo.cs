namespace practice3;

class Demo
{
  static void Main(string[] args)
  {
    Shape circle1 = new Circle();
    Shape triangle1 = new Triangle(3.4, 5.3, new double[] { 3, 4, 5 });
    circle1.Draw();
    triangle1.Draw();
    System.Console.WriteLine(circle1);

    Shape rectangle1 = new Rectangle(new Point(3.5, 5), new Point(9, 2.5));
    Shape rectangle2 = new Rectangle(new Point(3.5, 5), new Point(9, 2.5));

    System.Console.WriteLine($"Are rectangles 1 and 2 equal? {rectangle1.Equals(rectangle2)}. R1 Hash: {rectangle1.GetHashCode()}; R2 Hash: {rectangle2.GetHashCode()}");
    rectangle2.Draw(); // doesn't do anything, because it's not overriden. Better mark shape class as abstract
    Rectangle rect2Copy = (Rectangle)rectangle2; // downcast to be use properties from Rectangle class
    rect2Copy.BottomRight = new Point(10, 8);
    System.Console.WriteLine($"Are rectangles 1 and 2 equal? {rectangle1.Equals(rectangle2)}. R1 Hash: {rectangle1.GetHashCode()}; R2 Hash: {rectangle2.GetHashCode()}");

    Square square1 = new Square(3);
    Square cube1 = new Cube(3); // upcast to parent type
    System.Console.WriteLine($"Perimeter of a square: {square1.Perimeter()} and of a cube with the same side: {cube1.Perimeter()}");
  }
}

