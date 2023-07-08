namespace ConsoleExtensions;

public static class Terminal
{
    public static void Write(string message, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
