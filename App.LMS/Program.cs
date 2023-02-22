using System;
using Library.LMS.Models;

namespace App.LMS
{
    internal class Program
    {

        static void Main(string[] args)
        {

            LMSService svc = new LMSService();

            /*  
             *  NOTE: If option depth >0, always print back option.
             *  
             *  1. People options
             *    -> Add person
             *    -> Update a person
             *        -> Update details
             *        -> Manage courses
             *            -> Add a course
             *            -> Remove a course
             *        -> Manage assignments
             *            -> Grade an assignment
             *            -> Remove a grade
             *    -> Delete a person
             *    -> Find a person's details
             *    -> List all people
             *
             *  2. Course options
             *    -> Add a course
             *    -> Update a course
             *        -> Update details
             *        -> Manage members
             *            -> Add a member
             *            -> Remove a member
             *        -> Manage assignments
             *            -> Add
             *            -> Remove
             *            -> 
             *    -> Delete a course
             *    -> Find a course's details
             *    -> List all courses
             */


            Console.Clear();
            Console.WriteLine("1. People Options");
            Console.WriteLine("2. Course Options");
            int select = GetInputInt();

            if (select == 1)
            {
                Console.Clear();
                Console.WriteLine("1. Add a person");
                Console.WriteLine("2. Update a person");
                Console.WriteLine("3. Delete a person");
                Console.WriteLine("4. Find a person");
                Console.WriteLine("5. List all people");
                Console.WriteLine("6. Back");
                select = GetInputInt();

                switch (select)
                {
                    case 1:
                        {
                            string name;
                            string 
                        }
                        break;
                    case 2:
                        {

                        }
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    default:
                        break;
                }
            }
            else if (select == 2)
            {

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

                choice = GetInputInt();
                --choice; // Subtract 1 since menu starts at 1
            } while (choice > list.Count() || choice < 0);

            return list[choice];
        }
    }
}