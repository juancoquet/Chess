using Gui.Game;

using Raylib_cs;

using Chess.Board;
using Chess.Generics;

namespace Gui;

class Program
{
    static void Main(string[] args)
    {
        var board = ChessBoard.FromStartPosition();
        var gui = new GameUI();
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            gui.DrawBoard();
            var wpawn = new Piece(C.White, PType.WPawn);
            gui.DrawGameState(board);

            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}
