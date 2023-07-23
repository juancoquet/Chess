using System.Numerics;

using Raylib_cs;

using Chess.Generics;
using Chess.Board;

namespace Gui.Game;

public class GameUI
{
    private int sqSize;
    private int graphicSize;
    private int windowWidth;
    private int windowHeight;
    private Color light;
    private Color dark;
    private Dictionary<int, Texture2D> sprites;

    private Square _fromSquare = Square.None;

    public GameUI()
    {
        sqSize = 100;
        graphicSize = (int)(sqSize * 0.8);
        windowWidth = 800;
        windowHeight = 800;
        light = new Color(124, 133, 147, 255);
        dark = new Color(47, 54, 66, 255);

        Raylib.SetTraceLogLevel(TraceLogLevel.LOG_WARNING);
        Raylib.InitWindow(windowWidth, windowHeight, "Juan's Chess");
        Raylib.SetTargetFPS(60);
        sprites = LoadSprites();
    }

    public void DrawBoard()
    {
        foreach (var square in Enum.GetValues(typeof(Square)))
        {
            if (square is Square.None)
                continue;
            var file = (int)square % 8;
            var rank = (int)square / 8;
            var x = file * sqSize;
            var y = (7 - rank) * sqSize;
            var color = (file + rank) % 2 == 0 ? light : dark;
            Raylib.DrawRectangle(x, y, sqSize, sqSize, color);
        }
    }

    public void DrawGameState(ChessBoard board)
    {
        for (var i = 0; i < 64; i++)
        {
            var piece = board.Squares[i];
            if (piece == 0) // empty square
                continue;
            DrawPiece(piece, (Square)i);
        }
    }

    public void DrawPiece(int piece, Square square)
    {
        var file = (int)square % 8;
        var rank = (int)square / 8;
        var x = file * sqSize + (sqSize - graphicSize) / 2;
        var y = (7 - rank) * sqSize + (sqSize - graphicSize) / 2;
        var sprite = sprites[piece];
        Raylib.DrawTexturePro(
            sprite,
            new Rectangle(0, 0, sprite.width, sprite.height),
            new Rectangle(x, y, graphicSize, graphicSize),
            new Vector2(0, 0),
            0,
            Color.WHITE
        );
    }

    private static Dictionary<int, Texture2D> LoadSprites()
    {
        var colours = Enum.GetValues(typeof(C)).Cast<C>();
        var pieceTypes = Enum.GetValues(typeof(PType))
            .Cast<PType>()
            .Where(type => type != PType.None);

        var sprites = colours.SelectMany(
            colour => pieceTypes.Select(
                pieceType =>
                {
                    var pieceCode = new Piece(colour, pieceType).PieceCode;
                    var texture = Raylib.LoadTexture($"Gui/Sprites/{colour}{pieceType}.png");
                    return (pieceCode, texture);
                }
            )
        )
        .ToDictionary(tuple => tuple.pieceCode, tuple => tuple.texture);

        return sprites;
    }

    public int[] Move(int[] boardSquares)
    {
        var mousePos = Raylib.GetMousePosition();

        if (DragBegins())
        {
            _fromSquare = GetSquareUnderCursor(mousePos);
        }
        if (DragEnds())
        {
            var toSquare = GetSquareUnderCursor(mousePos);
            if (toSquare == Square.None) return boardSquares;
            var pieceCode = boardSquares[(int)_fromSquare];
            var ptype = PTypeFromPieceCode(pieceCode);
            var colour = ColourFromPieceCode(pieceCode);
            if (ptype == PType.None) return boardSquares;
            boardSquares[(int)_fromSquare] = 0;
            boardSquares[(int)toSquare] = pieceCode;

            Console.Write("\r" + new string(' ', Console.WindowWidth));
            Console.WriteLine($"\r{colour} {ptype} on {_fromSquare} to {toSquare}");
        }
        return boardSquares;
    }

    public Square GetSquareUnderCursor(Vector2 mousePos)
    {
        if (mousePos.X > sqSize * 8 || mousePos.Y > sqSize * 8 || mousePos.X < 0 || mousePos.Y < 0)
            return Square.None;

        var file = (int)mousePos.X / sqSize;
        var rank = 7 - (int)mousePos.Y / sqSize;
        var square = (Square)(rank * 8 + file);
        return square;
    }

    private static bool DragBegins() => Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT);
    private static bool DragEnds() => Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT);
    private static C ColourFromPieceCode(int pieceCode) => (pieceCode & 0b1000) == 0 ? C.White : C.Black;
    private static PType PTypeFromPieceCode(int pieceCode) => (PType)(pieceCode & 0b111);
}
