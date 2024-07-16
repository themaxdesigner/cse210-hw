using System;
using System.Collections.Generic;
using System.IO;

abstract class Goal
{
    public string Name { get; set; }
    public int Points { get; set; }
    public bool IsComplete { get; protected set; }

    public Goal(string name, int points)
    {
        Name = name;
        Points = points;
        IsComplete = false;
    }

    public abstract void RecordEvent();
    public abstract string GetStatus();
}

class SimpleGoal : Goal
{
    public SimpleGoal(string name, int points) : base(name, points) { }

    public override void RecordEvent()
    {
        IsComplete = true;
    }

    public override string GetStatus()
    {
        return IsComplete ? "[X] " + Name : "[ ] " + Name;
    }
}

class EternalGoal : Goal
{
    public EternalGoal(string name, int points) : base(name, points) { }

    public override void RecordEvent()
    {
        // Eternal goals are never marked as complete
    }

    public override string GetStatus()
    {
        return "[ ] " + Name;
    }
}

class ChecklistGoal : Goal
{
    private int TargetCount { get; set; }
    private int CurrentCount { get; set; }
    private int BonusPoints { get; set; }

    public ChecklistGoal(string name, int points, int targetCount, int bonusPoints)
        : base(name, points)
    {
        TargetCount = targetCount;
        CurrentCount = 0;
        BonusPoints = bonusPoints;
    }

    public override void RecordEvent()
    {
        CurrentCount++;
        if (CurrentCount >= TargetCount)
        {
            IsComplete = true;
        }
    }

    public override string GetStatus()
    {
        return IsComplete ? "[X] " + Name + $" (Completed {CurrentCount}/{TargetCount} times)" :
                            "[ ] " + Name + $" (Completed {CurrentCount}/{TargetCount} times)";
    }
}

class ProgressGoal : Goal
{
    private int TargetValue { get; set; }
    private int CurrentValue { get; set; }

    public ProgressGoal(string name, int points, int targetValue)
        : base(name, points)
    {
        TargetValue = targetValue;
        CurrentValue = 0;
    }

    public override void RecordEvent()
    {
        Console.Write("Enter progress value: ");
        int progress = int.Parse(Console.ReadLine());
        CurrentValue += progress;
        if (CurrentValue >= TargetValue)
        {
            IsComplete = true;
        }
    }

    public override string GetStatus()
    {
        return IsComplete ? "[X] " + Name + $" (Progress: {CurrentValue}/{TargetValue})" :
                            "[ ] " + Name + $" (Progress: {CurrentValue}/{TargetValue})";
    }
}

class NegativeGoal : Goal
{
    public NegativeGoal(string name, int points) : base(name, points) { }

    public override void RecordEvent()
    {
        IsComplete = true;
    }

    public override string GetStatus()
    {
        return IsComplete ? "[X] " + Name : "[ ] " + Name;
    }
}

class GoalManager
{
    private List<Goal> Goals { get; set; } = new List<Goal>();
    private int Score { get; set; } = 0;

    public void CreateGoal(Goal goal)
    {
        Goals.Add(goal);
    }

    public void RecordEvent(string goalName)
    {
        foreach (var goal in Goals)
        {
            if (goal.Name == goalName)
            {
                goal.RecordEvent();
                if (goal is NegativeGoal)
                {
                    Score -= goal.Points;
                }
                else
                {
                    Score += goal.Points;
                    if (goal is ChecklistGoal checklistGoal && checklistGoal.IsComplete)
                    {
                        Score += checklistGoal.BonusPoints;
                    }
                }
            }
        }
    }

    public void ShowGoals()
    {
        foreach (var goal in Goals)
        {
            Console.WriteLine(goal.GetStatus());
        }
    }

    public void ShowScore()
    {
        Console.WriteLine($"Current Score: {Score}");
    }

