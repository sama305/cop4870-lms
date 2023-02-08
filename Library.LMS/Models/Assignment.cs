﻿namespace Library.LMS.Models;

public class Assignment
{
    // properties
    public string Name { get; set; }
    public string Description { get; set; }
    public uint TotalPoints { get; set; }
    public DateTime DueDate { get; set; }

    // constructor
    public Assignment(string name, string description,
        uint totalPoints, DateTime dueDate)
	{
        Name = name;
        Description = description;
        TotalPoints = totalPoints;
        DueDate = dueDate;
	}

    // overrides
    public override string ToString()
    {
        return $"{Name} (due {DueDate})";
    }
}