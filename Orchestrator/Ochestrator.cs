using Raylib_cs;

using Chess.Board;
using Chess.Generics;
using ConsoleExtensions;
using Gui.Game;

namespace Orchestrator;

class GameOrchestrator
{
    private InputProcessor _inputProcessor = new InputProcessor();
    private ChessBoard _board = new ChessBoard();

    public GameOrchestrator() { }

    public void Run()
    {
        Terminal.WriteLine("starting chess engine...");
        var board = ChessBoard.FromStartPosition();
        var gui = new GameUI();

        while (!gui.Quit())
        {
            gui.DrawBoard();
            gui.DrawGameState(board);
            if (gui.PlayerHasMoved())
            {
                var proposedMove = gui.ProposeMove();
                // var mousePos = Raylib.GetMousePosition();
                // var pieceCode = boardSquares[(int)_fromSquare];
                // var ptype = PTypeFromPieceCode(pieceCode); // likely move this function to board
                // var colour = ColourFromPieceCode(pieceCode); // likely move this function to baard
                // if (ptype == PType.None) return boardSquares;
                // boardSquares[(int)_fromSquare] = 0;
                // boardSquares[(int)toSquare] = pieceCode;
                Console.Write("\r" + new string(' ', Console.WindowWidth));
                Console.WriteLine($"\r{proposedMove.From} to {proposedMove.To}");
            }
            gui.EndDraw();
            if (!ProcessInput(50)) { break; }
        }
    }

    private bool ProcessInput(int waitMs = 50)
    {
        Terminal.Write("\rchess> ", ConsoleColor.Green);
        if (_inputProcessor.IsNewInputAvailable)
        {
            var input = _inputProcessor.NewInput;
            if (input.Length > 0) { return HandleInput(input); }
        }
        else { Thread.Sleep(waitMs); }
        return true;
    }

    private bool HandleInput(string input)
    {
        if (input == "q") { return false; }
        SendUciCommand(input);
        return true;
    }

    private void SendUciCommand(string command)
    {
        Terminal.Write(" sending uci command: ", ConsoleColor.Yellow);
        Terminal.WriteLine(command);
    }
}

class InputProcessor
{
    public InputProcessor() { }

    public bool IsNewInputAvailable => Console.KeyAvailable;

    public string NewInput => Console.ReadLine() ?? "";
}
