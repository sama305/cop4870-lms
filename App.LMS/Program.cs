using System;
using System.Xml.Linq;
using Library.LMS.Models;

namespace App.LMS
{
    internal class Program
    {

        static void Main(string[] args)
        {

            LMSService svc = new LMSService();

            DialogueTree dt = new DialogueTree();

            dt.CurrentNode.createMultiChild("Person options")
                          .createMultiChild("Course options");
            // TODO: Add node getter


            // <------- "Person options" ------->
            dt.CurrentNode = dt.RootNode.Children[0];
            dt.CurrentNode.createMultiChild("Add a person")
                          .createMultiChild("Update a person")
                          .createMultiChild("Delete a person")
                          .createMultiChild("Find a person")
                          .createMultiChild("List all people");

            dt.CurrentNode = dt.CurrentNode.Children[1]; // "Update a person"
            dt.CurrentNode.createMultiChild("Update person details")
                          .createMultiChild("Manage courses")
                          .createMultiChild("Manage assignments");

            dt.CurrentNode = dt.CurrentNode.Children[1]; // "Manage courses"
            dt.CurrentNode.createMultiChild("Enroll in a course")
                          .createMultiChild("Unenroll from a course");

            dt.goUp();
            dt.CurrentNode = dt.CurrentNode.Children[2]; // "Manage assignments"
            dt.CurrentNode.createMultiChild("Add a grade")
                          .createMultiChild("Remove a grade");
            // <------- "Person options" ------->


            // <------- "Course options" ------->
            dt.CurrentNode = dt.RootNode.Children[1];
            dt.CurrentNode.createMultiChild("Add a course")
                          .createMultiChild("Update a course and it's components")
                          .createMultiChild("Delete a course")
                          .createMultiChild("Find a course")
                          .createMultiChild("List all courses");

            dt.CurrentNode = dt.CurrentNode.Children[1]; // "Update a course and it's components"
            dt.CurrentNode.createMultiChild("Update course details")
                          .createMultiChild("Manage members")
                          .createMultiChild("Manage assignments")
                          .createMultiChild("Manage modules");

            dt.CurrentNode = dt.CurrentNode.Children[1];
            dt.CurrentNode.createMultiChild("Enroll in a course")
                          .createMultiChild("Unenroll from a course");

            dt.goUp();
            dt.CurrentNode = dt.CurrentNode.Children[2];
            dt.CurrentNode.createMultiChild("Add an assignment")
                          .createMultiChild("Remove an assignment");

            dt.goUp();
            dt.CurrentNode = dt.CurrentNode.Children[3];
            dt.CurrentNode.createMultiChild("Add a module")
                          .createMultiChild("Remove a module")
                          .createMultiChild("Add content item")
                          .createMultiChild("Remove content item");


            dt.CurrentNode = dt.RootNode;
            // <------- "Course options" ------->

            Node selected = dt.RootNode;
            while (true)
            {
                Console.Clear();
                selected = dt.chooseChildFromMenu(selected, true);

                Console.Clear();
                switch (selected.Data)
                {
                    // People options
                    case "Add a person":
                        {
                            // Choosing fields for the person
                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Class Role (Student, TA, or Instructor): ");
                            var roleStr = Console.ReadLine();
                            if (Enum.TryParse<ClassRoles>(roleStr, true, out ClassRoles roleCnvt))
                            {
                                // Adding the person to the master list
                                svc.AddPerson(name, roleCnvt);
                                Console.WriteLine($"{svc.People.Last()}, has been created.");
                            }
                            else
                            {
                                Console.WriteLine($"Role {roleStr} not found");
                            }
                        }
                        break;
                    case "Update person details":
                        {
                            // Selecting the person
                            Console.WriteLine("Which person to update?: ");
                            Person person = ChooseFromMenuList(svc.People);
                            if (person == null) { break; }

                            Console.WriteLine($"You have chosen to update {person}.");

                            // Person's fields
                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Class Role: ");
                            if (Enum.TryParse(Console.ReadLine(), out ClassRoles roleCnvt))
                            {
                                // Updating the person
                                svc.UpdatePerson(person, name, roleCnvt);
                                Console.WriteLine($"{person} has been updated.");
                            }
                        }
                        break;
                    case "Delete a person":
                        {
                            // Selecting the person
                            Console.WriteLine("Which person to delete?: ");
                            Person person = ChooseFromMenuList(svc.People);
                            if (person == null) { break; }

                            Console.WriteLine($"You have chosen to delete {person}.");

                            svc.RemovePerson(person);
                        }
                        break;
                    case "Find a person":
                        {
                            // Entering search term
                            Console.Write("Please enter a search term: ");
                            var searchTerm = Console.ReadLine();

                            // Searching for all svc.People that contain 'searchTerm'
                            IEnumerable<Person> foundPeople = svc.People
                                .Where(p => p.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                                            p.Role.ToString().Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase));

                            List<Person> result = foundPeople.ToList(); // Converting IEnumerable to List

                            // Case for 0 results
                            if (result.Count() == 0)
                            { Console.WriteLine("No results found."); break; }

                            // Ultimate output
                            Console.WriteLine($"--> {result.Count()} results found!");
                            PrintListSelectionInterface(result);
                        }
                        break;
                    case "List all people":
                        // Simple function call
                        PrintListSelectionInterface(svc.People);
                        break;
                    case "Add a grade":
                        break;
                    case "Remove a grade":
                        break;








                    // Course options
                    case "Add a course":
                        {
                            // Course's fields
                            Console.Write("Course name: ");
                            var name = Console.ReadLine();
                            Console.Write("Class code: ");
                            var code = Console.ReadLine();
                            Console.Write("Description: ");
                            var desc = Console.ReadLine();

                            // Updating the course
                            var newCourse = svc.AddCourse(name, code, desc);
                            Console.WriteLine($"Course '{newCourse}' has been created.");
                        }
                        break;
                    case "Update course details":
                        {
                            // Selecting the course
                            Console.Write("Which course to update?: ");
                            Course course = ChooseFromMenuList(svc.Courses);
                            Console.WriteLine($"You have chosen to update {course}.");

                            // Course's fields
                            Console.Write("Course name: ");
                            var name = Console.ReadLine();
                            Console.Write("Class code: ");
                            var code = Console.ReadLine();
                            Console.Write("Description: ");
                            var desc = Console.ReadLine();

                            // Updating the course
                            svc.UpdateCourse(course, name, code, desc);
                            Console.WriteLine($"Course '{course}' has been updated.");
                        }
                        break;
                    case "Delete a course":
                        {
                            // Selecting the course
                            Console.WriteLine("Which course to delete?: ");
                            Course course = ChooseFromMenuList(svc.Courses);
                            if (course == null) { break; }

                            Console.WriteLine($"You have chosen to delete {course}.");

                            svc.Courses.Remove(course);
                        }
                        break;
                    case "Find a course":
                        {
                            // Entering search term
                            Console.Write("Please enter a course search term: ");
                            var searchTerm = Console.ReadLine();

                            // Searching for all svc.Courses that contain 'searchTerm'
                            IEnumerable<Course> foundCourses = svc.Courses
                                .Where(c => c.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                                            c.Description.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                                            c.Code.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase));

                            List<Course> result = foundCourses.ToList(); // Converting IEnumerable to List

                            // Case for 0 results
                            if (result.Count() == 0)
                            { Console.WriteLine("No results found."); break; }

                            // Ultimate output
                            Console.WriteLine($"--> {result.Count()} results found!");
                            PrintCoursesDetailed(result);
                        }
                        break;
                    case "List all courses":
                        // Simple function call
                        PrintCoursesDetailed(svc.Courses);
                        break;
                    case "Add an assignment":
                        {
                            Console.Write("Assignment name: ");
                            string name = Console.ReadLine() ?? string.Empty;
                            Console.Write("Description: ");
                            string desc = Console.ReadLine() ?? string.Empty;

                            string ptsStr;
                            uint pts;
                            do
                            {
                                Console.Write("Total points: ");
                                ptsStr = Console.ReadLine() ?? "0";
                            } while (!uint.TryParse(ptsStr, out pts));

                            string dueStr;
                            DateTime dueDate;
                            do
                            {
                                Console.Write("Due date [dd/mm/yyyy hh:mm {am or pm}]:\n\t");
                                dueStr = Console.ReadLine() ?? DateTime.Today.ToString();
                            } while (!DateTime.TryParse(dueStr, out dueDate));

                            Console.Write("Finally, the group: ");
                            string group = Console.ReadLine() ?? "Default";

                            Assignment assignment = new Assignment(name, desc, pts, dueDate, group);

                            Console.WriteLine($"Finally, which course would you like to add '{name}' to?:");
                            Course course = ChooseFromMenuList(svc.Courses);

                            course.AddAssignment(assignment);
                            Console.WriteLine($"Assignment {assignment} has been added to {course}");

                        }
                        break;
                    case "Remove an assignment":
                        {
                            Console.WriteLine("What course to remove an assignment from?:");
                            Course course = ChooseFromMenuList(svc.Courses);

                            // Selecting the assignment
                            Console.WriteLine("Which assignment to delete?: ");
                            Assignment assignment = ChooseFromMenuList(course.Assignments);
                            if (assignment == null) { break; }

                            Console.WriteLine($"You have chosen to delete {assignment}.");
                            svc.RemoveAssignment(assignment);
                        }
                        break;
                    case "Add a module":
                        {
                            Console.WriteLine($"What course to add a module to?:");
                            Course course = ChooseFromMenuList(svc.Courses);

                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Description: ");
                            var desc = Console.ReadLine();
                            var newModule = new Module(name, desc);

                            course.AddModule(newModule);
                            Console.WriteLine($"Module {newModule} has been added to {course}");
                        }
                        break;
                    case "Remove a module":
                        break;
                    case "Add a content item":
                        {
                            Console.WriteLine($"What course to add a content item to?:");
                            Course course = ChooseFromMenuList(svc.Courses);

                            Console.WriteLine($"...and what module?:");
                            Module module = ChooseFromMenuList(course.Modules);

                            Console.WriteLine("Now, which type of content item to add?:");
                            Console.WriteLine("1. Text (default)");
                            Console.WriteLine("2. Page");
                            Console.WriteLine("3. Assignment");
                            Console.WriteLine("4. File");

                            var select = GetInputInt();

                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Description: ");
                            var desc = Console.ReadLine();

                            ContentItem content;
                            switch(select)
                            {
                                case 2:
                                    {

                                    }
                                    break;
                                case 3:
                                    break;
                                case 4:
                                    break;
                                default:
                                    {
                                        content = new ContentItem(name, desc);
                                    }
                                    break;
                            }
                        }
                        break;
                    case "Remove a content item":
                        break;







                    // Shared options
                    case "Enroll in a course":
                        {
                            // Picking the person
                            Console.WriteLine("Which student do you want to add a course to?:");
                            Person person = ChooseFromMenuList(svc.People);

                            // Finding svc.Courses person isn't in
                            List<Course> filteredCourses = svc.Courses
                                .Where(c => !c.Roster.Contains(person)).ToList();

                            if (filteredCourses.Count() > 0)
                            {
                                Console.WriteLine("Which course would you like to add?:");
                                Course course = ChooseFromMenuList(filteredCourses);

                                // Adding the person
                                course.AddPerson(person);
                                Console.WriteLine($"You have chosen to enroll {person} to {course}.");
                            }
                            else { Console.WriteLine("No availiable svc.Courses."); }
                        }
                        break;
                    case "Unenroll a course":
                        {
                            // Selecting the course
                            Console.WriteLine("Select the course to remove a person from:");
                            Course course = ChooseFromMenuList(svc.Courses);
                            Console.WriteLine($"You have chosen {course}");

                            // Selecting the person
                            Console.WriteLine($"Now, which student would you like to remove?:");
                            Person person = ChooseFromMenuList(course.Roster);

                            // Removing the person from the course
                            Console.WriteLine($"You have chosen to remove {person} from {course}");
                            course.RemovePerson(person);
                        }
                        break;
                }


