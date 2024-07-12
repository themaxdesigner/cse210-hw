using System;
using System.Collections.Generic;
using System.Threading;

public abstract class MindfulnessActivity
{
    protected int durationInSeconds;

    public MindfulnessActivity(int durationInSeconds)
    {
        this.durationInSeconds = durationInSeconds;
    }

    public abstract void Start();

    protected void DisplayMessage(string message, int seconds)
    {
        Console.WriteLine(message);
        ShowSpinner(seconds);
    }

    protected void ShowSpinner(int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }

    protected void Countdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write($"{i} ");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }
}

public class BreathingActivity : MindfulnessActivity
{
    public BreathingActivity(int durationInSeconds) : base(durationInSeconds)
    {
    }

    public override void Start()
    {
        DisplayMessage("Welcome to Breathing Activity.", 2);
        DisplayMessage("This activity will help you relax by guiding you through breathing in and out slowly. Clear your mind and focus on your breathing.", 3);

        int timeElapsed = 0;
        while (timeElapsed < durationInSeconds)
        {
            DisplayMessage("Breathe in...", 3);
            Countdown(3);
            timeElapsed += 3;

            if (timeElapsed >= durationInSeconds)
                break;

            DisplayMessage("Breathe out...", 3);
            Countdown(3);
            timeElapsed += 3;
        }

        DisplayMessage($"Good job! You have completed Breathing Activity. Total time: {durationInSeconds} seconds.", 3);
    }
}

public class ReflectionActivity : MindfulnessActivity
{
    private List<string> prompts = new List<string>()
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private List<string> questions = new List<string>()
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    public ReflectionActivity(int durationInSeconds) : base(durationInSeconds)
    {
    }

    public override void Start()
    {
        DisplayMessage("Welcome to Reflection Activity.", 2);
        DisplayMessage("This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.", 3);

        Random random = new Random();
        int timeElapsed = 0;
        while (timeElapsed < durationInSeconds)
        {
            string prompt = prompts[random.Next(prompts.Count)];
            DisplayMessage(prompt, 3);

            foreach (var question in questions)
            {
                DisplayMessage(question, 3);
                timeElapsed += 3;
                if (timeElapsed >= durationInSeconds)
                    break;
            }
        }

        DisplayMessage($"Good job! You have completed Reflection Activity. Total time: {durationInSeconds} seconds.", 3);
    }
}

public class ListingActivity : MindfulnessActivity
{
    private List<string> prompts = new List<string>()
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity(int durationInSeconds) : base(durationInSeconds)
    {
    }

    public override void Start()
    {
        DisplayMessage("Welcome to Listing Activity.", 2);
        DisplayMessage("This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.", 3);

        Random random = new Random();
        string prompt = prompts[random.Next(prompts.Count)];
        DisplayMessage(prompt, 3);

        Countdown(5); // Give user 5 seconds to start listing items

        // Simulate user listing items (in a real application, this would involve user input)
        int numberOfItems = random.Next(5, 15); // Random number of items between 5 and 15
        DisplayMessage($"You listed {numberOfItems} items.", 3);

        DisplayMessage($"Good job! You have completed Listing Activity. Total time: {durationInSeconds} seconds.", 3);
    }
}

public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.WriteLine("Choose an activity:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartActivity(new BreathingActivity(180)); // 3 minutes for Breathing Activity
                    break;
                case "2":
                    StartActivity(new ReflectionActivity(300)); // 5 minutes for Reflection Activity
                    break;
                case "3":
                    StartActivity(new ListingActivity(240)); // 4 minutes for Listing Activity
                    break;
                case "4":
                    Console.WriteLine("Exiting program...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number from 1 to 4.");
                    break;
            }
        }
    }

    private static void StartActivity(MindfulnessActivity activity)
    {
        Console.Clear();
        activity.Start();
        Console.WriteLine();
    }
}
