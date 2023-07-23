using Chess.Board;
using Chess.Fen;
using Chess.Generics;

namespace Chess.Tests;

public class FenParserTests
{
    private readonly FenParser _fenParser = new();

    private static Piece[] _startPositionPieceArray = CreateStartPositionPieceArray();

    [Fact]
    public void TestParseStartPosition()
    {
        var board = _fenParser.Parse(FenParser.StartPosition);
        board.BitBoard.Should().Be(CreateStartPositionBitBoard());
        board.Squares.Should().Equal(_startPositionPieceArray.Select(p => p.PieceCode));
        board.Turn.Should().Be(C.White);
        board.MoveNumber.Should().Be(1);
        board.HalfMoveClock.Should().Be(0);
        board.EnPassantTarget.Should().Be(Square.None);
        board.CastleRights.White.Should().Be(ECastleRights.BothSides);
        board.CastleRights.Black.Should().Be(ECastleRights.BothSides);
    }

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
        var pieces = _fenParser.ParsePieces(FenParser.StartPosition.Split().First());
        pieces.Should().Equal(_startPositionPieceArray);
    }

    [Fact]
    public void TestParsePiecesComplex()
    {
        var pieces = _fenParser.ParsePieces("r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1");
        Assert.Equal(64, pieces.Count());

        var ranks = pieces.Batch(8);

        var expected0 = new[]
        {   // q5b1
            (C.Black,  PType.Queen),     // q
            (C.White,  PType.None),      // 5
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.Black,  PType.Bishop),   // b
            (C.White,  PType.None)      // 1
        };
        var expected1 = new[]
        {   // P1P1K3
            (C.White,  PType.WPawn),     // P
            (C.White,  PType.None),     // 1
            (C.White,  PType.WPawn),     // P
            (C.White,  PType.None),     // 1
            (C.White,  PType.King),     // K
            (C.White,  PType.None),     // 3
            (C.White,  PType.None),
            (C.White,  PType.None)
        };
        var expected2 = new[]
        {   // 3P4
            (C.White,  PType.None),     // 3
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.White,  PType.WPawn),     // P
            (C.White,  PType.None),     // 4
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.White,  PType.None)
        };
        var expected3 = new[]
        {   // 6P1
            (C.White,  PType.None),     // 6
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.White,  PType.WPawn),     // P
            (C.White,  PType.None)      // 1
        };
        var expected4 = new[]
        {   // 1p1NP2P
            (C.White,  PType.None),     // 1
            (C.Black,  PType.BPawn),     // p
            (C.White,  PType.None),     // 1
            (C.White, PType.Knight),   // N
            (C.White, PType.WPawn),     // P
            (C.White,  PType.None),     // 2
            (C.White,  PType.None),
            (C.White, PType.WPawn),     // P
        };
        var expected5 = new[]
        {   // n4n2
            (C.Black, PType.Knight),   // n
            (C.White,  PType.None),     // 4
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.Black, PType.Knight),   // n
            (C.White,  PType.None),     // 2
            (C.White,  PType.None)
        };
        var expected6 = new[]
        {   // p2pBpNp
            (C.Black, PType.BPawn),     // p
            (C.White,  PType.None),     // 2
            (C.White,  PType.None),
            (C.Black, PType.BPawn),     // p
            (C.White, PType.Bishop),   // B
            (C.Black, PType.BPawn),     // p
            (C.White, PType.Knight),   // N
            (C.Black, PType.BPawn),     // p
        };
        var expected7 = new[]
        {   // r1bk3r
            (C.Black, PType.Rook),     // r
            (C.White,  PType.None),     // 1
            (C.Black, PType.Bishop),   // b
            (C.Black, PType.King),     // k
            (C.White,  PType.None),     // 3
            (C.White,  PType.None),
            (C.White,  PType.None),
            (C.Black, PType.Rook),     // r
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

    [Fact]
    public void TestParseBitBoardFromFenStartPosition()
    {
        var startPosition = FenParser.StartPosition.Split().First();
        var bitBoard = _fenParser.ParseBitBoard(startPosition);
        bitBoard[C.White, PType.WPawn]  .Should().Be(0x000000000000FF00);
        bitBoard[C.Black, PType.BPawn]  .Should().Be(0x00FF000000000000);
        bitBoard[C.White, PType.Knight].Should().Be(0x0000000000000042);
        bitBoard[C.Black, PType.Knight].Should().Be(0x4200000000000000);
        bitBoard[C.White, PType.Bishop].Should().Be(0x0000000000000024);
        bitBoard[C.Black, PType.Bishop].Should().Be(0x2400000000000000);
        bitBoard[C.White, PType.Rook]  .Should().Be(0x0000000000000081);
        bitBoard[C.Black, PType.Rook]  .Should().Be(0x8100000000000000);
        bitBoard[C.White, PType.Queen] .Should().Be(0x0000000000000008);
        bitBoard[C.Black, PType.Queen] .Should().Be(0x0800000000000000);
        bitBoard[C.White, PType.King]  .Should().Be(0x0000000000000010);
        bitBoard[C.Black, PType.King]  .Should().Be(0x1000000000000000);
    }

    [Fact]
    public void TestParseBitBoardFromFenEmptyPosition()
    {
        var emptyPosition = "8/8/8/8/8/8/8/8";
        var bitBoard = _fenParser.ParseBitBoard(emptyPosition);
        bitBoard[C.White, PType.WPawn]  .Should().Be(0x0000000000000000);
        bitBoard[C.Black, PType.BPawn]  .Should().Be(0x0000000000000000);
        bitBoard[C.White, PType.Knight].Should().Be(0x0000000000000000);
        bitBoard[C.Black, PType.Knight].Should().Be(0x0000000000000000);
        bitBoard[C.White, PType.Bishop].Should().Be(0x0000000000000000);
        bitBoard[C.Black, PType.Bishop].Should().Be(0x0000000000000000);
        bitBoard[C.White, PType.Rook]  .Should().Be(0x0000000000000000);
        bitBoard[C.Black, PType.Rook]  .Should().Be(0x0000000000000000);
        bitBoard[C.White, PType.Queen] .Should().Be(0x0000000000000000);
        bitBoard[C.Black, PType.Queen] .Should().Be(0x0000000000000000);
        bitBoard[C.White, PType.King]  .Should().Be(0x0000000000000000);
        bitBoard[C.Black, PType.King]  .Should().Be(0x0000000000000000);
    }

    [Fact]
    public void TestParseBitBoardFromFenCustomPosition()
    {
        var customPosition = "r3k2r/1pp1qpb1/p1np1np1/3Pp3/2P1P3/2N2N2/PP2BPPP/R1BQK2R";
        var bitBoard = _fenParser.ParseBitBoard(customPosition);
        bitBoard[C.White, PType.WPawn]  .Should().Be(0x000000081400E300);  // h8 00000000 00000000 00000000 00001000 00010100 00000000 11100011 00000000 a1
        bitBoard[C.Black, PType.BPawn]  .Should().Be(0x0026491000000000);  // h8 00000000 00100110 01001001 00010000 00000000 00000000 00000000 00000000 a1
        bitBoard[C.White, PType.Knight].Should().Be(0x0000000000240000);  // h8 00000000 00000000 00000000 00000000 00000000 00100100 00000000 00000000 a1
        bitBoard[C.Black, PType.Knight].Should().Be(0x0000240000000000);  // h8 00000000 00000000 00100100 00000000 00000000 00000000 00000000 00000000 a1
        bitBoard[C.White, PType.Bishop].Should().Be(0x0000000000001004);  // h8 00000000 00000000 00000000 00000000 00000000 00000000 00010000 00000100 a1
        bitBoard[C.Black, PType.Bishop].Should().Be(0x0040000000000000);  // h8 00000000 01000000 00000000 00000000 00000000 00000000 00000000 00000000 a1
        bitBoard[C.White, PType.Rook]  .Should().Be(0x0000000000000081);  // h8 00000000 00000000 00000000 00000000 00000000 00000000 00000000 10000001 a1
        bitBoard[C.Black, PType.Rook]  .Should().Be(0x8100000000000000);  // h8 10000001 00000000 00000000 00000000 00000000 00000000 00000000 00000000 a1
        bitBoard[C.White, PType.Queen] .Should().Be(0x0000000000000008);  // h8 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00001000 a1
        bitBoard[C.Black, PType.Queen] .Should().Be(0x0010000000000000);  // h8 00000000 00010000 00000000 00000000 00000000 00000000 00000000 00000000 a1
        bitBoard[C.White, PType.King]  .Should().Be(0x0000000000000010);  // h8 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00010000 a1
        bitBoard[C.Black, PType.King]  .Should().Be(0x1000000000000000);  // h8 00010000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 a1
    }

    [Fact]
    public void TestParseBitBoardFromFenInvalidPosition()
    {
        var invalidPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNX";
        Action act = () => _fenParser.ParseBitBoard(invalidPosition);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void TestParseBitBoardFromPieceArray()
    {
        var empty = new Piece(C.White, PType.None);
        var wPawn = new Piece(C.White, PType.WPawn);
        var bPawn = new Piece(C.Black, PType.BPawn);
        var pieceArray = new[]
        {
            new Piece(C.White, PType.Rook),   // a1
            new Piece(C.White, PType.Knight), // b1
            new Piece(C.White, PType.Bishop), // c1
            new Piece(C.White, PType.Queen),  // d1
            new Piece(C.White, PType.King),   // e1
            new Piece(C.White, PType.Bishop), // f1
            new Piece(C.White, PType.Knight), // g1
            new Piece(C.White, PType.Rook),   // h1
            wPawn, wPawn, wPawn, empty, empty, wPawn, wPawn, wPawn, // a2 - h2
            empty, empty, empty, empty, wPawn, empty, empty, empty, // a3 - h3
            empty, empty, empty, wPawn, empty, empty, empty, empty, // a4 - h4
            empty, empty, empty, empty, empty, empty, empty, bPawn, // a5 - h5
            empty, empty, empty, empty, empty, empty, empty, empty, // a6 - h6
            bPawn, bPawn, bPawn, bPawn, bPawn, bPawn, bPawn, empty, // a7 - h7
            new Piece(C.Black, PType.Rook),   // a8
            new Piece(C.Black, PType.Knight), // b8
            new Piece(C.Black, PType.Bishop), // c8
            new Piece(C.Black, PType.Queen),  // d8
            new Piece(C.Black, PType.King),   // e8
            new Piece(C.Black, PType.Bishop), // f8
            new Piece(C.Black, PType.Knight), // g8
            new Piece(C.Black, PType.Rook),   // h8
        };

        var bitBoard = _fenParser.ParseBitBoard(pieceArray);
        bitBoard[C.White, PType.WPawn]  .Should().Be(0x000000000810E700); // h8 00000000 00000000 00000000 00000000 00001000 00010000 11100111 00000000 a1
        bitBoard[C.Black, PType.BPawn]  .Should().Be(0x007F008000000000); // h8 00000000 01111111 00000000 10000000 00000000 00000000 00000000 00000000 a1
        bitBoard[C.White, PType.Knight].Should().Be(0x0000000000000042);
        bitBoard[C.Black, PType.Knight].Should().Be(0x4200000000000000);
        bitBoard[C.White, PType.Bishop].Should().Be(0x0000000000000024);
        bitBoard[C.Black, PType.Bishop].Should().Be(0x2400000000000000);
        bitBoard[C.White, PType.Rook]  .Should().Be(0x0000000000000081);
        bitBoard[C.Black, PType.Rook]  .Should().Be(0x8100000000000000);
        bitBoard[C.White, PType.Queen] .Should().Be(0x0000000000000008);
        bitBoard[C.Black, PType.Queen] .Should().Be(0x0800000000000000);
        bitBoard[C.White, PType.King]  .Should().Be(0x0000000000000010);
        bitBoard[C.Black, PType.King]  .Should().Be(0x1000000000000000);
    }

    private static Piece[] CreateStartPositionPieceArray()
    {
        var empty = new Piece(C.White, PType.None);
        var wPawn = new Piece(C.White, PType.WPawn);
        var bPawn = new Piece(C.Black, PType.BPawn);

        return new[]
        {
            new Piece(C.White, PType.Rook),   // a1
            new Piece(C.White, PType.Knight), // b1
            new Piece(C.White, PType.Bishop), // c1
            new Piece(C.White, PType.Queen),  // d1
            new Piece(C.White, PType.King),   // e1
            new Piece(C.White, PType.Bishop), // f1
            new Piece(C.White, PType.Knight), // g1
            new Piece(C.White, PType.Rook),   // h1
            wPawn, wPawn, wPawn, wPawn, wPawn, wPawn, wPawn, wPawn, // a2 - h2
            empty, empty, empty, empty, empty, empty, empty, empty, // a3 - h3
            empty, empty, empty, empty, empty, empty, empty, empty, // a4 - h4
            empty, empty, empty, empty, empty, empty, empty, empty, // a5 - h5
            empty, empty, empty, empty, empty, empty, empty, empty, // a6 - h6
            bPawn, bPawn, bPawn, bPawn, bPawn, bPawn, bPawn, bPawn, // a7 - h7
            new Piece(C.Black, PType.Rook),   // a8
            new Piece(C.Black, PType.Knight), // b8
            new Piece(C.Black, PType.Bishop), // c8
            new Piece(C.Black, PType.Queen),  // d8
            new Piece(C.Black, PType.King),   // e8
            new Piece(C.Black, PType.Bishop), // f8
            new Piece(C.Black, PType.Knight), // g8
            new Piece(C.Black, PType.Rook)    // h8
        };
    }

    private static BitBoard CreateStartPositionBitBoard()
    {
        var white = (byte)C.White << 3;
        var black = (byte)C.Black << 3;
        var pieceBitBoards = new Dictionary<int, ulong>
        {
            { white | (byte)PType.WPawn,  0x000000000000FF00 },
            { white | (byte)PType.Knight, 0x0000000000000042 },
            { white | (byte)PType.Bishop, 0x0000000000000024 },
            { white | (byte)PType.Rook,   0x0000000000000081 },
            { white | (byte)PType.Queen,  0x0000000000000008 },
            { white | (byte)PType.King,   0x0000000000000010 },
            { black | (byte)PType.BPawn,  0x00FF000000000000 },
            { black | (byte)PType.Knight, 0x4200000000000000 },
            { black | (byte)PType.Bishop, 0x2400000000000000 },
            { black | (byte)PType.Rook,   0x8100000000000000 },
            { black | (byte)PType.Queen,  0x0800000000000000 },
            { black | (byte)PType.King,   0x1000000000000000 },
        };

        return BitBoard.FromDictionary(pieceBitBoards);
    }
}
