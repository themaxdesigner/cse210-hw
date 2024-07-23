using System;
using System.Collections.Generic;

// Base Activity Class
abstract class Activity
{
    private DateTime date;
    private int length; // in minutes

    public Activity(DateTime date, int length)
    {
        this.date = date;
        this.length = length;
    }

    public DateTime GetDate()
    {
        return date;
    }

    public int GetLength()
    {
        return length;
    }

    public abstract double GetDistance();
    public abstract double GetSpeed();
    public abstract double GetPace();

    public string GetSummary()
    {
        return $"{date.ToString("dd MMM yyyy")} {GetType().Name} ({length} min) - Distance: {GetDistance():0.0} miles, Speed: {GetSpeed():0.0} mph, Pace: {GetPace():0.0} min per mile";
    }
}

// Running Class
class Running : Activity
{
    private double distance; // in miles

    public Running(DateTime date, int length, double distance) : base(date, length)
    {
        this.distance = distance;
    }

    public override double GetDistance()
    {
        return distance;
    }

    public override double GetSpeed()
    {
        return (distance / GetLength()) * 60;
    }

    public override double GetPace()
    {
        return GetLength() / distance;
    }
}

// Cycling Class
class Cycling : Activity
{
    private double speed; // in mph

    public Cycling(DateTime date, int length, double speed) : base(date, length)
    {
        this.speed = speed;
    }

    public override double GetDistance()
    {
        return (speed * GetLength()) / 60;
    }

    public override double GetSpeed()
    {
        return speed;
    }

    public override double GetPace()
    {
        return 60 / speed;
    }
}

// Swimming Class
class Swimming : Activity
{
    private int laps;

    public Swimming(DateTime date, int length, int laps) : base(date, length)
    {
        this.laps = laps;
    }

    public override double GetDistance()
    {
        return laps * 50 / 1000.0 * 0.62;
    }

    public override double GetSpeed()
    {
        return (GetDistance() / GetLength()) * 60;
    }

    public override double GetPace()
    {
        return GetLength() / GetDistance();
    }
}

// Main Program
class Program
{
    static void Main(string[] args)
    {
        // Create activities
        Activity running = new Running(new DateTime(2022, 11, 3), 30, 3.0);
        Activity cycling = new Cycling(new DateTime(2022, 11, 4), 45, 15.0);
        Activity swimming = new Swimming(new DateTime(2022, 11, 5), 60, 20);

        // Add activities to a list
        List<Activity> activities = new List<Activity> { running, cycling, swimming };

        // Generate and display summaries for each activity
        foreach (var activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
