using System;
namespace Library.LMS.Models
{
	public class LMSService
    {
        public List<Course> Courses;
        public List<Person> People;

        public List<Student> Students
        {
            get
            {
                List<Student> students = new List<Student>();
                foreach(Person p in People)
                {
                    if (p.GetType() == typeof(Student))
                        students.Add((Student)p);
                }
                return students;
            }
        }

        private int idCount;

        public LMSService()
        {
            Courses = new List<Course>();
            People = new List<Person>();
            idCount = 0;
        }


        // Class Methods

        // <-- AddCourse -->
        public Course AddCourse(string name, string code, string desc, int hours)
        {
            Courses.Add(new Course(name, code, desc, hours));
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
                    (p as Student).AssignmentsDict.Clear();
            });
            Courses.Remove(c);
        }

        // <-- AddPerson -->
        public Person AddPerson(string name, ClassRoles role)
        {
            Person p = new Person(name, role, idCount);
            switch (role)
            {
                case ClassRoles.Student:
                    p = new Student(name, idCount);
                    break;
                case ClassRoles.TA:
                case ClassRoles.Instructor:
                    p = new Faculty(name, role, idCount);
                    break;
            }
            People.Add(p);

            idCount++; // increment id
            return People.Last();
        }

        // <-- UpdatePerson -->
        public Person UpdatePerson(Person p, string name, ClassRoles role)
        {
            p.Name = name;
            p.Role = role;
            return p;
        }

        // <-- RemovePerson -->
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

        // <-- GetEnrolledCourses -->
        public List<Course> GetEnrolledCourses(Person p, bool invert=false)
        {
            List<Course> enrolled = Courses.Where(c => c.Roster.Contains(p)).ToList();

            // If invert = true, return all courses the person isn't in
            // Otherwise, return the course a person is in
            return (invert) ? Courses.Except(enrolled).ToList() : enrolled;

        }


        // <-- GetStudentGPA -->
        public double GetStudentGPA(Student s)
        {
            List<Course> courses = GetEnrolledCourses(s);

            int creditHours = 0;
            int gradePoints = 0;
            int qualityPoints = 0;
            foreach(Course c in courses)
            {
                creditHours += c.CreditHours;

                gradePoints = GetGradePoints(GetCourseAverage(s, c));

                qualityPoints += gradePoints * c.CreditHours;
            }

            return (double)qualityPoints / creditHours;
        }

        public int GetGradePoints(double grade)
        {
            // Get the grade points by converting raw grade to grade point
            // A - 4, B - 3, C - 2, D - 1, F - 0
            return Math.Max((int)(grade / 20 - 1), 0);
        }

        public double GetCourseAverage(Student s, Course c)
        {
            // Filter completed assginments that are in the desired course
            List<Assignment> completedAssignments = s.AssignmentsDict.Keys
                .Where(a => a.ParentCourse == c).ToList();

            double weightedAverage = 0;
            foreach (Assignment a in completedAssignments)
            {
                weightedAverage += s.AssignmentsDict[a] * (a.Group.Weight);
            }

            return weightedAverage;
        }



        public void PrintPeopleDetailed(List<Person> pl)
        {
            // Example:
            //
            // 1.   Name/ID: Samuel Anderson (#0)
            //      Status: Student
            //      GPA: 3
            //      Classes:
            //          Biology I       90%
            //          Discrete Math   78%
            //
            // 2.   Name/ID: Robert Romero (#1)
            //      Status: Instructor
            //      Classes:
            //          Biology I
            //  etc.

            for (int i = 0; i < pl.Count(); i++)
            {
                Person p = pl[i];
                Console.WriteLine($"{i+1}.\tName/ID: {p.Name} (#{p.ID})");
                if (p.Role == ClassRoles.Student)
                    Console.WriteLine($"\tGPA: {GetStudentGPA(p as Student)}");
                Console.WriteLine($"\tStatus: {p.Role}");
                Console.WriteLine($"\tClasses:");
                foreach (Course c in GetEnrolledCourses(p))
                {
                    Console.Write($"\t\t{c.Name}");
                    if (p.Role == ClassRoles.Student)
                        Console.Write($"\t{GetCourseAverage(p as Student, c)}%");
                }
                Console.Write("\n");
            }
        }
    }
}

