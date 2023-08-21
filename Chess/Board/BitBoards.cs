using Chess.Generics;
using ConsoleExtensions;

namespace Chess.Board;

/// <summary>
/// Represents the state of all the pieces on the board.
/// Each piece type is represented by a ulong, where each bit represents a square.
/// A 1 means that a piece of that type is on that square, and a 0 means that it is not.
/// Least significant bit is A1, and most significant bit is H8.
/// </summary>
public class BitBoard
{
    private Dictionary<int, ulong> _bitBoards { get; set; }
    private Dictionary<int, Func<C, ulong?, ulong>> _pCodeMoveFunctionMap;

    public ulong Occupied => this[C.White] | this[C.Black];
    public ulong Empty => ~Occupied;

    public BitBoard()
    {
        _bitBoards = new Dictionary<int, ulong>();
        _pCodeMoveFunctionMap = new Dictionary<int, Func<C, ulong?, ulong>>()
        {
            [(int)PType.WPawn]  = PawnPossibleMoves,
            [(int)PType.BPawn]  = PawnPossibleMoves,
            [(int)PType.Knight] = KnightAttacks,
            [(int)PType.Bishop] = BishopAttacks,
            [(int)PType.Rook]   = RookAttacks,
            [(int)PType.Queen]  = QueenAttacks,
            [(int)PType.King]   = KingAttacks
        };
    }

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

    public void MakeMove(Move move, Piece pieceFrom, Piece pieceTo)
    {
        this[pieceFrom.Colour, pieceFrom.Type] ^= move.From.BitMask();
        this[pieceFrom.Colour, pieceFrom.Type] |= move.To.BitMask();
        if (pieceTo.Type != PType.None)
        {
            this[pieceTo.Colour, pieceTo.Type] = this[pieceTo.Colour, pieceTo.Type] ^= move.To.BitMask();
        }
    }

    public bool IsValidMoveForPiece(Move move, Piece pieceFrom)
    {
        if ((this[pieceFrom.Colour, pieceFrom.Type] & move.From.BitMask()) == 0)
        {
            throw new ArgumentException($"{pieceFrom} not on 'From' square {move.From}");
        }
        var bitBoard = this[pieceFrom.Colour, pieceFrom.Type] & move.From.BitMask();
        var moveFunc = _pCodeMoveFunctionMap[(int)pieceFrom.Type];
        return (moveFunc(pieceFrom.Colour, bitBoard) & move.To.BitMask()) != 0;
    }

    public bool IsInCheck(C colour)
    {
        var kingBitMask = this[colour, PType.King];
        var kingSquare = (Square)(int)Math.Log2(kingBitMask);
        return SquareIsAttackedBy(kingSquare, colour.Opposite());
    }
    internal bool SquareIsAttackedBy(Square square, C colour) => (Attacks(colour) & square.BitMask()) != 0;

    private ulong Attacks(C colour)
    {
        var pawnAttacks = colour == C.White ? WPawnAttacks() : BPawnAttacks();
        return pawnAttacks | KnightAttacks(colour) | BishopAttacks(colour) |
            RookAttacks(colour) | QueenAttacks(colour) | KingAttacks(colour);
    }

    private ulong WPawnsAbleToSinglePush() => SoutOne(Empty) & this[C.White, PType.WPawn];
    private ulong WPawnsAbleToDoublePush()
    {
        var emptyR3SquaresWithEmptyR4SquaresAhead = SoutOne(Empty & (ulong)Ranks.R4) & Empty;
        return SoutOne(emptyR3SquaresWithEmptyR4SquaresAhead) & this[C.White, PType.WPawn];
    }
    private ulong BPawnsAbleToSinglePush() => NortOne(Empty) & this[C.Black, PType.BPawn];
    private ulong BPawnsAbleToDoublePush()
    {
        var emptyR6SquaresWithEmptyR5SquaresAhead = NortOne(Empty & (ulong)Ranks.R5) & Empty;
        return NortOne(emptyR6SquaresWithEmptyR5SquaresAhead) & this[C.Black, PType.BPawn];
    }

    private ulong WPawnSinglePushTargets(ulong? bitBoard = null)
    {
        var wPawn = bitBoard ?? this[C.White, PType.WPawn];
        return NortOne(wPawn) & Empty;
    }

    private ulong WPawnDoublePushTargets(ulong? bitBoard = null)
    {
        var wPawn = bitBoard ?? this[C.White, PType.WPawn];
        return NortOne(WPawnSinglePushTargets(wPawn)) & Empty & (ulong)Ranks.R4;
    }

    private ulong WPawnAttacks(ulong? bitBoard = null)
    {
        var wPawn = bitBoard ?? this[C.White, PType.WPawn];
        return NoEaOne(wPawn) | noWeOne(wPawn);
    }

    private ulong BPawnSinglePushTargets(ulong? bitBoard = null)
    {
        var bPawn = bitBoard ?? this[C.Black, PType.BPawn];
        return SoutOne(bPawn) & Empty;
    }

    private ulong BPawnDoublePushTargets(ulong? bitBoard = null)
    {
        var bPawn = bitBoard ?? this[C.Black, PType.BPawn];
        return SoutOne(BPawnSinglePushTargets(bPawn)) & Empty & (ulong)Ranks.R5;
    }

    private ulong BPawnAttacks(ulong? bitBoard = null)
    {
        var bPawn = bitBoard ?? this[C.Black, PType.BPawn];
        return SoEaOne(bPawn) | SoWeOne(bPawn);
    }

