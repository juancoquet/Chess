namespace Board;

/// <summary>
/// A bitboard is a 64-bit integer (ulong) that represents the state
/// of a piece type on the  chess board. The least significant bit (LSB)
/// is the A1 square, and the most significant bit (MSB) is the H8 square.
/// </summary>
public interface IBitBoard
{
    public ulong PawnWhite   { get; set; }
    public ulong PawnBlack   { get; set; }
    public ulong KnightWhite { get; set; }
    public ulong KnightBlack { get; set; }
    public ulong BishopWhite { get; set; }
    public ulong BishopBlack { get; set; }
    public ulong RookWhite   { get; set; }
    public ulong RookBlack   { get; set; }
    public ulong QueenWhite  { get; set; }
    public ulong QueenBlack  { get; set; }
    public ulong KingWhite   { get; set; }
    public ulong KingBlack   { get; set; }
}

public class BitBoard
{
    public ulong PawnWhite   { get; set; }
    public ulong PawnBlack   { get; set; }
    public ulong KnightWhite { get; set; }
    public ulong KnightBlack { get; set; }
    public ulong BishopWhite { get; set; }
    public ulong BishopBlack { get; set; }
    public ulong RookWhite   { get; set; }
    public ulong RookBlack   { get; set; }
    public ulong QueenWhite  { get; set; }
    public ulong QueenBlack  { get; set; }
    public ulong KingWhite   { get; set; }
    public ulong KingBlack   { get; set; }

    public BitBoard()
    {
        // using hexadecimal for conciseness
        PawnWhite   = 0x000000000000FF00;
        PawnBlack   = 0x00FF000000000000;
        KnightWhite = 0x0000000000000042;
        KnightBlack = 0x4200000000000000;
        BishopWhite = 0x0000000000000024;
        BishopBlack = 0x2400000000000000;
        RookWhite   = 0x0000000000000081;
        RookBlack   = 0x8100000000000000;
        QueenWhite  = 0x0000000000000008;
        QueenBlack  = 0x0800000000000000;
        KingWhite   = 0x0000000000000010;
        KingBlack   = 0x1000000000000000;
    }
}
