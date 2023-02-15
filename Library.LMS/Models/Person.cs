namespace Library.LMS.Models;

public enum ClassRoles
{
    Student,
    TA,
    Instructor,
    FacultyGeneric
}

public class Person
{
    // fields
    private string? name;
    private ClassRoles role;

    // properties
    public string Name
    {
        get { return name ?? string.Empty;  }
        set { name = value; }
    }

    public ClassRoles Role
    {
        get { return role; }
        set { role = value; }
    }

    // constructor
    public Person(string name, ClassRoles role)
    {
        Name = name;
        Role = role;

    }

    public override string ToString()
    {
        return $"{Name}, a {Role}";
    }
}

public class Student : Person {
    private Dictionary<Assignment, float> gradesDict;

    public Student(string name)
        : base(name, ClassRoles.Student)
    {
        gradesDict = new Dictionary<Assignment, float>();
    }

    void AddAssignmentGrade(Assignment assignment, int grade)
    {
        float gradePercent = assignment.TotalPoints / grade;
        gradesDict.Add(assignment, gradePercent);
    }
}

public class Faculty : Person
{
    public Faculty(string name, ClassRoles role)
        : base(name, role)
    {
        if (role == ClassRoles.Student)
            Role = ClassRoles.FacultyGeneric;
    }
}

