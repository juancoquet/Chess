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

    public ulong Occupied => this[C.White] | this[C.Black];
    public ulong Empty => ~Occupied;

    public static BitBoard FromDictionary(Dictionary<int, ulong> bitBoards)
    {
        var bitBoard = new BitBoard();
        bitBoard._bitBoards = bitBoards;
        return bitBoard;
    }

    public ulong this[C colour, PType pieceType]
    {
        get => _bitBoards[(byte)colour << 3 | (byte)pieceType];
        set => _bitBoards[(byte)colour << 3 | (byte)pieceType] = value;
    }

    public ulong this[C colour]
    {
        get
        {
            var pawn  = colour == C.White ? PType.WPawn  : PType.BPawn;
            return this[colour, pawn]
                |  this[colour, PType.Knight]
                |  this[colour, PType.Bishop]
                |  this[colour, PType.Rook]
                |  this[colour, PType.Queen]
                |  this[colour, PType.King];
        }
    }

    public ulong this[PType pieceType]
    {
        get
        {
            if (pieceType == PType.WPawn || pieceType == PType.BPawn)
            {
                return this[C.White, PType.WPawn] | this[C.Black, PType.BPawn];
            }
            return this[C.White, pieceType] | this[C.Black, pieceType];
        }
    }

    // noWe         nort         noEa
    //          +7   +8   +9
    //             \  |  /
    // west    -1 <-  0 -> +1    east
    //             /  |  \
    //          -9   -8   -7
    // soWe         sout         soEa

    private ulong nortOne(ulong bitBoard) => bitBoard << 8;
    private ulong soutOne(ulong bitBoard) => bitBoard >> 8;
    private ulong eastOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.H) << 1; // H file can't move east
    private ulong noEaOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.H) << 9; // H file can't move east
    private ulong soEaOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.H) >> 7; // H file can't move east
    private ulong westOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.A) >> 1; // A file can't move west
    private ulong soWeOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.A) >> 9; // A file can't move west
    private ulong noWeOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.A) << 7; // A file can't move west

    private ulong noNoEa(ulong bitBoard) => (bitBoard & ~(ulong)Files.H) << 17;
    private ulong noEaEa(ulong bitBoard) => (bitBoard & ~(ulong)Files.G & ~(ulong)Files.H) << 10;
    private ulong soEaEa(ulong bitBoard) => (bitBoard & ~(ulong)Files.G & ~(ulong)Files.H) >> 6;
    private ulong soSoEa(ulong bitBoard) => (bitBoard & ~(ulong)Files.H) >> 15;
    private ulong soSoWe(ulong bitBoard) => (bitBoard & ~(ulong)Files.A) >> 17;
    private ulong soWeWe(ulong bitBoard) => (bitBoard & ~(ulong)Files.A & ~(ulong)Files.B) >> 10;
    private ulong noWeWe(ulong bitBoard) => (bitBoard & ~(ulong)Files.A & ~(ulong)Files.B) << 6;
    private ulong noNoWe(ulong bitBoard) => (bitBoard & ~(ulong)Files.A) << 15;

    private ulong wPawnSinglePushTargets() => nortOne(this[C.White, PType.WPawn]) & Empty;
    private ulong wPawnDoublePushTargets() => nortOne(wPawnSinglePushTargets()) & Empty & (ulong)Ranks.R4;
    private ulong wPawnsAbleToSinglePush() => soutOne(Empty) & this[C.White, PType.WPawn];
    private ulong wPawnsAbleToDoublePush()
    {
        var emptyR3SquaresWithEmptyR4SquaresAhead = soutOne(Empty & (ulong)Ranks.R4) & Empty;
        return soutOne(emptyR3SquaresWithEmptyR4SquaresAhead) & this[C.White, PType.WPawn];
    }
    private ulong wPawnAttacks() => noEaOne(this[C.White, PType.WPawn]) | noWeOne(this[C.White, PType.WPawn]);

    private ulong bPawnSinglePushTargets() => soutOne(this[C.Black, PType.BPawn]) & Empty;
    private ulong bPawnDoublePushTargets() => soutOne(bPawnSinglePushTargets()) & Empty & (ulong)Ranks.R5;
    private ulong bPawnsAbleToSinglePush() => nortOne(Empty) & this[C.Black, PType.BPawn];
    private ulong bPawnsAbleToDoublePush()
    {
        var emptyR6SquaresWithEmptyR5SquaresAhead = nortOne(Empty & (ulong)Ranks.R5) & Empty;
        return nortOne(emptyR6SquaresWithEmptyR5SquaresAhead) & this[C.Black, PType.BPawn];
    }
    private ulong bPawnAttacks() => soEaOne(this[C.Black, PType.BPawn]) | soWeOne(this[C.Black, PType.BPawn]);

    private ulong knightAttacks(C colour) =>
        noNoEa(this[colour, PType.Knight]) | noEaEa(this[colour, PType.Knight]) |
        soEaEa(this[colour, PType.Knight]) | soSoEa(this[colour, PType.Knight]) |
        soSoWe(this[colour, PType.Knight]) | soWeWe(this[colour, PType.Knight]) |
        noWeWe(this[colour, PType.Knight]) | noNoWe(this[colour, PType.Knight]);

    public override bool Equals(object obj) => obj is BitBoard other &&
        this[C.White, PType.WPawn]  == other[C.White, PType.WPawn]  &&
        this[C.White, PType.Knight] == other[C.White, PType.Knight] &&
        this[C.White, PType.Bishop] == other[C.White, PType.Bishop] &&
        this[C.White, PType.Rook]   == other[C.White, PType.Rook]   &&
        this[C.White, PType.Queen]  == other[C.White, PType.Queen]  &&
        this[C.White, PType.King]   == other[C.White, PType.King]   &&
        this[C.Black, PType.BPawn]  == other[C.Black, PType.BPawn]  &&
        this[C.Black, PType.Knight] == other[C.Black, PType.Knight] &&
        this[C.Black, PType.Bishop] == other[C.Black, PType.Bishop] &&
        this[C.Black, PType.Rook]   == other[C.Black, PType.Rook]   &&
        this[C.Black, PType.Queen]  == other[C.Black, PType.Queen]  &&
        this[C.Black, PType.King]   == other[C.Black, PType.King];

    public override int GetHashCode() => _bitBoards.GetHashCode();
}
