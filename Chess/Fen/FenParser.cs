using Chess.Board;
using Chess.Generics;
using ConsoleExtensions;

namespace Chess.Fen;

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
    public ChessBoard Parse(string fen)
    {
        var fields = fen.Split();
        if (fields.Length != 6)
        {
            throw new ArgumentException("FEN must contain 6 space-separated fields");
        }
        var pieces = ParsePieces(fields[1]);
        var turn = fields[1] switch
        {
            "w" => Colour.White,
            "b" => Colour.Black,
            _ => throw new ArgumentException($"Invalid turn: {fields[1]}")
        };
        var castleRights = ParseCastleRights(fields[2]);

        return new ChessBoard();
    }

    internal Piece[] ParsePieces(string fenRanks)
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
        return pieces.ToArray();
    }

    internal ChessBoard.ICastleRights ParseCastleRights(string fenCastleRights)
    {
        ValidateCastleRightsFen(fenCastleRights);
        if (fenCastleRights == "-")
        {
            return new ChessBoard.CastleRightsState()
            {
                White = ECastleRights.None,
                Black = ECastleRights.None
            };
        }

        var tokens = fenCastleRights.ToCharArray();
        var whiteTokens = tokens.Where(c => char.IsUpper(c)).SomeWhen(
            tokenSet => tokenSet.Count() > 0,
            () => Option.None<IEnumerable<char>>()
        );
        var blackTokens = tokens.Where(c => char.IsLower(c)).SomeWhen(
            tokenSet => tokenSet.Count() > 0,
            () => Option.None<IEnumerable<char>>()
        );
        var whiteCastleRights = whiteTokens.Match(
            tokens   => TokensToCastleRights(tokens),
            noTokens => ECastleRights.None
        );
        var blackCastleRights = blackTokens.Match(
            tokens   => TokensToCastleRights(tokens),
            noTokens => ECastleRights.None
        );

        return new ChessBoard.CastleRightsState()
        {
            White = whiteCastleRights,
            Black = blackCastleRights
        };
    }

    private static ECastleRights TokensToCastleRights(IEnumerable<char> tokens)
    {
        var tokenStr = string.Join("", tokens).ToLower();
        var rights = tokenStr switch
        {
            "k"  => ECastleRights.KingSide,
            "q"  => ECastleRights.QueenSide,
            "kq" => ECastleRights.BothSides,
            "qk" => ECastleRights.BothSides,
            _ => throw new ArgumentException($"Invalid castle rights FEN string: {tokenStr}")
        };
        return rights;
    }

    private static void ValidateCastleRightsFen(string fenCastleRights)
    {
        var expectedSet = new HashSet<char>() { 'k', 'q', 'K', 'Q', '-' };
        if (fenCastleRights.ToCharArray().Any(c => !expectedSet.Contains(c)))
        {
            throw new ArgumentException($"Invalid character(s) castle rights FEN string: {fenCastleRights}");
        }
        if (fenCastleRights.Length < 1 || fenCastleRights.Length > 4)
        {
            throw new ArgumentException("FEN castle rights string must be between 1 and 4 characters long");
        }
    }

    internal Square ParseEnPassantSquare(string fenEnPassant)
    {
        if (fenEnPassant == "-") { return Square.None; }
        ValidateEnPassantFen(fenEnPassant);
        return (Square) Enum.Parse(typeof(Square), fenEnPassant.ToUpper());
    }

    private static void ValidateEnPassantFen(string fenEnPassant)
    {
        if (fenEnPassant.Length != 2)
        {
            throw new ArgumentException("FEN en passant target square string must be 2 character long");
        }
        var ranks = new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        var files = MoreLinq.MoreEnumerable.ToHashSet(Enumerable.Range(1, 8));
        var tokens = fenEnPassant.ToCharArray();
        Terminal.WriteLine($"tokens: {tokens[0]}, {tokens[1]}");
        if (!ranks.Contains(tokens[0]) && !files.Contains(tokens[1]))
        {
            throw new ArgumentException($"FEN string for en passant target is not a valid square {fenEnPassant}");
        }
    }
}
