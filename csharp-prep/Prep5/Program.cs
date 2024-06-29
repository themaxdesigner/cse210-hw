using System;

namespace Prep5
{
    class Program
    {
        static void Main(string[] args)
        {
            // Call DisplayWelcome function
            DisplayWelcome();

            // Call PromptUserName function
            string userName = PromptUserName();

            // Call PromptUserNumber function
            int userNumber = PromptUserNumber();

            // Call SquareNumber function
            int squaredNumber = SquareNumber(userNumber);

            // Call DisplayResult function
            DisplayResult(userName, squaredNumber);
        }

        // Function to display welcome message
        static void DisplayWelcome()
        {
            Console.WriteLine("Welcome to the program!");
        }

        // Function to prompt user for name and return it
        static string PromptUserName()
        {
            Console.Write("Please enter your name: ");
            return Console.ReadLine();
        }

        // Function to prompt user for favorite number and return it
        static int PromptUserNumber()
        {
            Console.Write("Please enter your favorite number: ");
            return int.Parse(Console.ReadLine());
        }

        // Function to square a number and return the result
        static int SquareNumber(int number)
        {
            return number * number;
        }

        // Function to display the result
        static void DisplayResult(string name, int squaredNumber)
        {
            Console.WriteLine($"{name}, the square of your number is {squaredNumber}");
        }
    }
}
