using Board;
using Generics;

namespace Fen;

/// <summary>
/// A FEN contains 6 space-separated fields. The fields are:
/// 1. Piece placement, from A8 to H1. Lowercase is black, uppercase is white.
///    A number represents a sequence of n empty squares.
///    A '/' separates ranks.
/// 2. Turn. 'w' means white to move, 'b' means black to move.
/// 3. Castling rights. Casing indicates colour. K = kingside, Q = queenside.
///   '-' means no castling rights.
/// 4. En passant target square, or '-' if there is none.
/// 5. Halfmove clock.
/// 6. Move number.
/// e.g. rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1
/// </summary>
public class FenParser
{
    public static ChessBoard Parse(string fen)
    {
        var fields = fen.Split();
        if (fields.Length != 6)
        {
            throw new ArgumentException("FEN must contain 6 space-separated fields");
        }
        var pieces = ParsePieces(fields[0]);
        return new ChessBoard();
    }

    private static IEnumerable<Piece> ParsePieces(string fenRanks)
    {
        var ranks = fenRanks.Split('/').Reverse();
        if (ranks.Count() != 8)
        {
            throw new ArgumentException("FEN must contain 8 ranks");
        }
        var pieces = ranks.SelectMany(
            rank =>
                rank.SelectMany(token =>
                {
                    if (char.IsDigit(token))
                    {
                        var numEmptySquares = int.Parse(token.ToString());
                        return Enumerable.Repeat(Piece.None(), numEmptySquares);
                    }
                    var colour = char.IsUpper(token) ? Colour.White : Colour.Black;
                    var piece = char.ToLower(token) switch
                    {
                        'p' => new Piece(colour, PieceType.Pawn),
                        'n' => new Piece(colour, PieceType.Knight),
                        'b' => new Piece(colour, PieceType.Bishop),
                        'r' => new Piece(colour, PieceType.Rook),
                        'q' => new Piece(colour, PieceType.Queen),
                        'k' => new Piece(colour, PieceType.King),
                        _ => throw new ArgumentException($"Invalid piece type: {token}")
                    };
                    return new[] { piece };
                })
        );
        if (pieces.Count() != 64)
        {
            throw new ArgumentException("FEN must contain 64 squares");
        }
        return pieces;
    }
}