    private ulong PawnPushTargets(C colour, ulong? bitBoard = null)
    {
        var pawn = bitBoard ?? this[colour, colour == C.White ? PType.WPawn : PType.BPawn];
        return (colour == C.White ? WPawnSinglePushTargets(pawn) : BPawnSinglePushTargets(pawn))
            |  (colour == C.White ? WPawnDoublePushTargets(pawn) : BPawnDoublePushTargets(pawn));
    }

    private ulong PawnAttacks(C colour, ulong? bitBoard = null)
    {
        var pawn = bitBoard ?? this[colour, colour == C.White ? PType.WPawn : PType.BPawn];
        return colour == C.White ? WPawnAttacks(pawn) : BPawnAttacks(pawn);
    }

    private ulong PawnPossibleMoves(C colour, ulong? bitBoard = null)
    {
        var pawn = bitBoard ?? this[colour, colour == C.White ? PType.WPawn : PType.BPawn];
        return (PawnAttacks(colour, pawn) & this[colour.Opposite()]) | PawnPushTargets(colour, pawn);
    }

    private ulong KnightAttacks(C colour, ulong? bitBoard = null)
    {
        var knight = bitBoard ?? this[colour, PType.Knight];
        return NoNoEa(knight) | NoEaEa(knight) | SoEaEa(knight) | SoSoEa(knight) |
            SoSoWe(knight) | SoWeWe(knight) | NoWeWe(knight) | NoNoWe(knight);
    }

    private ulong KingAttacks(C colour, ulong? bitBoard = null)
    {
        var king = bitBoard ?? this[colour, PType.King];
        var laterals = EastOne(king) | WestOne(king);
        var threeSqMask = laterals | king;
        return NortOne(threeSqMask) | SoutOne(threeSqMask) | laterals;
    }

    private ulong RookAttacks(C colour, ulong? bitBoard = null)
    {
        var rook = bitBoard ?? this[colour, PType.Rook];
        return RayAttacks(rook, NortOne) | RayAttacks(rook, SoutOne) |
            RayAttacks(rook, EastOne) | RayAttacks(rook, WestOne);
    }

    private ulong BishopAttacks(C colour, ulong? bitBoard = null)
    {
        var bishop = bitBoard ?? this[colour, PType.Bishop];
        return RayAttacks(bishop, NoEaOne) | RayAttacks(bishop, SoEaOne) |
            RayAttacks(bishop, SoWeOne) | RayAttacks(bishop, noWeOne);
    }

    private ulong QueenAttacks(C colour, ulong? bitBoard = null)
    {
        var queen = bitBoard ?? this[colour, PType.Queen];
        return RayAttacks(queen, NortOne) | RayAttacks(queen, SoutOne) |
            RayAttacks(queen, EastOne) | RayAttacks(queen, WestOne) |
            RayAttacks(queen, NoEaOne) | RayAttacks(queen, SoEaOne) |
            RayAttacks(queen, SoWeOne) | RayAttacks(queen, noWeOne);
    }

    private ulong RayAttacks(ulong bitBoard, Func<ulong, ulong> DirectionOneStep) =>
        DirectionOneStep(Dumb7Fill(bitBoard, DirectionOneStep));

    private ulong Dumb7Fill(ulong bitBoard, Func<ulong, ulong> directionOneStep) =>
        Enumerable.Range(0, 7)
            .Aggregate(bitBoard, (current, _) => (Empty & directionOneStep(current)) | current);

    // noWe         nort         noEa
    //          +7   +8   +9
    //             \  |  /
    // west    -1 <-  0 -> +1    east
    //             /  |  \
    //          -9   -8   -7
    // soWe         sout         soEa

    private ulong NortOne(ulong bitBoard) => bitBoard << 8;
    private ulong SoutOne(ulong bitBoard) => bitBoard >> 8;
    private ulong EastOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.H) << 1; // H file can't move east
    private ulong NoEaOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.H) << 9; // H file can't move east
    private ulong SoEaOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.H) >> 7; // H file can't move east
    private ulong WestOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.A) >> 1; // A file can't move west
    private ulong SoWeOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.A) >> 9; // A file can't move west
    private ulong noWeOne(ulong bitBoard) => (bitBoard & ~(ulong)Files.A) << 7; // A file can't move west

    private ulong NoNoEa(ulong bitBoard) => (bitBoard & ~(ulong)Files.H) << 17;
    private ulong NoEaEa(ulong bitBoard) => (bitBoard & ~(ulong)Files.G & ~(ulong)Files.H) << 10;
    private ulong SoEaEa(ulong bitBoard) => (bitBoard & ~(ulong)Files.G & ~(ulong)Files.H) >> 6;
    private ulong SoSoEa(ulong bitBoard) => (bitBoard & ~(ulong)Files.H) >> 15;
    private ulong SoSoWe(ulong bitBoard) => (bitBoard & ~(ulong)Files.A) >> 17;
    private ulong SoWeWe(ulong bitBoard) => (bitBoard & ~(ulong)Files.A & ~(ulong)Files.B) >> 10;
    private ulong NoWeWe(ulong bitBoard) => (bitBoard & ~(ulong)Files.A & ~(ulong)Files.B) << 6;
    private ulong NoNoWe(ulong bitBoard) => (bitBoard & ~(ulong)Files.A) << 15;

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
