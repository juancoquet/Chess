using Generics;

namespace Board;

public class ChessBoard
{
    public BitBoard BitBoard          { get; set; } = new BitBoard();
    public Piece[] Squares            { get; set; }
    public Colour Turn                { get; set; } = Colour.White;
    public int MoveNumber             { get; set; } = 1;
    public int HalfMoveClock          { get; set; } = 1;
    public bool InCheck               { get; set; } = false;
    public Square EnPassantTarget     { get; set; } = Square.None;
    public ICastleRights CastleRights { get; set; }

    private List<BoardState> _history { get; set; } = new List<BoardState>();

    public ChessBoard()
    {
        Squares = Enumerable.Repeat(Piece.None(), 64).ToArray();
        CastleRights = new CastleRightsState();
        RecordState();
    }

    private void PlacePiece(Square sq, Colour colour, PieceType pieceType)
    {
        Squares[(int)sq] = new Piece(colour, pieceType);
    }


    public interface ICastleRights
    {
        ECastleRights White { get; set; }
        ECastleRights Black { get; set; }
    }

    internal class CastleRightsState : ICastleRights
    {
        public ECastleRights White { get; set; } = ECastleRights.BothSides;
        public ECastleRights Black { get; set; } = ECastleRights.BothSides;
    }

    private void RecordState()
    {
        _history.Add(new BoardState()
        {
            BitBoard = BitBoard,
            Squares = Squares,
            Turn = Turn,
            MoveNumber = MoveNumber,
            HalfMoveClock = HalfMoveClock,
            InCheck = InCheck,
            EnPassantTarget = EnPassantTarget,
            CastleRights = CastleRights
        });
    }

    internal record BoardState
    {
        public required BitBoard BitBoard          { get; init; }
        public required Piece[] Squares            { get; init; }
        public required Colour Turn                { get; init; }
        public required int MoveNumber             { get; init; }
        public required int HalfMoveClock          { get; init; }
        public required bool InCheck               { get; init; }
        public required Square EnPassantTarget     { get; init; }
        public required ICastleRights CastleRights { get; init; }
    }
}

public class Piece
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
