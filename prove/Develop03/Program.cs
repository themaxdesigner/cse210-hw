using System;
using System.Collections.Generic;
using System.Linq;

// Class representing a single word in the scripture
class Word
{
    private string _text;
    private bool _hidden;

    public Word(string text)
    {
        _text = text;
        _hidden = false;
    }

    public string GetText()
    {
        return _hidden ? new string('_', _text.Length) : _text;
    }

    public void Hide()
    {
        _hidden = true;
    }

    public bool IsHidden()
    {
        return _hidden;
    }
}

// Class representing a reference, handling single verses and verse ranges
class Reference
{
    private string _book;
    private string _chapter;
    private string _verses;

    // Constructor for single verse reference
    public Reference(string book, string chapter, string verse)
    {
        _book = book;
        _chapter = chapter;
        _verses = verse;
    }

    // Constructor for verse range reference
    public Reference(string book, string chapter, string startVerse, string endVerse)
    {
        _book = book;
        _chapter = chapter;
        _verses = $"{startVerse}-{endVerse}";
    }

    public string GetReference()
    {
        return $"{_book} {_chapter}:{_verses}";
    }
}

// Class representing a scripture, storing the reference and words
class Scripture
{
    private Reference _reference;
    private List<Word> _words;
    private Random _random;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
        _random = new Random();
    }

    public void Display()
    {
        Console.Clear();
        Console.WriteLine($"Scripture Reference: {_reference.GetReference()}");
        foreach (var word in _words)
        {
            Console.Write($"{word.GetText()} ");
        }
        Console.WriteLine();
    }

    public bool HideRandomWord()
    {
        var visibleWords = _words.Where(word => !word.IsHidden()).ToList();
        if (visibleWords.Count == 0)
        {
            return false; // All words are hidden
        }

        // Hide a few random words (e.g., 3 words)
        int wordsToHide = Math.Min(3, visibleWords.Count);
        for (int i = 0; i < wordsToHide; i++)
        {
            var index = _random.Next(visibleWords.Count);
            visibleWords[index].Hide();
            visibleWords.RemoveAt(index);
        }

        return true;
    }
}

// Class to manage the library of scriptures
class ScriptureLibrary
{
    private List<Scripture> _scriptures;
    private Random _random;

    public ScriptureLibrary()
    {
        _scriptures = new List<Scripture>();
        _random = new Random();

        // Add scriptures to the library
        _scriptures.Add(new Scripture(new Reference("Proverbs", "3", "5", "6"), "Trust in the Lord with all thine heart; and lean not unto thine own understanding. In all thy ways acknowledge him, and he shall direct thy paths."));
        _scriptures.Add(new Scripture(new Reference("John", "3", "16"), "For God so loved the world, that he gave his only begotten Son, that whosoever believeth in him should not perish, but have everlasting life."));
        _scriptures.Add(new Scripture(new Reference("Psalm", "23", "1"), "The Lord is my shepherd; I shall not want."));
        // Add more scriptures as needed
    }

    public Scripture GetRandomScripture()
    {
        var index = _random.Next(_scriptures.Count);
        return _scriptures[index];
    }
}

class Program
{
    static void Main(string[] args)
    {
        var scriptureLibrary = new ScriptureLibrary();
        var scripture = scriptureLibrary.GetRandomScripture();

        while (true)
        {
            scripture.Display();
            Console.WriteLine("Press Enter to hide a word, or type 'quit' to exit.");
            var input = Console.ReadLine();

            if (input.ToLower() == "quit")
            {
                break;
            }

            var success = scripture.HideRandomWord();
            if (!success)
            {
                Console.WriteLine("All words are hidden. Press Enter to exit.");
                Console.ReadLine();
                break;
            }
        }
    }
}
