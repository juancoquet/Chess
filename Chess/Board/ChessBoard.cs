using Chess.Generics;
using Chess.Fen;

namespace Chess.Board;

public class ChessBoard
{
    public BitBoard BitBoard          { get; set; }
    public int[] Squares              { get; set; }
    public C Turn                     { get; set; }
    public int MoveNumber             { get; set; }
    public int HalfMoveClock          { get; set; }
    public Square EnPassantTarget     { get; set; }
    public ICastleRights CastleRights { get; set; }

    private List<BoardState> _history { get; set; } = new List<BoardState>();

    public static ChessBoard FromStartPosition()
    {
        var fenParser = new FenParser();
        return fenParser.Parse(FenParser.StartPosition);
    }

    public static ChessBoard FromFen(string fen)
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
            SquaresOccupants = Squares,
            Turn = Turn,
            MoveNumber = MoveNumber,
            HalfMoveClock = HalfMoveClock,
            EnPassantTarget = EnPassantTarget,
            CastleRights = CastleRights
        });
    }

    internal record BoardState
    {
        public BitBoard BitBoard          { get; init; }
        public int[] SquaresOccupants     { get; init; }  // 64 piece codes
        public C Turn                     { get; init; }
        public int MoveNumber             { get; init; }
        public int HalfMoveClock          { get; init; }
        public Square EnPassantTarget     { get; init; }
        public ICastleRights CastleRights { get; init; }
    }

    public bool IsValidMove(Move move)
    {
        // TODO: check colour matches turn
        // what defines a pseudo legal move?
        var pieceCodeFrom = Squares[(int)move.From];
        var pieceFrom = Piece.FromPieceCode(pieceCodeFrom);
        if (pieceFrom.Type == PType.None) return false;
        var pieceCodeTo = Squares[(int)move.To];
        var pieceTo = Piece.FromPieceCode(pieceCodeTo);
        if (pieceFrom.Colour == pieceTo.Colour && pieceTo.Type != PType.None) return false; // TODO: check castling
        return true;
    }

    private static C ColourFromPieceCode(int pieceCode) => (pieceCode & 0b1000) == 0 ? C.White : C.Black;
    private static PType PTypeFromPieceCode(int pieceCode) => (PType)(pieceCode & 0b111);
}

public class Piece
{
    public C Colour { get; set; }
    public PType Type { get; set; }
    public int PieceCode => (int)Colour << 3 | (int)Type;

    public Piece(C colour, PType pieceType)
    {
        Colour = colour;
        Type = pieceType;
    }

    public static Piece FromPieceCode(int pieceCode) => new Piece((C)(pieceCode >> 3), (PType)(pieceCode & 0b111));

    public static Piece None() => new Piece(C.White, PType.None);

    public bool Is(C colour, PType pieceType) => Colour == colour && Type == pieceType;

    public bool Is(C colour) => Colour == colour;

    public bool Is(PType pieceType) => Type == pieceType;

    public override bool Equals(object obj) => obj is Piece other &&
        Colour == other.Colour &&
        Type == other.Type;

    public override int GetHashCode() => HashCode.Combine(Colour, Type);
}

public record Move
{
    public Square From { get; init; }
    public Square To { get; init; }
}
