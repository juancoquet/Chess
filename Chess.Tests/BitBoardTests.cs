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

    [Fact]
    public void TestWPawnAttacks()
    {
        var board = CreateOtherwiseEmptyBoard(P: new[] {Square.D5, Square.H2});
        board.SquareIsAttackedBy(Square.C6, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.D6, C.White).Should().BeFalse();
        board.SquareIsAttackedBy(Square.E6, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.G3, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.A3, C.White).Should().BeFalse();
        board.SquareIsAttackedBy(Square.A3, C.White).Should().BeFalse();
    }

    [Fact]
    public void TestBPawnAttacks()
    {
        var board = CreateOtherwiseEmptyBoard(p: new[] {Square.D5, Square.H2});
        board.SquareIsAttackedBy(Square.C4, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.D4, C.Black).Should().BeFalse();
        board.SquareIsAttackedBy(Square.E4, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.G1, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.H1, C.Black).Should().BeFalse();
        board.SquareIsAttackedBy(Square.A1, C.Black).Should().BeFalse();
        board.SquareIsAttackedBy(Square.A3, C.Black).Should().BeFalse();
    }

    [Fact]
    public void TestKnightAttacks()
    {
        var board = CreateOtherwiseEmptyBoard(N: new[] {Square.D5, Square.G6}, p: new[] {Square.E4});
        // Knight on D5
        board.SquareIsAttackedBy(Square.C7, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.E7, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.F6, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.F4, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.E3, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.C3, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.B4, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.B6, C.White).Should().BeTrue();
        // Knight on G6
        board.SquareIsAttackedBy(Square.F8, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.H8, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.H4, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.F4, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.E5, C.White).Should().BeTrue();

        board.SquareIsAttackedBy(Square.A6, C.White).Should().BeFalse();
        board.SquareIsAttackedBy(Square.A8, C.White).Should().BeFalse();
    }

    [Fact]
    public void TestBishopAttacks()
    {
        var board = CreateOtherwiseEmptyBoard(B: new[] {Square.D5}, b: new[] {Square.G2}, p: new[] {Square.F7});
        // Bishop on D5
        board.SquareIsAttackedBy(Square.C6, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.B7, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.A8, C.White).Should().BeTrue();

        board.SquareIsAttackedBy(Square.E6, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.F7, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.G8, C.White).Should().BeFalse();

        board.SquareIsAttackedBy(Square.E4, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.F3, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.G2, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.H1, C.White).Should().BeFalse();

        board.SquareIsAttackedBy(Square.C4, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.B3, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.A2, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.H8, C.White).Should().BeFalse();

        // Bishop on G2
        board.SquareIsAttackedBy(Square.F3, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.E4, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.D5, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.C6, C.Black).Should().BeFalse();

        board.SquareIsAttackedBy(Square.H3, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.A4, C.Black).Should().BeFalse();

        board.SquareIsAttackedBy(Square.H1, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.H2, C.Black).Should().BeFalse();

        board.SquareIsAttackedBy(Square.F1, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.F2, C.Black).Should().BeFalse();
    }

    [Fact]
    public void TestRookAttacks()
    {
        var board = CreateOtherwiseEmptyBoard(R: new[] {Square.D5}, r: new[] {Square.G2}, p: new[] {Square.G7});
        // Rook on D5
        board.SquareIsAttackedBy(Square.D5, C.White).Should().BeFalse();
        board.SquareIsAttackedBy(Square.E5, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.F5, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.G5, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.H5, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.A7, C.White).Should().BeFalse();

        board.SquareIsAttackedBy(Square.D6, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.D7, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.D8, C.White).Should().BeTrue();

        board.SquareIsAttackedBy(Square.D4, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.D3, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.D2, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.D1, C.White).Should().BeTrue();

        board.SquareIsAttackedBy(Square.C5, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.B5, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.A5, C.White).Should().BeTrue();
        board.SquareIsAttackedBy(Square.H4, C.White).Should().BeFalse();

        // Rook on G2
        board.SquareIsAttackedBy(Square.G2, C.Black).Should().BeFalse();
        board.SquareIsAttackedBy(Square.G3, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.G4, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.G5, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.G6, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.G7, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.G8, C.Black).Should().BeFalse();

        board.SquareIsAttackedBy(Square.H2, C.Black).Should().BeTrue();
        board.SquareIsAttackedBy(Square.A3, C.Black).Should().BeFalse();

        board.SquareIsAttackedBy(Square.G1, C.Black).Should().BeTrue();

        board.SquareIsAttackedBy(Square.F3, C.Black).Should().BeFalse();
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
