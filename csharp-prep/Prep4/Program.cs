using System;
using System.Collections.Generic;
using System.Linq;

namespace Prep4
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int>();

            Console.WriteLine("Enter a list of numbers, type 0 when finished.");

            int input;
            do
            {
                Console.Write("Enter number: ");
                input = int.Parse(Console.ReadLine());
                if (input != 0)
                {
                    numbers.Add(input);
                }
            } while (input != 0);

            // Core Requirements
            if (numbers.Count > 0)
            {
                // Compute sum
                int sum = numbers.Sum();

                // Compute average
                double average = numbers.Average();

                // Find maximum
                int max = numbers.Max();

                Console.WriteLine($"The sum is: {sum}");
                Console.WriteLine($"The average is: {average}");
                Console.WriteLine($"The largest number is: {max}");
            }
            else
            {
                Console.WriteLine("No numbers entered.");
            }

            // Stretch Challenge
            if (numbers.Count > 0)
            {
                // Find smallest positive number
                int smallestPositive = numbers.Where(n => n > 0).Min();

                // Sort the list
                numbers.Sort();

                Console.WriteLine($"The smallest positive number is: {smallestPositive}");
                Console.WriteLine("The sorted list is:");
                foreach (int num in numbers)
                {
                    Console.WriteLine(num);
                }
            }
            else
            {
                Console.WriteLine("No numbers entered.");
            }
        }
    }
}
