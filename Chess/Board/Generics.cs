namespace Chess.Generics;

[Flags]
public enum C : byte
{
    White = 0,
    Black = 1
}

public static class CExtensions
{
    public static C Opposite(this C color) => color == C.White ? C.Black : C.White;
}

[Flags]
public enum PType : byte
{
    None   = 0,
    WPawn  = 1,
    BPawn  = 2,
    Knight = 3,
    Bishop = 4,
    Rook   = 5,
    Queen  = 6,
    King   = 7
}

[Flags]
public enum Ranks : ulong
{
    R1 = 0x00000000000000FF,
    R2 = 0x000000000000FF00,
    R3 = 0x0000000000FF0000,
    R4 = 0x00000000FF000000,
    R5 = 0x000000FF00000000,
    R6 = 0x0000FF0000000000,
    R7 = 0x00FF000000000000,
    R8 = 0xFF00000000000000
}

public static class RanksExtensions
{
    public static int Index(this Ranks rank)
    {
        var r = (ulong)rank;
        var i = 0;
        while ((r & 1) == 0)
        {
            r >>= 1;
            i++;
        }
        return i / 8;
    }
}

[Flags]
public enum Files : ulong
{
    A    = 0x0101010101010101, // 00000001 x8
    B    = 0x0202020202020202, // 00000010 x8
    C    = 0x0404040404040404, // 00000100 x8
    D    = 0x0808080808080808, // 00001000 x8
    E    = 0x1010101010101010, // 00010000 x8
    F    = 0x2020202020202020, // 00100000 x8
    G    = 0x4040404040404040, // 01000000 x8
    H    = 0x8080808080808080  // 10000000 x8
}

public static class FilesExtensions
{
    public static int Index(this Files file)
    {
        var f = (ulong)file;
        var i = 0;
        while ((f & 1) == 0)
        {
            f >>= 1;
            i++;
        }
        return i;
    }
}

public enum Square
{
    A1, B1, C1, D1, E1, F1, G1, H1,
    A2, B2, C2, D2, E2, F2, G2, H2,
    A3, B3, C3, D3, E3, F3, G3, H3,
    A4, B4, C4, D4, E4, F4, G4, H4,
    A5, B5, C5, D5, E5, F5, G5, H5,
    A6, B6, C6, D6, E6, F6, G6, H6,
    A7, B7, C7, D7, E7, F7, G7, H7,
    A8, B8, C8, D8, E8, F8, G8, H8,
    None
}

public static class SquareExtensions
{
    public static ulong BitMask(this Square square) => 1UL << (int)square;
    public static Ranks Rank(this Square square)
    {
        var r = (int)square / 8;
        return (Ranks)(0xFFUL << (r * 8));
    }
    public static Files File(this Square square)
    {
        var f = (int)square % 8;
        return (Files)(0x0101010101010101UL << f);
    }
}

public enum ECastleRights
{
    None,
    KingSide,
    QueenSide,
    BothSides
}
