using System;
namespace Library.LMS.Models
{
	public class LMSService
    {
        public List<Course> Courses;
        public List<Person> People;
        public List<Tuple<string, List<Assignment>>> AssignmentGroups;

        public LMSService()
        {
            Courses = new List<Course>();
            People = new List<Person>();
            AssignmentGroups = new List< Tuple< string, List<Assignment> > >();
        }


        // Class Methods

        // <-- AddCourse -->
        public Course AddCourse(string name, string code, string desc)
        {
            Courses.Add(new Course(name, code, desc));
            return Courses.Last();
        }

        // <-- UpdateCourse -->
        public Course UpdateCourse(Course c, string name, string code, string desc)
        {
            c.Name = name;
            c.Code = code;
            c.Description = desc;
            return c;
        }

        // <-- RemoveCourse -->
        public void RemoveCourse(Course c)
        {
            c.Assignments.Clear();
            c.Modules.Clear();
            c.Roster.ForEach(p => {
                if (p.GetType() == typeof(Student))
                    (p as Student).GradesDict.Clear();
            });
            Courses.Remove(c);
        }

        // <-- AddPerson -->
        public Person AddPerson(string name, ClassRoles role)
        {
            Person p = new Person(name, role);
            switch (role)
            {
                case ClassRoles.Student:
                    p = new Student(name);
                    break;
                case ClassRoles.TA:
                case ClassRoles.Instructor:
                    p = new Faculty(name, role);
                    break;
            }
            People.Add(p);
            return People.Last();
        }

        // <-- UpdatePerson -->
        public Person UpdatePerson(Person p, string name, ClassRoles role)
        {
            p.Name = name;
            p.Role = role;
            return p;
        }

        public void RemovePerson(Person p)
        {
            foreach(Course c in Courses)
            {
                if (c.Roster.Contains(p))
                {
                    c.Roster.Remove(p);
                }
            }
            // TODO: determine if you need to remove grades
            People.Remove(p);
        }

        public void AddAssignmentGroup(string name)
        {
            AssignmentGroups
                .Add(new Tuple<string, List<Assignment> >(name, new List<Assignment>()));
        }

        public void AddAssignmentToGroup(string name, Assignment a)
        {
            AssignmentGroups.Find(m => m.Item1.Equals(name)).Item2.Add(a);
        }
    }
}

