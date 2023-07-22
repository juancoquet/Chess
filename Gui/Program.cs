using Raylib_cs;
using Chess.Generics;

namespace Gui;

class Program
{
    static void Main(string[] args)
    {
        Raylib.InitWindow(800, 600, "Juan's Chess");
        Raylib.SetTargetFPS(60);
        var s = Square.H7;
        Console.WriteLine(s);
        Console.WriteLine(s.File().Index());
        Console.WriteLine(s.Rank().Index());

        // foreach (var square in Enum.GetValues(typeof(Square)))
        // {
        //     Console.WriteLine($"{square} = {square.GetHashCode()}");
        // }

        // while (!Raylib.WindowShouldClose())
        // {
        //     Raylib.BeginDrawing();
        //     Raylib.ClearBackground(Color.DARKGRAY);
        //     Raylib.DrawText("Juan's chess engine", 12, 12, 20, Color.WHITE);
        //     Raylib.EndDrawing();
        // }
        // Raylib.CloseWindow();
    }
}
