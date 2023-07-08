using ConsoleExtensions;

namespace Engine;

class ChessEngine
{
    private InputProcessor _inputProcessor;

    public ChessEngine()
    {
        _inputProcessor = new InputProcessor();
    }

    public void Run()
    {
        Terminal.WriteLine("starting chess engine...");
        while (true)
        {
            if (!ProcessInput(50))
            {
                break;
            }
        }
    }

    private bool ProcessInput(int waitMs = 50)
    {
        Terminal.Write("\rchess > ", ConsoleColor.Green);
        if (_inputProcessor.IsNewInputAvailable())
        {
            var input = _inputProcessor.GetNewInput();
            if (input.Length > 0)
            {
                return HandleInput(input);
            }
        }
        else
        {
            Thread.Sleep(waitMs);
        }
        return true;
    }

    private bool HandleInput(string input)
    {
        if (input == "q")
        {
            return false;
        }
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

    public bool IsNewInputAvailable()
    {
        return Console.KeyAvailable;
    }

    public string GetNewInput()
    {
        return Console.ReadLine() ?? "";
    }
}
