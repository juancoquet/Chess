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
    public ColourBitBoard White { get; set; }
    public ColourBitBoard Black { get; set; }

    public ulong Occupied => White.All | Black.All;
    public ulong Empty => ~Occupied;

    public BitBoard(ColourBitBoard white, ColourBitBoard black)
    {
        White = white;
        Black = black;
    }

    public override bool Equals(object obj) => obj is BitBoard other &&
        EqualityComparer<ColourBitBoard>.Default.Equals(White, other.White) &&
        EqualityComparer<ColourBitBoard>.Default.Equals(Black, other.Black);

    public override int GetHashCode() => HashCode.Combine(White, Black);
}

public class ColourBitBoard
{
    public ulong Pawn   { get; set; }
    public ulong Knight { get; set; }
    public ulong Bishop { get; set; }
    public ulong Rook   { get; set; }
    public ulong Queen  { get; set; }
    public ulong King   { get; set; }

    public ulong All => Pawn | Knight | Bishop | Rook | Queen | King;

    public override bool Equals(object obj) => obj is ColourBitBoard other &&
        Pawn == other.Pawn &&
        Knight == other.Knight &&
        Bishop == other.Bishop &&
        Rook == other.Rook &&
        Queen == other.Queen &&
        King == other.King;

    public override int GetHashCode() => HashCode.Combine(Pawn, Knight, Bishop, Rook, Queen, King);
}
