using Chess.Board;
using Chess.Generics;
using ConsoleExtensions;
using Gui.Game;

namespace Orchestrator;

class GameOrchestrator
{
    private InputProcessor _inputProcessor = new InputProcessor();
    private ChessBoard _board = new ChessBoard();
    private GameUI _gui = new GameUI();
    private Square _moveFrom = Square.None;
    private Square _moveTo = Square.None;

    public GameOrchestrator() { }

    public void Run()
    {
        Terminal.WriteLine("starting chess engine...");
        var board = ChessBoard.FromStartPosition();

        while (!_gui.Quit())
        {
            _gui.DrawBoard();
            _gui.DrawGameState(board.SquaresOccupants);
            if (PlayerHasMoved())
            {
                var proposedMove = new Move()
                {
                    From = _moveFrom,
                    To = _moveTo
                };
                if (board.IsValidMove(proposedMove))
                {
                    board.MakeMove(proposedMove);
                    Console.Write("\r" + new string(' ', Console.WindowWidth));
                    Console.WriteLine($"\r{proposedMove.From} to {proposedMove.To}");
                }
            }
            _gui.EndDraw();
            if (!ProcessInput(50)) { break; }
        }
    }

    /// <summary>
    /// Called every frame to check if a player has made a move. Passively updates the _moveFrom and _moveTo fields.
    /// </summary>
    public bool PlayerHasMoved()
    {
        var mousePos = _gui.MousePosition();
        if (_gui.DragBegins())
        {
            _moveFrom = _gui.GetSquareUnderCursor(mousePos);
        }
        if (_gui.DragEnds())
        {
            _moveTo = _gui.GetSquareUnderCursor(mousePos);
            return _moveTo != Square.None && _moveFrom != _moveTo;
        }
        return false;
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
