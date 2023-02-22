namespace Library.LMS.Models;

public class Course
{
    // fields
    public List<Module> Modules { get; set; }
    public List<Person> Roster { get; set; }
    public List<Assignment> Assignments;

    // properties
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public List<Assignment> GetAssignments()
    { return Assignments; }

    // constructor
    public Course(string name, string code, string description)
	{
        Name = name;
        Code = code;
        Description = description;

        Modules = new List<Module>();
        Roster = new List<Person>();
        Assignments = new List<Assignment>();
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

    public override string ToString()
    {
        return $"{Name} ({Code})";
    }
}

