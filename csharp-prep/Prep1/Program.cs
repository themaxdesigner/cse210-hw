using System;

namespace Prep1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompting user for first name
            Console.WriteLine("What is your first name?");
            string firstName = Console.ReadLine();

            // Prompting user for last name
            Console.WriteLine("What is your last name?");
            string lastName = Console.ReadLine();

            // Displaying the name in the specified format
            Console.WriteLine($"Your name is {lastName}, {firstName} {lastName}.");
        }
    }
}
