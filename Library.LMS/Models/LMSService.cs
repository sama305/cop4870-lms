using System;
namespace Library.LMS.Models
{
	public class LMSService
    {
        public List<Course> Courses;
        public List<Person> People;
        public List<List<Assignment>> AssignmentGroups;

        public LMSService()
        {
            Courses = new List<Course>();
            People = new List<Person>();
            AssignmentGroups = new List<List<Assignment>>();
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

        // <-- AddPerson -->
        public Person AddPerson(string name, ClassRoles role)
        {
            People.Add(new Person(name, role));
            return People.Last();
        }

        // <-- UpdatePerson -->
        public Person UpdatePerson(Person p, string name, ClassRoles role)
        {
            p.Name = name;
            p.Role = role;
            return p;
        }


    }
}

