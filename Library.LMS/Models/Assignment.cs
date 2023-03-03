namespace Library.LMS.Models;

public class Assignment
{
    // properties
    public Course ParentCourse { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int TotalPoints { get; set; }
    public DateTime DueDate { get; set; }

    public AssignmentGroup Group { get; set; }

    // constructor
    public Assignment(Course course, string name, string description,
        int totalPoints, DateTime dueDate, AssignmentGroup group)
	{
        ParentCourse = course;
        Name = name;
        Description = description;
        TotalPoints = totalPoints;
        DueDate = dueDate;
        Group = group;
	}

    // overrides
    public override string ToString()
    {
        return $"{Name}, due {DueDate} ({Group.Name})";
    }
}

public class AssignmentGroup
{
    public string Name { get; }
    public double Weight { get; }

    public AssignmentGroup(string name, double weight)
    {
        Name = name;
        Weight = (weight >  1.0) ? weight / 100.0 : (weight < 0.0) ? 0.0 : weight;
    }

    public override string ToString()
    {
        return $"{Name}, {Weight * 100.0}%";
    }
}