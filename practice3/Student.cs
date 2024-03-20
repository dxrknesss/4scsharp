namespace practice3;

class Student
{
  string _name;
  byte _course;
  bool _hasScholarship;

  public Student(string Name, byte Course, bool HasScholarship)
  {
    this._name = Name;
    this._course = Course;
    this._hasScholarship = HasScholarship;
  }

  public Student(string Name, byte Course)
  {
    this._name = Name;
    this._course = Course;
  }

  public Student(string Name)
  {
    this._name = Name;
  }
}

