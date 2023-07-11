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

    internal static ColourBitBoard FromStartPosition(Colour colour) => colour switch
    {
        Colour.White => new ColourBitBoard()
        {
            Pawn    = 0x000000000000FF00,
            Knight  = 0x0000000000000042,
            Bishop  = 0x0000000000000024,
            Rook    = 0x0000000000000081,
            Queen   = 0x0000000000000008,
            King    = 0x0000000000000010,
        },
        Colour.Black => new ColourBitBoard()
        {
            Pawn    = 0x00FF000000000000,
            Knight  = 0x4200000000000000,
            Bishop  = 0x2400000000000000,
            Rook    = 0x8100000000000000,
            Queen   = 0x0800000000000000,
            King    = 0x1000000000000000,
        },
        _ => throw new ArgumentException("Invalid colour"),
    };
}
