using System.Text;

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

    public static string StringifyBitBoard(ulong bitBoard)
    {
        var ranks = new List<string>();
        for (int rank = 0; rank < 8; rank++)
        {
            var sb = new StringBuilder();
            for (int file = 0; file < 8; file++)
            {
                var square = (ulong)1 << (rank * 8 + file);
                if (file > 0) { sb.Append(" "); }
                sb.Append((bitBoard & square) == square ? "1" : "0");
            }
            ranks.Add(sb.ToString());
        }
        ranks.Reverse();
        return string.Join("\n", ranks);
    }
}
