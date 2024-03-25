using System.Text.Json.Serialization;
namespace practice5;

[Serializable]
class Student
{
  [JsonPropertyName("FirstName")]
  public string Name { get; set; }
  public int Age { get; set; }
  [JsonIgnore]
  public Dictionary<string, float> Marks { get; set; }

  public Student()
  {
    this.Marks = new Dictionary<string, float>();
  }

  public Student(String Name, int Age)
  {
    this.Name = Name;
    this.Age = Age;
    this.Marks = new Dictionary<string, float>();
  }

  public Student(string Name, int Age, float MathGrade, float PhiGrade, float EngGrade)
  {
    this.Name = Name;
    this.Age = Age;
    this.Marks = new Dictionary<string, float>();
    Marks.Add("Mathematics", MathGrade);
    Marks.Add("Philosophy", PhiGrade);
    Marks.Add("English", EngGrade);
  }

  override public string ToString()
  {
    return $"{Name} {Age}, {String.Join(", ", Marks)}";
  }
}

