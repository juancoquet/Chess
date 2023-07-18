using Chess.Board;
using Chess.Generics;

public class BitBoardTests
{
    private static BitBoard CreateOtherwiseEmptyBoard(
        Square[] p = null,
        Square[] P = null,
        Square[] n = null,
        Square[] N = null,
        Square[] b = null,
        Square[] B = null,
        Square[] r = null,
        Square[] R = null,
        Square[] q = null,
        Square[] Q = null,
        Square[] k = null,
        Square[] K = null
    )
    {
        var bPawn = p.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var wPawn = P.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var bKnight = n.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var wKnight = N.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var bBishop = b.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var wBishop = B.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var bRook = r.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var wRook = R.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var bQueen = q.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var wQueen = Q.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var bKing = k.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);
        var wKing = K.SomeNotNull().Map(
            squares =>
                squares.Select(sq => sq.BitMask())
                .Aggregate((a, b) => a | b)
        ).ValueOr(0UL);

        return BitBoard.FromDictionary(new Dictionary<int, ulong>
            {
                { PieceCode(C.White, PType.BPawn),  bPawn   },
                { PieceCode(C.Black, PType.WPawn),  wPawn   },
                { PieceCode(C.White, PType.Knight), wKnight },
                { PieceCode(C.Black, PType.Knight), bKnight },
                { PieceCode(C.White, PType.Bishop), wBishop },
                { PieceCode(C.Black, PType.Bishop), bBishop },
                { PieceCode(C.White, PType.Rook),   wRook   },
                { PieceCode(C.Black, PType.Rook),   bRook   },
                { PieceCode(C.White, PType.Queen),  wQueen  },
                { PieceCode(C.Black, PType.Queen),  bQueen  },
                { PieceCode(C.White, PType.King),   wKing   },
                { PieceCode(C.Black, PType.King),   bKing   },
            }
        );
    }

    private static int PieceCode(C colour, PType pieceType) => (byte)colour << 3 | (byte)pieceType;
}
