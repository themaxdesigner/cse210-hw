using System;
using System.Collections.Generic;

// Comment class to track the name of the commenter and the text of the comment
public class Comment
{
    public string Name { get; set; }
    public string Text { get; set; }

    public Comment(string name, string text)
    {
        Name = name;
        Text = text;
    }

    public override string ToString()
    {
        return $"{Name}: {Text}";
    }
}

// Video class to track the title, author, length, and comments of the video
public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; }
    private List<Comment> comments;

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return comments.Count;
    }

    public void DisplayVideoInfo()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {Length} seconds");
        Console.WriteLine($"Number of Comments: {GetNumberOfComments()}");
        Console.WriteLine("Comments:");
        foreach (var comment in comments)
        {
            Console.WriteLine(comment);
        }
    }
}

// Main program class
class Program
{
    static void Main(string[] args)
    {
        // Create video instances
        Video video1 = new Video("Learning C#", "John Doe", 300);
        Video video2 = new Video("Cooking Pasta", "Jane Smith", 600);
        Video video3 = new Video("Travel Vlog: Japan", "Alice Johnson", 1200);

        // Add comments to videos
        video1.AddComment(new Comment("Mike", "Great tutorial!"));
        video1.AddComment(new Comment("Sara", "Very helpful, thanks!"));
        video1.AddComment(new Comment("Tom", "I love this video!"));

        video2.AddComment(new Comment("Anna", "Looks delicious!"));
        video2.AddComment(new Comment("Bob", "I will try this recipe!"));
        video2.AddComment(new Comment("Cindy", "Yummy!"));

        video3.AddComment(new Comment("David", "Amazing places!"));
        video3.AddComment(new Comment("Eva", "Japan is beautiful!"));
        video3.AddComment(new Comment("Frank", "I want to visit Japan now!"));

        // Add videos to a list
        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Display information for each video
        foreach (var video in videos)
        {
            video.DisplayVideoInfo();
            Console.WriteLine();
        }
    }
}
