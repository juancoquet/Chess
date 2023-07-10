using Chess.Board;
using Chess.Generics;

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
        var expectedSet = new HashSet<char>() { 'k', 'q', 'K', 'Q', '-' };
        if (fenCastleRights.ToCharArray().Any(c => !expectedSet.Contains(c)))
        {
            throw new ArgumentException($"Invalid character(s) castle rights FEN string: {fenCastleRights}");
        }
        if (fenCastleRights.Length < 1 || fenCastleRights.Length > 4)
        {
            throw new ArgumentException("FEN castle rights string must be between 1 and 4 characters long");
        }

        if (fenCastleRights == "-")
        {
            return new ChessBoard.CastleRightsState()
            {
                White = ECastleRights.None,
                Black = ECastleRights.None
            };
        }

        var tokens = fenCastleRights.ToCharArray();
        var whiteTokens = tokens.Where(c => char.IsUpper(c)).SomeNotNull();
        var blackTokens = tokens.Where(c => char.IsLower(c)).SomeNotNull();
        var bothTokenSets = new[] { whiteTokens, blackTokens };
        var bothRights = bothTokenSets.Select(
            tokenSet =>
                tokenSet.Match(
                    hasRights => TokensToCastleRights(hasRights),
                    () => ECastleRights.None
                )
        );

        return new ChessBoard.CastleRightsState()
        {
            White = bothRights.First(),
            Black = bothRights.Last()
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

}
