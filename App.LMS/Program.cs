using System;
using Library.LMS.Models;

namespace App.LMS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Course> courses = new List<Course>();
            List<Person> people = new List<Person>();


            /*
            // Demo courses (debug only)
            courses.Add(new Course("Biology I", "BSC2010", "Freshmen biology class."));
            courses.Add(new Course("Statistics", "STA1101", "Introductory statistics class."));
            courses.Add(new Course("Discrete Math", "MAD3250", "Formal logic and induction."));
            courses.Add(new Course("Physical Nutrition", "PHY2220", "Nutritional analysis and discourse."));
            courses.Add(new Course("Biology I Lab", "BSC2010L", "Lab component of BSC2010."));
            courses.Add(new Course("Popular Music", "IDS3100", "History of American popular music."));
            */

            // Listing commands
            Console.WriteLine("Type the number of the command you'd like to perform: ");
            Console.WriteLine("1. Create a course");
            Console.WriteLine("2. Create a person");
            Console.WriteLine("3. Enroll a person in a course");
            Console.WriteLine("4. Remove a person from a course");
            Console.WriteLine("5. List all courses");
            Console.WriteLine("6. Search for a course");
            Console.WriteLine("7. List all people");
            Console.WriteLine("8. Search for a person");
            Console.WriteLine("9. List a person's courses");
            Console.WriteLine("10. Update a course");
            Console.WriteLine("11. Update a person");
            Console.WriteLine("12. Create an assignment and add to a course");
            Console.WriteLine("13. Exit the program");


            // ===================
            //      MAIN LOOP
            // ===================
            bool cont = true;
            while (cont)
            {
                Console.Write(">> ");
                var choice = Console.ReadLine() ?? "13";
                if (int.TryParse(choice, out int choiceInt))
                {
                    switch (choiceInt)
                    {
                        // OPTION 1: Add course
                        case 1:
                            {
                                // Choosing all fields for the course
                                Console.Write("Course name: ");
                                var name = Console.ReadLine();
                                Console.Write("Class code: ");
                                var code = Console.ReadLine();
                                Console.Write("Description: ");
                                var desc = Console.ReadLine();

                                // Adding the course to the master list
                                AddCourse(courses, name, code, desc);
                                Console.WriteLine($"Course '{courses.Last()}' has been added.");
                            }
                            break;


                        // OPTION 2: Add person
                        case 2:
                            {
                                // Choosing fields for the person
                                Console.Write("Name: ");
                                var name = Console.ReadLine();
                                Console.Write("Classification: ");
                                var classification = Console.ReadLine();

                                // Adding the person to the master list
                                AddPerson(people, name, classification);
                                Console.WriteLine($"{people.Last()}, has been created.");
                            }
                            break;


                        // OPTION 3: Add person to a course
                        case 3:
                            {
                                // Picking the person
                                Console.WriteLine("Which student do you want to add a course to?:");
                                Person person = ChooseFromMenuList(people);

                                // Finding courses person isn't in
                                List<Course> filteredCourses = courses
                                    .Where(c => !c.GetRoster().Contains(person)).ToList();

                                if (filteredCourses.Count() > 0)
                                {
                                    Console.WriteLine("Which course would you like to add?:");
                                    Course course = ChooseFromMenuList(filteredCourses);

                                    // Adding the person
                                    course.AddPerson(person);
                                    Console.WriteLine($"You have chosen to enroll {person} to {course}.");
                                }
                                else { Console.WriteLine("No availiable courses."); }
                            }
                            break;


                        // OPTION 4: Remove a person from a course
                        case 4:
                            {
                                // Selecting the course
                                Console.WriteLine("Select the course to remove a person from:");
                                Course course = ChooseFromMenuList(courses);
                                Console.WriteLine($"You have chosen {course}");

                                // Selecting the person
                                Console.WriteLine($"Now, which student would you like to remove?:");
                                Person person = ChooseFromMenuList(course.GetRoster());

                                // Removing the person from the course
                                Console.WriteLine($"You have chosen to remove {person} from {course}");
                                course.RemovePerson(person);
                            }

                            break;


                        // OPTION 5: List all courses
                        case 5:
                            // Simple function call
                            PrintCoursesDetailed(courses);
                            break;


                        // OPTION 6: Search for a course
                        case 6:
                            {
                                // Entering search term
                                Console.Write("Please enter a course search term: ");
                                var searchTerm = Console.ReadLine();

                                // Searching for all courses that contain 'searchTerm'
                                IEnumerable<Course> foundCourses = courses
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


                        // OPTION 7: List all people
                        case 7:
                            // Simple function call
                            PrintListSelectionInterface(people);
                            break;


                        // OPTION 8: Search for a person
                        case 8:
                            {
                                // Entering search term
                                Console.Write("Please enter a course search term: ");
                                var searchTerm = Console.ReadLine();

                                // Searching for all people that contain 'searchTerm'
                                IEnumerable<Person> foundPeople = people
                                    .Where(p => p.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                                                p.Classification.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase));

                                List<Person> result = foundPeople.ToList(); // Converting IEnumerable to List

                                // Case for 0 results
                                if (result.Count() == 0)
                                { Console.WriteLine("No results found."); break; }

                                // Ultimate output
                                Console.WriteLine($"--> {result.Count()} results found!");
                                PrintListSelectionInterface(result);
                            }
                            break;


                        // OPTION 9: List a person's courses
                        case 9:
                            {
                                Console.WriteLine("Which student's courses would you like to view?:");
                                Person person = ChooseFromMenuList(people);

                                foreach (Course c in courses)
                                {
                                    if (c.GetRoster().Contains(person))
                                    {
                                        Console.WriteLine($"{courses.IndexOf(c)+1}.\t{c}");
                                    }

                                }
                            }
                            break;


                        // OPTION 10: Update a course
                        case 10:
                            {
                                // Selecting the course
                                Console.Write("Which course to update?: ");
                                Course course = ChooseFromMenuList(courses);
                                Console.WriteLine($"You have chosen to update {course}.");

                                // Course's fields
                                Console.Write("Course name: ");
                                var name = Console.ReadLine();
                                Console.Write("Class code: ");
                                var code = Console.ReadLine();
                                Console.Write("Description: ");
                                var desc = Console.ReadLine();

                                // Updating the course
                                UpdateCourse(course, name, code, desc);
                                Console.WriteLine($"Course '{course}' has been update.");
                            }
                            break;


                        // OPTION 11: Update a person
                        case 11:
                            {
                                // Selecting the person
                                Console.WriteLine("Which person to update?: ");
                                Person person = ChooseFromMenuList(people);
                                Console.WriteLine($"You have chosen to update {person}.");

                                // Person's fields
                                Console.Write("Name: ");
                                var name = Console.ReadLine();
                                Console.Write("Classification: ");
                                var _class = Console.ReadLine();

                                // Updating the person
                                UpdatePerson(person, name, _class);
                                Console.WriteLine($"{person} has been updated.");
                            }
                            break;


                        // OPTION 12: Create an assignment and add to a course
                        case 12:
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

                                Assignment assignment = new Assignment(name, desc, pts, dueDate);

                                Console.WriteLine($"Finally, which course would you like to add '{name}' to?:");
                                Course course = ChooseFromMenuList(courses);

                                course.AddAssignment(assignment);
                                Console.WriteLine($"Assignment {assignment} has been added to {course}");

                            }
                            break;


                        // OPTION 13: Exit
                        case 13:
                            cont = false;
                            break;


                        default:
                            break;
                    }
                }
            }
        }

        // ===================
        //      FUNCTIONS
        // ===================

        // <-- AddCourse -->
        // Adds a course with specified name, code, and description to a list.
        private static Course AddCourse(List<Course> cl, string name, string code, string desc)
        {
            cl.Add(new Course(name, code, desc));
            return cl.Last();
        }

        // <-- UpdateCourse -->
        // Update a course with new desired values.
        private static Course UpdateCourse(Course c, string name, string code, string desc)
        {
            c.Name = name;
            c.Code = code;
            c.Description = desc;
            return c;
        }

        // <-- AddPerson -->
        // Adds a person with specified name and classification to a list.
        private static Person AddPerson(List<Person> pl, string name, string classification)
        {
            pl.Add(new Person(name, classification));
            return pl.Last();
        }

        // <-- UpdatePerson -->
        // Update a person with new desired values.
        private static Person UpdatePerson(Person p, string name, string _class)
        {
            p.Name = name;
            p.Classification = _class;
            return p;
        }

        // <-- PrintListSelectionInterface -->
        // Take a list and print each item in a formatted and numbered manner.
        private static void PrintListSelectionInterface<T>(List<T> myList)
        {
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

        // <-- PrintCoursesDetailed -->
        // Special function that prints more detailed info on courses such as
        // its description and students.
        private static void PrintCoursesDetailed(List<Course> courseList)
        {
            for (int i = 0; i < courseList.Count(); i++)
            {
                // Name and description
                Console.WriteLine($"{i + 1}.\tCourse name: {courseList[i]}\n\tDescription: {courseList[i].Description}\n\tPeople enrolled:");

                // Student roster
                for (int j = 0; j < courseList[i].GetRoster().Count(); j++)
                {
                    Console.WriteLine($"\t\t{j + 1}. {courseList[i].GetRoster()[j]}");
                }
                if (courseList[i].GetRoster().Count() == 0) { Console.WriteLine("\t\tNone."); }

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

        // <-- ChooseFromMenuList -->
        // Prints a menu (provided by PrintListSelectionInterface) and allows
        // the user to pick a value from it.
        private static T ChooseFromMenuList<T>(List<T> list)
        {
            // Print menu
            PrintListSelectionInterface(list);

            int choice;
            do
            {
                Console.Write(">> ");
                var choiceStr = Console.ReadLine() ?? "-1"; // We add '?? -1' to
                                                            // to make sure null
                                                            // inputs don't break
                                                            // the loop

                // Try to convert the input into an int
                if (int.TryParse(choiceStr, out choice))
                {
                    --choice;   // We subtract 1 since we added 1 for ease of use earlier
                    break;
                }
            } while (choice > list.Count() || choice < 0);

            return list[choice];
        }
    }
}