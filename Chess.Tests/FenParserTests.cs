using Chess.Fen;
using Chess.Generics;
using ConsoleExtensions;

namespace Chess.Tests;

public class FenParserTests
{
    private readonly FenParser _fenParser = new();

    [Fact]
    public void TestInvalidInputThrows()
    {
        Assert.Throws<ArgumentException>(() => _fenParser.Parse("abc"));
        Assert.Throws<ArgumentException>(
            () => _fenParser.Parse("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR")
        );
    }

    [Fact]
    public void TestParsePiecesSimple()
    {
        var pieces = _fenParser.ParsePieces("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
        Assert.Equal(64, pieces.Count());
        Assert.Equal(16, pieces.Count(piece => piece.Colour == Colour.White));
        Assert.Equal(16, pieces.Count(piece => piece.Colour == Colour.Black));
        Assert.Equal(32, pieces.Count(piece => piece.Type == PieceType.None));
        
        // ParsePieces reverses the fen input ranks so that the output is A1 to H8
        Assert.All(pieces.Take(16), piece => Assert.Equal(Colour.White, piece.Colour));
        Assert.All(pieces.Skip(48), piece => Assert.Equal(Colour.Black, piece.Colour));
        var firstPiece = pieces.ElementAt(3);
        var lastPiece = pieces.ElementAt(59);
        Assert.Equal((Colour.White, PieceType.Queen), (firstPiece.Colour, firstPiece.Type));
        Assert.Equal((Colour.Black, PieceType.Queen), (lastPiece.Colour, lastPiece.Type));

        Assert.Equal(
            8,
            pieces.Count(piece => piece.Colour == Colour.White && piece.Type == PieceType.Pawn)
        );
        Assert.Equal(
            8,
            pieces.Count(piece => piece.Colour == Colour.Black && piece.Type == PieceType.Pawn)
        );
        Assert.Equal(
            2,
            pieces.Count(piece => piece.Colour == Colour.White && piece.Type == PieceType.Knight)
        );
        Assert.Equal(
            2,
            pieces.Count(piece => piece.Colour == Colour.Black && piece.Type == PieceType.Knight)
        );
        Assert.Equal(
            2,
            pieces.Count(piece => piece.Colour == Colour.White && piece.Type == PieceType.Bishop)
        );
        Assert.Equal(
            2,
            pieces.Count(piece => piece.Colour == Colour.Black && piece.Type == PieceType.Bishop)
        );
        Assert.Equal(
            2,
            pieces.Count(piece => piece.Colour == Colour.White && piece.Type == PieceType.Rook)
        );
        Assert.Equal(
            2,
            pieces.Count(piece => piece.Colour == Colour.Black && piece.Type == PieceType.Rook)
        );
        Assert.Equal(
            1,
            pieces.Count(piece => piece.Colour == Colour.White && piece.Type == PieceType.Queen)
        );
        Assert.Equal(
            1,
            pieces.Count(piece => piece.Colour == Colour.Black && piece.Type == PieceType.Queen)
        );
        Assert.Equal(
            1,
            pieces.Count(piece => piece.Colour == Colour.White && piece.Type == PieceType.King)
        );
        Assert.Equal(
            1,
            pieces.Count(piece => piece.Colour == Colour.Black && piece.Type == PieceType.King)
        );
    }

    [Fact]
    public void TestParsePiecesComplex()
    {
        var pieces = _fenParser.ParsePieces("r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1");
        Assert.Equal(64, pieces.Count());
        Assert.Equal(10, pieces.Count(piece => piece.Colour == Colour.White));
        Assert.Equal(13, pieces.Count(piece => piece.Colour == Colour.Black));

        var ranks = pieces.Batch(8);

        var expected0 = new[]
        {   // q5b1
            (Colour.Black, PieceType.Queen),     // q
            (Colour.None,  PieceType.None),      // 5
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.Black, PieceType.Bishop),   // b
            (Colour.None,  PieceType.None)      // 1
        };
        var expected1 = new[]
        {   // P1P1K3
            (Colour.White, PieceType.Pawn),     // P
            (Colour.None,  PieceType.None),     // 1
            (Colour.White, PieceType.Pawn),     // P
            (Colour.None,  PieceType.None),     // 1
            (Colour.White, PieceType.King),     // K
            (Colour.None,  PieceType.None),     // 3
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None)
        };
        var expected2 = new[]
        {   // 3P4
            (Colour.None,  PieceType.None),     // 3
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.White, PieceType.Pawn),     // P
            (Colour.None,  PieceType.None),     // 4
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None)
        };
        var expected3 = new[]
        {   // 6P1
            (Colour.None,  PieceType.None),     // 6
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.White, PieceType.Pawn),     // P
            (Colour.None,  PieceType.None)      // 1
        };
        var expected4 = new[]
        {   // 1p1NP2P
            (Colour.None,  PieceType.None),     // 1
            (Colour.Black, PieceType.Pawn),     // p
            (Colour.None,  PieceType.None),     // 1
            (Colour.White, PieceType.Knight),   // N
            (Colour.White, PieceType.Pawn),     // P
            (Colour.None,  PieceType.None),     // 2
            (Colour.None,  PieceType.None),
            (Colour.White, PieceType.Pawn),     // P
        };
        var expected5 = new[]
        {   // n4n2
            (Colour.Black, PieceType.Knight),   // n
            (Colour.None,  PieceType.None),     // 4
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.Black, PieceType.Knight),   // n
            (Colour.None,  PieceType.None),     // 2
            (Colour.None,  PieceType.None)
        };
        var expected6 = new[]
        {   // p2pBpNp
            (Colour.Black, PieceType.Pawn),     // p
            (Colour.None,  PieceType.None),     // 2
            (Colour.None,  PieceType.None),
            (Colour.Black, PieceType.Pawn),     // p
            (Colour.White, PieceType.Bishop),   // B
            (Colour.Black, PieceType.Pawn),     // p
            (Colour.White, PieceType.Knight),   // N
            (Colour.Black, PieceType.Pawn),     // p
        };
        var expected7 = new[]
        {   // r1bk3r
            (Colour.Black, PieceType.Rook),     // r
            (Colour.None,  PieceType.None),     // 1
            (Colour.Black, PieceType.Bishop),   // b
            (Colour.Black, PieceType.King),     // k
            (Colour.None,  PieceType.None),     // 3
            (Colour.None,  PieceType.None),
            (Colour.None,  PieceType.None),
            (Colour.Black, PieceType.Rook),     // r
        };
        Assert.Equal(expected0, ranks.ElementAt(0).Select(piece => (piece.Colour, piece.Type)));
        Assert.Equal(expected1, ranks.ElementAt(1).Select(piece => (piece.Colour, piece.Type)));
        Assert.Equal(expected2, ranks.ElementAt(2).Select(piece => (piece.Colour, piece.Type)));
        Assert.Equal(expected3, ranks.ElementAt(3).Select(piece => (piece.Colour, piece.Type)));
        Assert.Equal(expected4, ranks.ElementAt(4).Select(piece => (piece.Colour, piece.Type)));
        Assert.Equal(expected5, ranks.ElementAt(5).Select(piece => (piece.Colour, piece.Type)));
        Assert.Equal(expected6, ranks.ElementAt(6).Select(piece => (piece.Colour, piece.Type)));
        Assert.Equal(expected7, ranks.ElementAt(7).Select(piece => (piece.Colour, piece.Type)));
    }


    [Fact]
    public void TestParsePiecesInvalidInputThrows()
    {
        Assert.Throws<ArgumentException>(() => _fenParser.ParsePieces("abc"));
        Assert.Throws<ArgumentException>(() => _fenParser.ParsePieces("rnbqkbnr/pppppppp"));
    }

    [Fact]
    public void TestParseCastleRightsSimple()
    {
        var noCastleRights = _fenParser.ParseCastleRights("-");
        noCastleRights.White.Should().Be(ECastleRights.None);
        noCastleRights.Black.Should().Be(ECastleRights.None);

        var whiteQueenSide = _fenParser.ParseCastleRights("Q");
        whiteQueenSide.White.Should().Be(ECastleRights.QueenSide);
        whiteQueenSide.Black.Should().Be(ECastleRights.None);

        var blackKingSide = _fenParser.ParseCastleRights("k");
        blackKingSide.White.Should().Be(ECastleRights.None);
        blackKingSide.Black.Should().Be(ECastleRights.KingSide);
    }

    [Fact]
    public void TestParseCastleRightsComplex()
    {
        var allRights = _fenParser.ParseCastleRights("KQkq");
        allRights.White.Should().Be(ECastleRights.BothSides);
        allRights.Black.Should().Be(ECastleRights.BothSides);

        var bothAndNone = _fenParser.ParseCastleRights("KQ");
        bothAndNone.White.Should().Be(ECastleRights.BothSides);
        bothAndNone.Black.Should().Be(ECastleRights.None);

        var bothAndKingSide = _fenParser.ParseCastleRights("KQk");
        bothAndKingSide.White.Should().Be(ECastleRights.BothSides);
        bothAndKingSide.Black.Should().Be(ECastleRights.KingSide);

        var bothAndQueenSide = _fenParser.ParseCastleRights("KQq");
        bothAndQueenSide.White.Should().Be(ECastleRights.BothSides);
        bothAndQueenSide.Black.Should().Be(ECastleRights.QueenSide);

        var noneAndBoth = _fenParser.ParseCastleRights("kq");
        noneAndBoth.White.Should().Be(ECastleRights.None);
        noneAndBoth.Black.Should().Be(ECastleRights.BothSides);

        var KingSideAndBoth = _fenParser.ParseCastleRights("Kkq");
        KingSideAndBoth.White.Should().Be(ECastleRights.KingSide);
        KingSideAndBoth.Black.Should().Be(ECastleRights.BothSides);

        var QueenSideAndBoth = _fenParser.ParseCastleRights("Qkq");
        QueenSideAndBoth.White.Should().Be(ECastleRights.QueenSide);
        QueenSideAndBoth.Black.Should().Be(ECastleRights.BothSides);
    }

    [Fact]
    public void TestCastleRightsTokenOrderIrrelevant()
    {
        var allRights = _fenParser.ParseCastleRights("KqQk");
        allRights.White.Should().Be(ECastleRights.BothSides);
        allRights.Black.Should().Be(ECastleRights.BothSides);

        var bothAndNone = _fenParser.ParseCastleRights("QK");
        bothAndNone.White.Should().Be(ECastleRights.BothSides);
        bothAndNone.Black.Should().Be(ECastleRights.None);

        var bothAndKingSide = _fenParser.ParseCastleRights("kQK");
        bothAndKingSide.White.Should().Be(ECastleRights.BothSides);
        bothAndKingSide.Black.Should().Be(ECastleRights.KingSide);

        var bothAndQueenSide = _fenParser.ParseCastleRights("KqQ");
        bothAndQueenSide.White.Should().Be(ECastleRights.BothSides);
        bothAndQueenSide.Black.Should().Be(ECastleRights.QueenSide);

        var noneAndBoth = _fenParser.ParseCastleRights("qk");
        noneAndBoth.White.Should().Be(ECastleRights.None);
        noneAndBoth.Black.Should().Be(ECastleRights.BothSides);

        var KingSideAndBoth = _fenParser.ParseCastleRights("qKk");
        KingSideAndBoth.White.Should().Be(ECastleRights.KingSide);
        KingSideAndBoth.Black.Should().Be(ECastleRights.BothSides);

        var QueenSideAndBoth = _fenParser.ParseCastleRights("qkQ");
        QueenSideAndBoth.White.Should().Be(ECastleRights.QueenSide);
        QueenSideAndBoth.Black.Should().Be(ECastleRights.BothSides);
    }

    [Fact]
    public void TestInvalidCatleRightsFenThrows()
    {
        _fenParser.Invoking(parser => parser.ParseCastleRights("abc"))
            .Should().Throw<ArgumentException>()
            .WithMessage("Invalid character(s) castle rights FEN string: abc");

        _fenParser.Invoking(parser => parser.ParseCastleRights("KQkqKq"))
            .Should().Throw<ArgumentException>()
            .WithMessage("FEN castle rights string must be between 1 and 4 characters long");
    }

    [Fact]
    public void TestParseEnPassantSquare()
    {
        var noEnPassant = _fenParser.ParseEnPassantSquare("-");
        noEnPassant.Should().Be(Square.None);

        var a1 = _fenParser.ParseEnPassantSquare("a1");
        a1.Should().Be(Square.A1);

        var c4 = _fenParser.ParseEnPassantSquare("C4");
        c4.Should().Be(Square.C4);

        var h8 = _fenParser.ParseEnPassantSquare("h8");
        h8.Should().Be(Square.H8);
    }

    [Fact]
    public void TestParseEnPassantInvalidThrows()
    {
        _fenParser.Invoking(parser => parser.ParseEnPassantSquare("a9"))
            .Should().Throw<ArgumentException>()
            .WithMessage("FEN string for en passant target is not a valid square: a9");

        _fenParser.Invoking(parser => parser.ParseEnPassantSquare(""))
            .Should().Throw<ArgumentException>()
            .WithMessage("FEN en passant target square string must be 2 character long");

        _fenParser.Invoking(parser => parser.ParseEnPassantSquare("abc"))
            .Should().Throw<ArgumentException>()
            .WithMessage("FEN en passant target square string must be 2 character long");

        _fenParser.Invoking(parser => parser.ParseEnPassantSquare("a"))
            .Should().Throw<ArgumentException>()
            .WithMessage("FEN en passant target square string must be 2 character long");
    }
}
