using Raylib_cs;

namespace Gui;

class Program
{
    static void Main(string[] args)
    {
        Raylib.InitWindow(800, 600, "Juan's Chess");
        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.DARKGRAY);
            Raylib.DrawText("Juan's chess engine", 12, 12, 20, Color.WHITE);
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}
