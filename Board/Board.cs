using Generics;

namespace Board;

public class ChessBoard
{
    public BitBoard BitBoard { get; set; }
    public IPiece[] Squares  { get; set; }
    public Colour Turn       { get; set; } = Colour.White;
    public int MoveNumber    { get; set; } = 1;
    public bool InCheck      { get; set; } = false;

    private List<BoardState> _history { get; set; } = new List<BoardState>();

    public ChessBoard()
    {
        BitBoard = new BitBoard();
        Squares = Enumerable.Repeat(Piece.None(), 64).ToArray();
        RecordState();
    }

    private void PlacePiece(Squares sq, Colour colour, PieceType pieceType)
    {
        Squares[(int)sq] = new Piece(colour, pieceType);
    }

    public interface IPiece
    {
        Colour Colour { get; set; }
        PieceType Type { get; set; }
    }

    internal class Piece : IPiece
    {
        public Colour Colour { get; set; }
        public PieceType Type { get; set; }

        public Piece(Colour colour, PieceType pieceType)
        {
            Colour = colour;
            Type = pieceType;
        }

        public static Piece None() => new Piece(Colour.None, PieceType.None);
    }

    private void RecordState()
    {
        _history.Add(new BoardState()
        {
            BitBoard = BitBoard,
            Squares = Squares,
            Turn = Turn,
            MoveNumber = MoveNumber,
            InCheck = InCheck,
        });
    }

    internal record BoardState
    {
        public required BitBoard BitBoard { get; init; }
        public required IPiece[] Squares  { get; init; }
        public required Colour Turn       { get; init; }
        public required int MoveNumber    { get; init; }
        public required bool InCheck      { get; init; }
    }
}