                if (selected.Children.Count() == 0)
                {
                    Console.ReadLine();
                    selected = selected.Parent;
                }
            }
        }

        // ===================
        //      FUNCTIONS
        // ===================

        // <-- GetInputInt -->
        private static int GetInputInt()
        {
            Console.Write(">> ");
            if (int.TryParse(Console.ReadLine(), out int choiceInt))
            {
                return choiceInt;
            }
            return -1;
        }


        private static void PrintListSelectionInterface<T>(List<T> myList)
        {
            Console.Clear();
            // Case for 0 items in the list
            if (myList.Count() == 0)
            {
                Console.WriteLine($"No items of type '{typeof(T)}' found");
                return;
            }
            // Print formatted list
            for (int i = 0; i < myList.Count(); i++)
            {
                Console.WriteLine($"{i + 1}.\t{myList[i]}");
            }
        }

        private static void PrintCoursesDetailed(List<Course> courseList)
        {
            for (int i = 0; i < courseList.Count(); i++)
            {
                // Name and description
                Console.WriteLine($"{i + 1}.\tCourse name: {courseList[i]}\n\tDescription: {courseList[i].Description}\n\tPeople enrolled:");

                // Student Roster
                for (int j = 0; j < courseList[i].Roster.Count(); j++)
                {
                    Console.WriteLine($"\t\t{j + 1}. {courseList[i].Roster[j]}");
                }
                if (courseList[i].Roster.Count() == 0) { Console.WriteLine("\t\tNone."); }

                // Assignment list
                Console.WriteLine("\tAssignments:");
                for (int j = 0; j < courseList[i].GetAssignments().Count(); j++)
                {
                    Console.WriteLine($"\t\t{j + 1}. {courseList[i].GetAssignments()[j]}");
                }
                if (courseList[i].GetAssignments().Count() == 0) { Console.WriteLine("\t\tNone."); }
                Console.Write("\n");
            }
        }

        private static T ChooseFromMenuList<T>(List<T> list)
        {
            if (list.Count() == 0) { return default(T); }
            // Print menu
            PrintListSelectionInterface(list);

            int choice;
            do
            {
                choice = GetInputInt();
                --choice; // Subtract 1 since menu starts at 1
            } while (choice > list.Count() && choice < 0);

            return list[choice];
        }
    }
}

