namespace Orchestrator;

class Program
{
    static void Main(string[] args)
    {
        var engine = new GameOrchestrator();
        engine.Run();
    }
}
