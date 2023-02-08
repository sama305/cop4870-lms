using System;
namespace Library.LMS.Models;
public class ContentItem
{
    // properties
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Path { get; set; }

    // constructor
    public ContentItem(string name, string desc, string path)
	{
        Name = name;
        Desc = desc;
        Path = path;
	}
}


// PageItem (subclass of ContentItem)
public class PageItem : ContentItem
{
    public string HTMLBody { get; set; }

    public PageItem(string hb, string name, string desc, string path)
        : base(name, desc, path)
    {
        HTMLBody = hb;
    }
}


// AssignmentItem (subclass of ContentItem)
public class AssignmentItem : ContentItem
{
    public Assignment Assigned { get; set; }

    public AssignmentItem(Assignment assigned, string name, string desc, string path)
        : base(name, desc, path)
    {
        Assigned = assigned;
    }
}


// PageItem (subclass of ContentItem)
public class FileItem : ContentItem
{
    public string FilePath { get; set; }

    public FileItem(string filePath, string name, string desc, string path)
        : base(name, desc, path)
    {
        FilePath = filePath;
    }
}