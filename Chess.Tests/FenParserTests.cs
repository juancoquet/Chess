using Board;
using Chess.Fen;
using Generics;

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
    public void TestParsePieces()
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
    public void TestParsePiecesInvalidInputThrows()
    {
        Assert.Throws<ArgumentException>(() => _fenParser.ParsePieces("abc"));
        Assert.Throws<ArgumentException>(() => _fenParser.ParsePieces("rnbqkbnr/pppppppp"));
    }
}
