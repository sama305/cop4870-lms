namespace Library.LMS.Models;

public class Person
{
    // fields
    private string? name;
    private string? classification;
    private Dictionary<Assignment, float> gradesDict;

    // properties
    public string Name
    {
        get { return name ?? string.Empty;  }
        set { name = value; }
    }

    public string Classification
    {
        get { return classification ?? "NULL CLASSIFICATION"; }
        set { classification = value; }
    }

    // constructor
    public Person(string name, string classification)
    {
        Name = name;
        Classification = classification;

        gradesDict = new Dictionary<Assignment, float>();
    }

    // methods
    void AddAssignmentGrade(ref Assignment assignment, int grade)
    {
        float gradePercent = assignment.TotalPoints / grade;
        gradesDict.Add(assignment, gradePercent);
    }



    public override string ToString()
    {
        return $"{Name}, a {Classification.ToLower()}";
    }
}

