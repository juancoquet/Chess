using Raylib_cs;
using Chess.Generics;

namespace Gui;

class Program
{
    static void Main(string[] args)
    {
        var sqSize = 100;
        var boardSize = sqSize * 8;
        var windowWidth = 800; ;
        var windowHeight = 800;
        var light = new Color(124, 133, 147, 255);
        var dark = new Color(47, 54, 66, 255);

        Raylib.SetTraceLogLevel(TraceLogLevel.LOG_WARNING);
        Raylib.InitWindow(windowWidth, windowHeight, "Juan's Chess");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            foreach (var square in Enum.GetValues(typeof(Square)))
            {
                if (square is Square.None) continue;
                var file = (int)square % 8;
                var rank = (int)square / 8;
                var x = file * sqSize;
                var y = (7 - rank) * sqSize;
                var color = (file + rank) % 2 == 0 ? light : dark;
                Raylib.DrawRectangle(x, y, sqSize, sqSize, color);
            }

            var texture = Raylib.LoadTexture("Gui/Graphics/Wn.png");
            Raylib.DrawTexture(texture, 0, 0, Color.WHITE);

            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}
