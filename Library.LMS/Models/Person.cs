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
    public int ID { get; }

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
    public Person(string name, ClassRoles role, int id)
    {
        Name = name;
        Role = role;
        ID = id;
    }

    public override string ToString()
    {
        return $"{Name}, a {Role} (id:{ID})";
    }
}

public class Student : Person {
    public Dictionary<Assignment, double> AssignmentsDict { get; }
    //public Dictionary<Course, double> GradesDict { get;  }

    public Student(string name, int id)
        : base(name, ClassRoles.Student, id)
    {
        AssignmentsDict = new Dictionary<Assignment, double>();
        //GradesDict = new Dictionary<Course, double>();
    }

    public void AddAssignmentGrade(Assignment assignment, int grade)
    {
        // Normalize the grade to be in terms of 100%
        double gradeRatio = (double)grade / assignment.TotalPoints * 100;

        try
        {
            AssignmentsDict.Add(assignment, grade);
        }
        catch (ArgumentException)
        {
            return;
        }
    }

    public override string ToString()
    {
        return $"{Name}, a {Role} (id:{ID})";
    }
}

public class Faculty : Person
{
    public Faculty(string name, ClassRoles role, int id)
        : base(name, role, id)
    {
        if (role == ClassRoles.Student)
            Role = ClassRoles.FacultyGeneric;
    }
}

