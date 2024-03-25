using System.Text.Json;

namespace practice5;

class Demo
{
  static void Main(string[] args)
  {
    List<Student> Students = new List<Student>();
    Students.Add(new Student("John", 20, 90, 70, 100));
    Students.Add(new Student("Alice", 19, 85, 80, 95));
    Students.Add(new Student("Bob", 21, 75, 85, 90));
    Students.Add(new Student("Emily", 20, 95, 90, 80));
    Students.Add(new Student("Michael", 22, 80, 85, 75));
    Students.Add(new Student("Sophia", 19, 90, 95, 85));
    Students.Add(new Student("James", 21, 70, 75, 80));
    Students.Add(new Student("Olivia", 20, 85, 80, 60));
    Students.Add(new Student("Daniel", 22, 45, 90, 85));
    Students.Add(new Student("Emma", 19, 75, 70, 80));
    Students.Add(new Student("Alexander", 21, 80, 85, 90));
    Students.Add(new Student("Ava", 20, 30, 95, 45));
    Students.Add(new Student("William", 18, 70, 35, 80));
    Students.Add(new Student("Mia", 19, 85, 80, 90));
    Students.Add(new Student("Ethan", 19, 95, 90, 85));
    Students.Add(new Student("Isabella", 20, 75, 80, 80));

    System.Console.WriteLine("Students with name, longer than 4 chars and younger than 20:");
    System.Console.WriteLine(
        String.Join(",\n", from st in Students
                           where st.Name.Length > 4 && st.Age < 20
                           orderby st.Name descending
                           select st)
    );

    System.Console.WriteLine("\nStudents sorted by math grade:");
    System.Console.WriteLine(String.Join(",\n", Students.OrderBy(st => st.Marks["Mathematics"])));

    System.Console.WriteLine("\nStudents that didn't pass one or more exams:");
    System.Console.WriteLine(
        String.Join(",\n", from st in Students
                           where st.Marks["Mathematics"] < 60 || st.Marks["Philosophy"] < 60 || st.Marks["English"] < 60
                           select st
        )
    );

    System.Console.WriteLine("\nAverage grades by subjects:");
    var MathGrade = (from stud in Students
                     select stud.Marks["Mathematics"]).Average();
    var PhiGrade = (from stud in Students
                    select stud.Marks["Philosophy"]).Average();
    var EngGrade = (from stud in Students
                    select stud.Marks["English"]).Average();
    Dictionary<string, float> AverageGrades = new Dictionary<string, float>();
    AverageGrades.Add("Mathematics", MathGrade);
    AverageGrades.Add("Philosophy", PhiGrade);
    AverageGrades.Add("English", EngGrade);

    System.Console.WriteLine(
        String.Join(",\n", AverageGrades)
    );

    System.Console.WriteLine("\nNumber of students in each age group:");
    var StudentGroupsByAge = from st in Students
                             group st by st.Age into AgeGroups
                             orderby AgeGroups.Key
                             select AgeGroups;
    foreach (var Group in StudentGroupsByAge)
    {
      System.Console.WriteLine($"Age: {Group.Key}, Number of People: {Group.Count()}");
    }

    System.Console.WriteLine("Serializing this collection into students.json...");
    string OutJson = JsonSerializer.Serialize<List<Student>>(Students, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText("students.json", OutJson);
    System.Console.WriteLine("Serialization success!");

    System.Console.WriteLine("Deserializing students.json...");
    string InJson = File.ReadAllText("students.json");
    var InList = JsonSerializer.Deserialize<List<Student>>(InJson);
    System.Console.WriteLine(String.Join("\n", InList));
  }
}

