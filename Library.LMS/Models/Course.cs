namespace Library.LMS.Models;

public class Course
{

    // properties
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public int CreditHours { get; }

    public List<Module> Modules { get; set; }
    public List<Person> Roster { get; set; }
    public List<Assignment> Assignments { get; set; }
    public List<AssignmentGroup> AssignmentGroups { get; }
    public List<Announcement> Announcements { get; }

    // constructor
    public Course(string name, string code, string description, int hours)
	{
        Name = name;
        Code = code;
        Description = description;
        CreditHours = hours;

        Modules = new List<Module>();
        Roster = new List<Person>();
        Assignments = new List<Assignment>();
        AssignmentGroups = new List<AssignmentGroup>();
    }

    // methods
    public void AddPerson(Person person)
    {
        Roster.Add(person);
    }

    public void RemovePerson(Person p)
    {
        Roster.Remove(p);
    }


    // CRUD for module
    public void AddModule(Module module)
    {
        Modules.Add(module);
    }

    public void UpdateModule(Module module, string? name =null, string? desc=null)
    {
        module.Name = name ?? module.Name;
        module.Desc = desc ?? module.Desc;
    }

    public void RemoveModule(Module module)
    {
        Modules.Remove(module);
    }

    public void AddAssignment(Assignment assignment)
    {
        Assignments.Add(assignment);
    }

    public void AddAssignment(string name, string desc, int totalPoints, DateTime due, AssignmentGroup group)
    {
        Assignments.Add(new Assignment(this, name, desc, totalPoints, due, group));
    }

    public void RemoveAssignment(Assignment assignment)
    {
        Assignments.Remove(assignment);
    }

    public AssignmentGroup AddAssignmentGroup(string n, double v)
    {
        AssignmentGroups.Add(new AssignmentGroup(n, v));
        return AssignmentGroups.Last();
    }

    public Announcement AddAnnouncement(string h, string c)
    {
        Announcements.Add(new Announcement(h, c));
        return Announcements.Last();
    }

    public void RemoveAnnouncement(Announcement a)
    {
        Announcements.Remove(a);
    }



    public override string ToString()
    {
        return $"{Name} ({Code})";
    }
}

