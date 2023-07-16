using Chess.Generics;
using Chess.Fen;

namespace Chess.Board;

public class ChessBoard
{
    public BitBoard BitBoard          { get; set; }
    public Piece[] Squares            { get; set; }
    public C Turn                { get; set; }
    public int MoveNumber             { get; set; }
    public int HalfMoveClock          { get; set; }
    public Square EnPassantTarget     { get; set; }
    public ICastleRights CastleRights { get; set; }

    private List<BoardState> _history { get; set; } = new List<BoardState>();

    public ChessBoard FromFen(string fen)
    {
        var fenParser = new FenParser(); 
        return fenParser.Parse(fen);
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
            EnPassantTarget = EnPassantTarget,
            CastleRights = CastleRights
        });
    }

    internal record BoardState
    {
        public required BitBoard BitBoard          { get; init; }
        public required Piece[] Squares            { get; init; }
        public required C Turn                { get; init; }
        public required int MoveNumber             { get; init; }
        public required int HalfMoveClock          { get; init; }
        public required Square EnPassantTarget     { get; init; }
        public required ICastleRights CastleRights { get; init; }
    }
}

public class Piece
{
    public C Colour { get; set; }
    public PType Type { get; set; }

    public Piece(C colour, PType pieceType)
    {
        Colour = colour;
        Type = pieceType;
    }

    public static Piece None() => new Piece(C.White, PType.None);

    public bool Is(C colour, PType pieceType) => Colour == colour && Type == pieceType;

    public bool Is(C colour) => Colour == colour;

    public bool Is(PType pieceType) => Type == pieceType;

    public override bool Equals(object obj) => obj is Piece other &&
        Colour == other.Colour &&
        Type == other.Type;

    public override int GetHashCode() => HashCode.Combine(Colour, Type);
}
