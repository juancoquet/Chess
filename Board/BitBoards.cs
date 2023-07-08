using Generics;

namespace Board;

/// <summary>
/// Represents the state of all the pieces on the board.
/// Each piece type is represented by a ulong, where each bit represents a square.
/// A 1 means that a piece of that type is on that square, and a 0 means that it is not.
/// </summary>
public class BitBoard
{
    public ColourBitBoard White { get; set; }
    public ColourBitBoard Black { get; set; }

    public BitBoard()
    {
        White = ColourBitBoard.FromColour(Colour.White);
        Black = ColourBitBoard.FromColour(Colour.Black);
    }

    public ulong OfColour(Colour colour) => colour switch
    {
        Colour.White => White.Pawn | White.Knight | White.Bishop | White.Rook | White.Queen | White.King,
        Colour.Black => Black.Pawn | Black.Knight | Black.Bishop | Black.Rook | Black.Queen | Black.King,
        _ => throw new ArgumentException("Invalid colour"),
    };

}

public class ColourBitBoard
{
    public ulong Pawn   { get; set; }
    public ulong Knight { get; set; }
    public ulong Bishop { get; set; }
    public ulong Rook   { get; set; }
    public ulong Queen  { get; set; }
    public ulong King   { get; set; }

    public static ColourBitBoard FromColour(Colour colour) => colour switch
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
