using Chess.Orchestration;

namespace Chess;

class Program
{
    static void Main(string[] args)
    {
        var engine = new Orchestrator();
        engine.Run();
    }
}