    public void SaveGoals(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(Score);
            foreach (var goal in Goals)
            {
                writer.WriteLine($"{goal.GetType().Name}|{goal.Name}|{goal.Points}|{goal.IsComplete}");
                if (goal is ChecklistGoal checklistGoal)
                {
                    writer.WriteLine($"{checklistGoal.CurrentCount}|{checklistGoal.TargetCount}|{checklistGoal.BonusPoints}");
                }
                else if (goal is ProgressGoal progressGoal)
                {
                    writer.WriteLine($"{progressGoal.CurrentValue}|{progressGoal.TargetValue}");
                }
            }
        }
    }

    public void LoadGoals(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            Score = int.Parse(reader.ReadLine());
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split('|');
                string type = parts[0];
                string name = parts[1];
                int points = int.Parse(parts[2]);
                bool isComplete = bool.Parse(parts[3]);

                Goal goal = type switch
                {
                    "SimpleGoal" => new SimpleGoal(name, points),
                    "EternalGoal" => new EternalGoal(name, points),
                    "ChecklistGoal" =>
                    {
                        int currentCount = int.Parse(reader.ReadLine());
                        int targetCount = int.Parse(reader.ReadLine());
                        int bonusPoints = int.Parse(reader.ReadLine());
                        var checklistGoal = new ChecklistGoal(name, points, targetCount, bonusPoints);
                        for (int i = 0; i < currentCount; i++)
                        {
                            checklistGoal.RecordEvent();
                        }
                        return checklistGoal;
                    },
                    "ProgressGoal" =>
                    {
                        int currentValue = int.Parse(reader.ReadLine());
                        int targetValue = int.Parse(reader.ReadLine());
                        var progressGoal = new ProgressGoal(name, points, targetValue);
                        while (progressGoal.CurrentValue < currentValue)
                        {
                            progressGoal.RecordEvent();
                        }
                        return progressGoal;
                    },
                    "NegativeGoal" => new NegativeGoal(name, points),
                    _ => throw new InvalidOperationException("Unknown goal type")
                };

                if (isComplete)
                {
                    goal.RecordEvent();
                }
                Goals.Add(goal);
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        GoalManager manager = new GoalManager();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Eternal Quest");
            Console.WriteLine("1. Create a new goal");
            Console.WriteLine("2. Record an event");
            Console.WriteLine("3. Show goals");
            Console.WriteLine("4. Show score");
            Console.WriteLine("5. Save goals");
            Console.WriteLine("6. Load goals");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Select goal type:");
                    Console.WriteLine("1. Simple Goal");
                    Console.WriteLine("2. Eternal Goal");
                    Console.WriteLine("3. Checklist Goal");
                    Console.WriteLine("4. Progress Goal");
                    Console.WriteLine("5. Negative Goal");
                    int goalType = int.Parse(Console.ReadLine());

                    Console.Write("Enter goal name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter goal points: ");
                    int points = int.Parse(Console.ReadLine());

                    Goal goal = goalType switch
                    {
                        1 => new SimpleGoal(name, points),
                        2 => new EternalGoal(name, points),
                        3 =>
                        {
                            Console.Write("Enter target count: ");
                            int targetCount = int.Parse(Console.ReadLine());
                            Console.Write("Enter bonus points: ");
                            int bonusPoints = int.Parse(Console.ReadLine());
                            return new ChecklistGoal(name, points, targetCount, bonusPoints);
                        },
                        4 =>
                        {
                            Console.Write("Enter target value: ");
                            int targetValue = int.Parse(Console.ReadLine());
                            return new ProgressGoal(name, points, targetValue);
                        },
                        5 => new NegativeGoal(name, points),
                        _ => throw new InvalidOperationException("Unknown goal type")
                    };

                    manager.CreateGoal(goal);
                    break;

                case 2:
                    Console.Write("Enter goal name to record: ");
                    string goalName = Console.ReadLine();
                    manager.RecordEvent(goalName);
                    break;

                case 3:
                    manager.ShowGoals();
                    Console.ReadKey();
                    break;

                case 4:
                    manager.ShowScore();
                    Console.ReadKey();
                    break;

                case 5:
                    Console.Write("Enter file path to save goals: ");
                    string savePath = Console.ReadLine();
                    manager.SaveGoals(savePath);
                    break;

                case 6:
                    Console.Write("Enter file path to load goals: ");
                    string loadPath = Console.ReadLine();
                    manager.LoadGoals(loadPath);
                    break;

                case 7:
                    return;
            }
        }
    }
}
