using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
    class Program
    {
        static void Main(string[] args)
        {
            EternalQuestApp app = new EternalQuestApp();
            app.Run();
            Console.WriteLine("Goodbye! Thank you for using the Eternal Quest Program.");
        }
    }

    public abstract class Goal
    {
        private string _name;
        private string _description;
        protected int _points;
        protected bool _isCompleted;

        public string Name { get => _name; }
        public string Description { get => _description; }
        public int Points { get => _points; }
        public bool IsCompleted { get => _isCompleted; }

        public Goal(string name, string description, int points)
        {
            _name = name;
            _description = description;
            _points = points;
            _isCompleted = false;
        }

        public abstract void RecordProgress();
        public abstract string GetStatus();
        public abstract string Serialize();
        public abstract void Deserialize(string data);
    }

    public class SimpleGoal : Goal
    {
        public SimpleGoal(string name, string description, int points) : base(name, description, points) { }

        public override void RecordProgress()
        {
            _isCompleted = true;
        }

        public override string GetStatus()
        {
            return _isCompleted ? "[X] " + Name : "[ ] " + Name;
        }

        public override string Serialize()
        {
            return $"SimpleGoal:{Name},{Description},{Points},{_isCompleted}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split(',');
            _points = int.Parse(parts[2]);
            _isCompleted = bool.Parse(parts[3]);
        }
    }

    public class EternalGoal : Goal
    {
        public EternalGoal(string name, string description, int points) : base(name, description, points) { }

        public override void RecordProgress()
        {
            // Eternal goals are never completed, just accumulate points
        }

        public override string GetStatus()
        {
            return "[∞] " + Name;
        }

        public override string Serialize()
        {
            return $"EternalGoal:{Name},{Description},{Points}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split(',');
            _points = int.Parse(parts[2]);
        }
    }

    public class ChecklistGoal : Goal
    {
        private int _targetCount;
        private int _currentCount;
        private int _bonusPoints;

        public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints) : base(name, description, points)
        {
            _targetCount = targetCount;
            _currentCount = 0;
            _bonusPoints = bonusPoints;
        }

        public override void RecordProgress()
        {
            if (_currentCount < _targetCount)
            {
                _currentCount++;
                if (_currentCount == _targetCount)
                {
                    _isCompleted = true;
                }
            }
        }

        public override string GetStatus()
        {
            return _isCompleted ? $"[X] {Name} (Completed {_currentCount}/{_targetCount} times)" : $"[ ] {Name} (Completed {_currentCount}/{_targetCount} times)";
        }

        public override string Serialize()
        {
            return $"ChecklistGoal:{Name},{Description},{Points},{_currentCount},{_targetCount},{_bonusPoints},{_isCompleted}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split(',');
            _points = int.Parse(parts[2]);
            _currentCount = int.Parse(parts[3]);
            _targetCount = int.Parse(parts[4]);
            _bonusPoints = int.Parse(parts[5]);
            _isCompleted = bool.Parse(parts[6]);
        }

        public int CalculateBonusPoints()
        {
            if (_isCompleted)
            {
                return _points + _bonusPoints;
            }
            return _points;
        }
    }

    public class EternalQuestApp
    {
        private List<Goal> _goals;
        private int _score;

        public EternalQuestApp()
        {
            _goals = new List<Goal>();
            _score = 0;
        }

        public void Run()
        {
            string input;
            do
            {
                ShowMenu();
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        CreateNewGoal();
                        break;
                    case "2":
                        RecordEvent();
                        break;
                    case "3":
                        ShowGoals();
                        break;
                    case "4":
                        SaveGoals();
                        break;
                    case "5":
                        LoadGoals();
                        break;
                    case "6":
                        Console.WriteLine($"Your current score is: {_score}");
                        break;
                }
            } while (input != "7");
        }

        private void ShowMenu()
        {
            Console.WriteLine("Welcome to the Eternal Quest Program!!!");
            Console.WriteLine("Share this program to family, friends and loved ones");
            Console.WriteLine();
            Console.WriteLine("Eternal Quest Menu:");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. Record Event");
            Console.WriteLine("3. Show Goals");
            Console.WriteLine("4. Save Goals");
            Console.WriteLine("5. Load Goals");
            Console.WriteLine("6. Show Score");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: (eg). 1, 2, 3, 4, 5, 6, 7:");
        }

        private void CreateNewGoal()
        {
            Console.WriteLine("Choose a type of goal:");
            Console.WriteLine("1. Simple Goal");
            Console.WriteLine("2. Eternal Goal");
            Console.WriteLine("3. Checklist Goal");
            string choice = Console.ReadLine();
            Console.Write("Enter the name of the goal: ");
            string name = Console.ReadLine();
            Console.Write("Enter the description of the goal: ");
            string description = Console.ReadLine();
            Console.Write("Enter the points for the goal: ");
            int points = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case "1":
                    _goals.Add(new SimpleGoal(name, description, points));
                    break;
                case "2":
                    _goals.Add(new EternalGoal(name, description, points));
                    break;
                case "3":
                    Console.Write("Enter the number of times the goal must be completed: ");
                    int targetCount = int.Parse(Console.ReadLine());
                    Console.Write("Enter the bonus points for completing the goal: ");
                    int bonusPoints = int.Parse(Console.ReadLine());
                    _goals.Add(new ChecklistGoal(name, description, points, targetCount, bonusPoints));
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }

        private void RecordEvent()
        {
            Console.WriteLine("Choose a goal to record progress:");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].Name}");
            }
            if (int.TryParse(Console.ReadLine(), out int goalIndex) && goalIndex > 0 && goalIndex <= _goals.Count)
            {
                goalIndex -= 1; // Adjusting index to zero-based
                Goal goal = _goals[goalIndex];
                goal.RecordProgress();
                if (goal is SimpleGoal simpleGoal && simpleGoal.IsCompleted)
                {
                    _score += simpleGoal.Points;
                }

                else if (goal is EternalGoal)
                {
                    _score += goal.Points;
                }
                else if (goal is ChecklistGoal checklistGoal)
                {
                    _score += checklistGoal.CalculateBonusPoints(); // Calculate bonus points
                }
                Console.WriteLine("Progress recorded!");
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        private void ShowGoals()
        {
            Console.WriteLine("Your goals:");
            foreach (var goal in _goals)
            {
                Console.WriteLine(goal.GetStatus());
            }
        }

        private void SaveGoals()
        {
            try
            {
                Console.Write("Enter the filename to save goals: ");
                string filename = Console.ReadLine().Trim() + ".txt";
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine(_score);
                    foreach (var goal in _goals)
                    {
                        writer.WriteLine(goal.Serialize());
                    }
                }
                Console.WriteLine($"Goals saved to {filename}!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving goals: {ex.Message}");
            }
        }

        private void LoadGoals()
        {
            try
            {
                Console.Write("Enter the filename to load goals: ");
                string filename = Console.ReadLine().Trim();
                if (File.Exists(filename))
                {
                    _goals.Clear();
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        _score = int.Parse(reader.ReadLine());
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine("Line read: " + line);
                            var parts = line.Split(':');
                            if (parts.Length >= 2)
                            {
                                string type = parts[0];
                                string data = parts[1];
                                Goal goal = null;
                                switch (type)
                                {
                                    case "SimpleGoal":
                                        goal = new SimpleGoal("", "", 0);
                                        break;
                                    case "EternalGoal":
                                        goal = new EternalGoal("", "", 0);
                                        break;
                                    case "ChecklistGoal":
                                        goal = new ChecklistGoal("", "", 0, 0, 0);
                                        break;
                                }
                                if (goal != null)
                                {
                                    Console.WriteLine("Goal type: " + type);
                                    Console.WriteLine("Goal data: " + data);
                                    goal.Deserialize(data);
                                    _goals.Add(goal);
                                }
                            }
                        }
                    }
                    Console.WriteLine("Goals loaded!");
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading goals: {ex.Message}");
            }
        }
    }
}
