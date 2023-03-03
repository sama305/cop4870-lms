using System;
using System.Linq;
using System.Xml.Linq;
using Library.LMS.Models;

namespace App.LMS
{
    internal class Program
    {

        static void Main(string[] args)
        {

            LMSService svc = new LMSService();

            // All menu options are initialized here
            DialogueTree dt = new DialogueTree();

            dt.CurrentNode.createMultiChild("Person options")
                          .createMultiChild("Course options");


            // <------- "Person options" ------->
            dt.CurrentNode = dt.RootNode.Children[0];
            dt.CurrentNode.createMultiChild("Add a person")
                          .createMultiChild("Manage a person")
                          .createMultiChild("Delete a person")
                          .createMultiChild("Find a person")
                          .createMultiChild("List all people");

            dt.CurrentNode = dt.CurrentNode.Children[1]; // "Update a person"
            dt.CurrentNode.createMultiChild("Update person details")
                          .createMultiChild("Manage courses")
                          .createMultiChild("Manage assignments");

            dt.CurrentNode = dt.CurrentNode.Children[1]; // "Manage courses"
            dt.CurrentNode.createMultiChild("Enroll")
                          .createMultiChild("Unenroll");

            dt.goUp();
            dt.CurrentNode = dt.CurrentNode.Children[2]; // "Manage assignments"
            dt.CurrentNode.createMultiChild("Add/update a grade")
                          .createMultiChild("Remove a grade");
            // <------- "Person options" ------->


            // <------- "Course options" ------->
            dt.CurrentNode = dt.RootNode.Children[1];
            dt.CurrentNode.createMultiChild("Add a course")
                          .createMultiChild("Manage a course")
                          .createMultiChild("Delete a course")
                          .createMultiChild("Find a course")
                          .createMultiChild("List all courses");

            dt.CurrentNode = dt.CurrentNode.Children[1]; // "Update a course and it's components"
            dt.CurrentNode.createMultiChild("Update course details")
                          .createMultiChild("Manage members")
                          .createMultiChild("Manage assignments")
                          .createMultiChild("Manage modules")
                          .createMultiChild("Add an announcement")
                          .createMultiChild("Remove a announcement");

            dt.CurrentNode = dt.CurrentNode.Children[1];
            dt.CurrentNode.createMultiChild("Enroll")
                          .createMultiChild("Unenroll");

            dt.goUp();
            dt.CurrentNode = dt.CurrentNode.Children[2];
            dt.CurrentNode.createMultiChild("Add an assignment")
                          .createMultiChild("Remove an assignment")
                          .createMultiChild("Add an assignment group")
                          .createMultiChild("Remove an assignment group");

            dt.goUp();
            dt.CurrentNode = dt.CurrentNode.Children[3];
            dt.CurrentNode.createMultiChild("Add a module")
                          .createMultiChild("Remove a module")
                          .createMultiChild("Add a content item")
                          .createMultiChild("Remove content item");


            dt.CurrentNode = dt.RootNode;
            // <------- "Course options" ------->

            Node selected = dt.RootNode;
            Course? selectedCourse = null;
            Person? selectedPerson = null;
            while (true)
            {
                Console.Clear();
                selected = dt.chooseChildFromMenu(selected, true);

                Console.Clear();
                switch (selected.Data)
                {
                    // People options
                    case "Person options":
                        selectedPerson = null;
                        break;
                    case "Manage a person":
                        if (selectedPerson == null)
                            selectedPerson = ChooseFromMenuList(svc.People, "Please choose a person to update:");
                        break;


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
                    case "Delete a person":
                        {
                            // Selecting the person
                            Person person = ChooseFromMenuList(svc.People, "Which person to delete?: ");
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
                            svc.PrintPeopleDetailed(result);
                        }
                        break;
                    case "List all people":
                        {
                            ListNavigator<Person> ln = new ListNavigator<Person>(svc.People);

                            bool quit = false;
                            while (!quit)
                            {
                                Console.Clear();
                                foreach (int i in ln.GetCurrentPage().Keys)
                                {
                                    Console.WriteLine($"{i}.\t{ln.GetCurrentPage()[i]}");
                                }

                                Console.Write("\n(1: left, 3: right, 2: exit) ");
                                var go = GetInputInt();

                                switch (go)
                                {
                                    case 1:
                                        try
                                        {
                                            ln.GoBackward();
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        break;
                                    case 2:
                                        quit = true;
                                        break;
                                    case 3:
                                        try
                                        {
                                            ln.GoForward();
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case "Update person details":
                        {
                            // Choosing fields for the person
                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Class Role (Student, TA, or Instructor): ");
                            var roleStr = Console.ReadLine();
                            if (Enum.TryParse<ClassRoles>(roleStr, true, out ClassRoles roleCnvt))
                            {
                                // Updating the person
                                svc.UpdatePerson(selectedPerson, name, roleCnvt);
                                Console.WriteLine($"{selectedPerson} has been updated.");
                            }
                            else
                            {
                                Console.WriteLine($"Role {roleStr} not found");
                            }
                        }
                        break;
                    case "Add/update a grade":
                        {
                            if (selectedPerson.GetType() != typeof(Student))
                            {
                                Console.WriteLine("Selected person is not a student");
                                break;
                            }

                            List<Course> enrolled = svc.GetEnrolledCourses(selectedPerson);

                            if (enrolled.Count() == 0)
                            {
                                Console.WriteLine("No courses found.");
                                break;
                            }
                            var course = ChooseFromMenuList(enrolled, "Which course to add grade from?:");

                            if (course.Assignments.Count() == 0)
                            {
                                Console.WriteLine("No assignment found.");
                                break;
                            }
                            var assignment = ChooseFromMenuList(course.Assignments, "Finally, which assignment to add a grade for?:");

                            Console.WriteLine($"What was the student's grade (out of {assignment.TotalPoints})");
                            var score = GetInputInt();

                            // Make the score either the max or 0 (if the input is out of range)
                            score = ((score > assignment.TotalPoints) ? assignment.TotalPoints : ((score < 0) ? 0 : score));

                            (selectedPerson as Student).AddAssignmentGrade(assignment, score);

                            Console.WriteLine($"Assignment '{assignment.Name}' ({(double)score / assignment.TotalPoints * 100}%) has been graded.");
                        }
                        break;
                    case "Remove a grade":
                        break;








                    // Course options
                    case "Course options":
                        selectedCourse = null;
                        break;
                    case "Manage a course":
                        if (selectedCourse == null)
                            selectedCourse = ChooseFromMenuList(svc.Courses, "Please select a course to update:");
                        break;


                    case "Add a course":
                        {
                            // Course's fields
                            Console.Write("Course name: ");
                            var name = Console.ReadLine();
                            Console.Write("Class code: ");
                            var code = Console.ReadLine();
                            Console.Write("Description: ");
                            var desc = Console.ReadLine();
                            Console.Write("Credit hours: ");
                            int hours = GetInputInt();

                            // Adding the course
                            if (svc.Courses.Find(c => c.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase)) == null)
                            {
                                var newCourse = svc.AddCourse(name, code, desc, hours);
                                Console.WriteLine($"Course '{newCourse}' has been created.");
                            } else
                            {
                                Console.WriteLine("A course with that code already exists.");
                            }
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
                    case "Update course details":
                        {
                            // Course's fields
                            Console.Write("Course name: ");
                            var name = Console.ReadLine();
                            Console.Write("Class code: ");
                            var code = Console.ReadLine();
                            Console.Write("Description: ");
                            var desc = Console.ReadLine();

                            // Updating the course
                            if (svc.Courses.Find(c => c.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase)) == null)
                            {
                                svc.UpdateCourse(selectedCourse, name, code, desc);
                                Console.WriteLine($"Course '{selectedCourse}' has been updated.");
                            }
                            else
                            {
                                Console.WriteLine("A course with that code already exists.");
                            }
                        }
                        break;
                    case "Add an assignment":
                        {
                            if (selectedCourse.AssignmentGroups.Count() == 0)
                            {
                                Console.WriteLine("No assignment groups exist. Please add to create an assignment.");
                                break;
                            }

                            Console.Write("Assignment name: ");
                            string name = Console.ReadLine() ?? string.Empty;
                            Console.Write("Description: ");
                            string desc = Console.ReadLine() ?? string.Empty;

                            string ptsStr;
                            int pts;
                            do
                            {
                                Console.Write("Total points: ");
                                ptsStr = Console.ReadLine() ?? "0";
                            } while (!int.TryParse(ptsStr, out pts));

                            string dueStr;
                            DateTime dueDate;
                            do
                            {
                                Console.Write("Due date [mm/dd/yyyy hh:mm {am or pm}]:\n\t");
                                dueStr = Console.ReadLine() ?? DateTime.Today.ToString();
                            } while (!DateTime.TryParse(dueStr, out dueDate));

                            AssignmentGroup ag = ChooseFromMenuList(selectedCourse.AssignmentGroups, "Finally the group:");

                            Assignment assignment = new Assignment(selectedCourse, name, desc, pts, dueDate, ag);

                            selectedCourse.AddAssignment(assignment);
                            Console.WriteLine($"Assignment {assignment} has been added to {selectedCourse}");

                        }
                        break;
                    case "Remove an assignment":
                        {
                            // Selecting the assignment
                            Assignment assignment = ChooseFromMenuList(selectedCourse.Assignments, "Which assignment to delete?: ");
                            if (assignment == null) { break; }

                            Console.WriteLine($"You have chosen to delete {assignment}.");
                            selectedCourse.RemoveAssignment(assignment);
                        }
                        break;
                    case "Add an assignment group":
                        {
                            Console.Write("Assignment group name: ");
                            string name = Console.ReadLine();
                            Console.Write("Assignment group weight: ");
                            int weight = GetInputInt();

                            AssignmentGroup ag = selectedCourse.AddAssignmentGroup(name, weight);
                            Console.WriteLine($"Assignment group {ag} has been added.");
                            
                        }
                        break;
                    case "Add a module":
                        {
                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Description: ");
                            var desc = Console.ReadLine();
                            var newModule = new Module(name, desc);

                            selectedCourse.AddModule(newModule);
                            Console.WriteLine($"Module {newModule} has been added to {selectedCourse}");
                        }
                        break;
                    case "Remove a module":
                        {
                            Course course = ChooseFromMenuList(svc.Courses, "What course to remove a module from?:");

                            // Selecting the module
                            Module module = ChooseFromMenuList(course.Modules, "Which module to delete?: ");
                            if (module == null) { break; }

                            Console.WriteLine($"You have chosen to delete {module}.");
                            course.RemoveModule(module);
                        }
                        break;
                    case "Add a content item":
                        {
                            Module module = ChooseFromMenuList(selectedCourse.Modules, "Which module to add content to?:");

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
                                        Console.Write("HTML: ");
                                        var html = Console.ReadLine();

                                        content = new PageItem(html, name, desc);
                                    }
                                    break;
                                case 3:
                                    {
                                        Console.Write("Link an assignment: ");
                                        Assignment assignment = ChooseFromMenuList(selectedCourse.Assignments);

                                        content = new AssignmentItem(assignment, name, desc);
                                    }
                                    break;
                                case 4:
                                    {
                                        Console.Write("File path: ");
                                        var path = Console.ReadLine();

                                        content = new FileItem(path, name, desc);
                                    }
                                    break;
                                default:
                                    {
                                        content = new ContentItem(name, desc);
                                    }
                                    break;
                            }

                            Console.WriteLine($"Content {content} was added to {module}.");
                        }
                        break;
                    case "Remove a content item":
                        {
                            List<ContentItem> allContents = new List<ContentItem>();
                            foreach (Module m in selectedCourse.Modules)
                            {
                                allContents.AddRange(m.Contents);
                            }
                            if (allContents.Count() == 0) { break; }
                            var content = ChooseFromMenuList(allContents, "Which content would you like to remove");
                        }
                        break;
                    case "Add an announcement":
                        {
                            Console.Write("Announcement header: ");
                            var header = Console.ReadLine();
                            Console.Write("Content: ");
                            var content = Console.ReadLine();

                            selectedCourse.AddAnnouncement(header, content);
                            Console.WriteLine($"Announcement {selectedCourse.Announcements.Last()} has been added.");
                        }
                        break;
                    case "Remove a annoucement":
                        {
                            Announcement a = ChooseFromMenuList(selectedCourse.Announcements);

                            selectedCourse.RemoveAnnouncement(a);
                            Console.WriteLine($"Annoucement {a} has been removed.");
                        }
                        break;



                    // SHARED options
                    case "Enroll":
                        {
                            if (selectedCourse != null)
                            {
                                // Finding people not in the course
                                List<Person> filteredPeople = svc.People
                                    .Where(p => !selectedCourse.Roster.Contains(p)).ToList();

                                if (filteredPeople.Count() > 0)
                                {
                                    Console.WriteLine("Which person would you like to add?:");
                                    Person person = ChooseFromMenuList(filteredPeople);

                                    // Adding the person
                                    selectedCourse.AddPerson(person);
                                    Console.WriteLine($"You have chosen to enroll {person} to {selectedCourse}.");
                                }
                                else { Console.WriteLine("No availiable people."); }
                            }
                            else if (selectedPerson != null)
                            {
                                // Finding course the person isn't enrolled in
                                List<Course> unenrolled = svc.GetEnrolledCourses(selectedPerson, true); 

                                if (unenrolled.Count() > 0)
                                {
                                    Console.WriteLine("Which person would you like to add?:");
                                    Course course = ChooseFromMenuList(unenrolled);

                                    // Adding the person
                                    course.AddPerson(selectedPerson);
                                    Console.WriteLine($"You have chosen to enroll {selectedPerson} to {course}.");
                                }
                                else { Console.WriteLine("No availiable courses."); }
                            }
                        }
                        break;
                    case "Unenroll":
                        {
                            if (selectedCourse != null)
                            {
                                // Selecting the person
                                Console.WriteLine($"Which student would you like to remove?:");
                                Person person = ChooseFromMenuList(selectedCourse.Roster);

                                // Removing the person from the course
                                Console.WriteLine($"You have chosen to remove {person} from {selectedCourse}");
                                selectedCourse.RemovePerson(person);
                            }
                            else if (selectedPerson != null)
                            {
                                // Selecting the course
                                Console.WriteLine($"Which course would you like to remove?:");
                                Course course = ChooseFromMenuList(svc.GetEnrolledCourses(selectedPerson));

                                // Removing the person from the course
                                Console.WriteLine($"You have chosen to remove {selectedPerson} from {course}");
                                course.RemovePerson(selectedPerson);
                            }
                        }
                        break;

                    case "TESTTEST":
                        {
                            Student student = ChooseFromMenuList(svc.Students);
                            var course = ChooseFromMenuList(svc.GetEnrolledCourses(student));


                            Console.WriteLine(svc.GetStudentGPA(student));
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


        private static void PrintListSelectionInterface<T>(List<T> myList, string? header="")
        {
            Console.Clear();
            if (header.Length != 0)
                Console.WriteLine(header);
            
            // Case for 0 items in the list
            if (myList.Count() == 0)
                throw new Exception($"No items of type '{typeof(T)}' found");

            // Print formatted list
            for (int i = 0; i < myList.Count(); i++)
            {
                Console.WriteLine($"{i + 1}.\t{myList[i].ToString()}");
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
                for (int j = 0; j < courseList[i].Assignments.Count(); j++)
                {
                    Console.WriteLine($"\t\t{j + 1}. {courseList[i].Assignments[j]}");
                }
                if (courseList[i].Assignments.Count() == 0) { Console.WriteLine("\t\tNone."); }
                Console.Write("\n");

                // Modules list
                Console.WriteLine("\tModules:");
                for (int j = 0; j < courseList[i].Modules.Count(); j++)
                {
                    Console.WriteLine($"\t\t{j + 1}. {courseList[i].Modules[j]}");

                    for (int k = 0; k < courseList[i].Modules[j].Contents.Count(); k++)
                    {
                        Console.WriteLine($"\t\t{k + 1}. {courseList[i].Modules[j].Contents[k]}");
                    }
                }
                if (courseList[i].Modules.Count() == 0) { Console.WriteLine("\t\tNone."); }
            }
        }

        private static T ChooseFromMenuList<T>(List<T> list, string header="")
        {
            if (list.Count() == 0) { return default(T); }

            try
            {
                PrintListSelectionInterface(list, header);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }

            int choice;
            do
            {
                choice = GetInputInt();
            } while (choice > list.Count() || choice < 0);

            --choice; // Subtract 1 since menu starts at 1

            return list[choice];
        }
    }
}

