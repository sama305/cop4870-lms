namespace Library.LMS.Models;

public class Course
{
    // fields
    private List<Module> modules;
    private List<Person> roster;
    private List<Assignment> assignments;

    // properties
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public List<Person> GetRoster()
    {
        return roster;
    }

    // constructor
    public Course(string name, string code, string description)
	{
        Name = name;
        Code = code;
        Description = description;

        modules = new List<Module>();
        roster = new List<Person>();
        assignments = new List<Assignment>();
	}

    // methods
    public void AddPerson(Person person)
    {
        roster.Add(person);
    }

    public void RemovePerson(Person p)
    {
        roster.Remove(p);
    }

    public void AddModule(Module module)
    {
        modules.Add(module);
    }

    public void AddAssignment(ref Assignment assignment)
    {
        assignments.Add(assignment);
    }

    public override string ToString()
    {
        return $"{Name} ({Code})";
    }
}

