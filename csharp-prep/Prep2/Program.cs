using System;

namespace Prep2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompting user for grade percentage
            Console.WriteLine("Enter your grade percentage:");
            int gradePercentage = int.Parse(Console.ReadLine());

            // Determine letter grade
            string letter = "";
            if (gradePercentage >= 90)
            {
                letter = "A";
            }
            else if (gradePercentage >= 80)
            {
                letter = "B";
            }
            else if (gradePercentage >= 70)
            {
                letter = "C";
            }
            else if (gradePercentage >= 60)
            {
                letter = "D";
            }
            else
            {
                letter = "F";
            }

            // Determine if the user passed the course
            bool passed = gradePercentage >= 70;

            // Print out the letter grade
            Console.Write($"Your letter grade is {letter}");

            // Stretch challenge: Determine sign
            int lastDigit = gradePercentage % 10;
            string sign = "";
            if (letter != "F" && lastDigit >= 7)
            {
                sign = "+";
            }
            else if (letter != "F" && lastDigit < 3)
            {
                sign = "-";
            }

            // Handling exceptional cases
            if (letter == "A" && sign == "+")
            {
                sign = ""; // A+ is not a valid grade
            }
            else if (letter == "F" && (sign == "+" || sign == "-"))
            {
                sign = ""; // F+ or F- is not a valid grade
            }

            // Displaying grade sign if applicable
            if (!string.IsNullOrEmpty(sign))
            {
                Console.Write(sign);
            }

            // Display congratulatory or encouraging message
            if (passed)
            {
                Console.WriteLine("\nCongratulations! You passed the course.");
            }
            else
            {
                Console.WriteLine("\nYour grade is Low! You can improve next time.");
            }
        }
    }
}
