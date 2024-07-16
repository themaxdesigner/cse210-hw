using System;
using System.Collections.Generic;
using System.Threading;

abstract class MindfulnessActivity
{
    protected int Duration { get; set; }

    public void Start(string name, string description)
    {
        Console.Clear();
        Console.WriteLine($"Starting {name} Activity");
        Console.WriteLine(description);
        Console.Write("Enter duration (in seconds): ");
        Duration = int.Parse(Console.ReadLine());
        Console.WriteLine("Prepare to begin...");
        PauseWithAnimation(3);
    }

    public void Finish(string name)
    {
        Console.WriteLine("Good job!");
        Console.WriteLine($"You have completed the {name} Activity for {Duration} seconds.");
        PauseWithAnimation(3);
    }

    protected void PauseWithAnimation(int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }

    public abstract void PerformActivity();
}

class BreathingActivity : MindfulnessActivity
{
    public override void PerformActivity()
    {
        Start("Breathing", "This activity will help you relax by guiding you through breathing in and out slowly. Clear your mind and focus on your breathing.");
        int timeElapsed = 0;
        while (timeElapsed < Duration)
        {
            Console.WriteLine("Breathe in...");
            PauseWithAnimation(3);
            Console.WriteLine("Breathe out...");
            PauseWithAnimation(3);
            timeElapsed += 6;
        }
        Finish("Breathing");
    }
}

class ReflectionActivity : MindfulnessActivity
{
    private static readonly List<string> Prompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private static readonly List<string> Questions = new List<string>
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

    private List<string> promptsSession;
    private List<string> questionsSession;

    private void Shuffle<T>(List<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void InitializeSessionLists()
    {
        promptsSession = new List<string>(Prompts);
        questionsSession = new List<string>(Questions);
        Shuffle(promptsSession);
        Shuffle(questionsSession);
    }

    private string GetNextPrompt()
    {
        if (promptsSession.Count == 0)
        {
            InitializeSessionLists();
        }
        string prompt = promptsSession[0];
        promptsSession.RemoveAt(0);
        return prompt;
    }

    private string GetNextQuestion()
    {
        if (questionsSession.Count == 0)
        {
            InitializeSessionLists();
        }
        string question = questionsSession[0];
        questionsSession.RemoveAt(0);
        return question;
    }

    public override void PerformActivity()
    {
        InitializeSessionLists();
        Start("Reflection", "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.");

        Console.WriteLine(GetNextPrompt());
        PauseWithAnimation(5);

        int timeElapsed = 0;
        while (timeElapsed < Duration)
        {
            Console.WriteLine(GetNextQuestion());
            PauseWithAnimation(5);
            timeElapsed += 5;
        }
        Finish("Reflection");
    }
}

class ListingActivity : MindfulnessActivity
{
    private static readonly List<string> Prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    private List<string> promptsSession;

    private void Shuffle<T>(List<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void InitializeSessionLists()
    {
        promptsSession = new List<string>(Prompts);
        Shuffle(promptsSession);
    }

    private string GetNextPrompt()
    {
        if (promptsSession.Count == 0)
        {
            InitializeSessionLists();
        }
        string prompt = promptsSession[0];
        promptsSession.RemoveAt(0);
        return prompt;
    }

    public override void PerformActivity()
    {
        InitializeSessionLists();
        Start("Listing", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.");

        Console.WriteLine(GetNextPrompt());
        PauseWithAnimation(5);

        List<string> items = new List<string>();
        int timeElapsed = 0;
        while (timeElapsed < Duration)
        {
            Console.Write("Enter an item: ");
            string item = Console.ReadLine();
            items.Add(item);
            timeElapsed += 5;
        }

        Console.WriteLine($"You listed {items.Count} items.");
        Finish("Listing");
    }
}

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose an activity:");
            Console.WriteLine("1. Breathing");
            Console.WriteLine("2. Reflection");
            Console.WriteLine("3. Listing");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            MindfulnessActivity activity = choice switch
            {
                1 => new BreathingActivity(),
                2 => new ReflectionActivity(),
                3 => new ListingActivity(),
                _ => null
            };

            if (activity == null)
                break;

            activity.PerformActivity();
        }
    }
}
