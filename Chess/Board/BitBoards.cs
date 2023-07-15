using Chess.Generics;

namespace Chess.Board;

/// <summary>
/// Represents the state of all the pieces on the board.
/// Each piece type is represented by a ulong, where each bit represents a square.
/// A 1 means that a piece of that type is on that square, and a 0 means that it is not.
/// Least significant bit is A1, and most significant bit is H8.
/// </summary>
public class BitBoard
{

    private Dictionary<int, ulong> _bitBoards { get; set; } = new Dictionary<int, ulong>();

    public static BitBoard FromDictionary(Dictionary<int, ulong> bitBoards)
    {
        var bitBoard = new BitBoard();
        bitBoard._bitBoards = bitBoards;
        return bitBoard;
    }

    public ulong this[Colour colour, PieceType pieceType]
    {
        get => _bitBoards[(byte)colour << 3 | (byte)pieceType];
        set => _bitBoards[(byte)colour << 3 | (byte)pieceType] = value;
    }

    public ulong this[Colour colour]
    {
        get
        {
            var pawn  = colour == Colour.White ? PieceType.WPawn  : PieceType.BPawn;
            return this[colour, pawn]
                |  this[colour, PieceType.Knight]
                |  this[colour, PieceType.Bishop]
                |  this[colour, PieceType.Rook]
                |  this[colour, PieceType.Queen]
                |  this[colour, PieceType.King];
        }
    }

    public ulong this[PieceType pieceType]
    {
        get
        {
            if (pieceType == PieceType.WPawn || pieceType == PieceType.BPawn)
            {
                return this[Colour.White, PieceType.WPawn] | this[Colour.Black, PieceType.BPawn];
            }
            return this[Colour.White, pieceType] | this[Colour.Black, pieceType];
        }
    }

    public ulong Occupied => this[Colour.White] | this[Colour.Black];
    public ulong Empty => ~Occupied;

    public override bool Equals(object obj) => obj is BitBoard other &&
        this[Colour.White, PieceType.WPawn]  == other[Colour.White, PieceType.WPawn]  &&
        this[Colour.White, PieceType.Knight] == other[Colour.White, PieceType.Knight] &&
        this[Colour.White, PieceType.Bishop] == other[Colour.White, PieceType.Bishop] &&
        this[Colour.White, PieceType.Rook]   == other[Colour.White, PieceType.Rook]   &&
        this[Colour.White, PieceType.Queen]  == other[Colour.White, PieceType.Queen]  &&
        this[Colour.White, PieceType.King]   == other[Colour.White, PieceType.King]   &&
        this[Colour.Black, PieceType.BPawn]  == other[Colour.Black, PieceType.BPawn]  &&
        this[Colour.Black, PieceType.Knight] == other[Colour.Black, PieceType.Knight] &&
        this[Colour.Black, PieceType.Bishop] == other[Colour.Black, PieceType.Bishop] &&
        this[Colour.Black, PieceType.Rook]   == other[Colour.Black, PieceType.Rook]   &&
        this[Colour.Black, PieceType.Queen]  == other[Colour.Black, PieceType.Queen]  &&
        this[Colour.Black, PieceType.King]   == other[Colour.Black, PieceType.King];

    public override int GetHashCode() => HashCode.Combine(_bitBoards);
}

