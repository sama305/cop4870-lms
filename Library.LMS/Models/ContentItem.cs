using System;
namespace Library.LMS.Models;
public class ContentItem
{
    // properties
    public string Name { get; set; }
    public string Desc { get; set; }

    // constructor
    public ContentItem(string name, string desc)
	{
        Name = name;
        Desc = desc;
	}

    public override string ToString()
    {
        return $"{Name} (content)";
    }
}


// PageItem (subclass of ContentItem)
public class PageItem : ContentItem
{
    public string HTMLBody { get; set; }

    public PageItem(string hb, string name, string desc)
        : base(name, desc)
    {
        HTMLBody = hb;
    }

    public override string ToString()
    {
        return $"{Name} (page)";
    }
}


// AssignmentItem (subclass of ContentItem)
public class AssignmentItem : ContentItem
{
    public Assignment Assigned { get; set; }

    public AssignmentItem(Assignment assigned, string name, string desc)
        : base(name, desc)
    {
        Assigned = assigned;
    }

    public override string ToString()
    {
        return $"{Name} (assignment)";
    }
}


// PageItem (subclass of ContentItem)
public class FileItem : ContentItem
{
    public string FilePath { get; set; }

    public FileItem(string filePath, string name, string desc)
        : base(name, desc)
    {
        FilePath = filePath;
    }

    public override string ToString()
    {
        return $"{Name} (file)";
    }
}