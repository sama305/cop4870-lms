namespace Library.LMS.Models;

public class Assignment
{
    // fields
    //private string? name;
    //private string? description;
    //private int totalPoints;
    //private string? dueDate;

    // properties
    public string Name { get; set; }
    public string Description { get; set; }
    public uint TotalPoints { get; set; }
    public string DueDate { get; set; }

    // constructor
    public Assignment(string name, string description,
        uint totalPoints, string dueDate)
	{
        Name = name;
        Description = description;
        TotalPoints = totalPoints;
        DueDate = dueDate;
	}
}