using Chess.Board;
using Chess.Generics;

public class BitBoardTests
{
    [Fact]
    public void TestSquareAttackedBy()
    {
        var board = CreateOtherwiseEmptyBoard(P: new[] {Square.A1});
        board.SquareIsAttackedBy(Square.B2, C.White).Should().BeTrue();

        board = CreateOtherwiseEmptyBoard(p: new[] {Square.E4});
        board.SquareIsAttackedBy(Square.D3, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.F3, C.Black).Should().BeTrue();

        board = CreateOtherwiseEmptyBoard(r: new[] {Square.F2}, Q: new[] {Square.D2}, K: new[] {Square.F7});
        board.SquareIsAttackedBy(Square.D2, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.C2, C.Black).Should().BeFalse();
        board.SquareIsAttackedBy(Square.F7, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.F6, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.F8, C.Black).Should().BeFalse();
        board.SquareIsAttackedBy(Square.F2, C.White).Should().BeTrue();
    }

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
                { PieceCode(C.White, PType.WPawn),  wPawn   },
                { PieceCode(C.Black, PType.BPawn),  bPawn   },
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
