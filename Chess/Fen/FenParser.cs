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
    public const string StartPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    public ChessBoard Parse(string fen)
    {
        var fields = fen.Split();
        if (fields.Length != 6)
        {
            throw new ArgumentException("FEN must contain 6 space-separated fields");
        }
        var pieces = ParsePieces(fields[0]);
        var turn = fields[1] switch
        {
            "w" => C.White,
            "b" => C.Black,
            _ => throw new ArgumentException($"Invalid turn: {fields[1]}")
        };
        var castleRights = ParseCastleRights(fields[2]);
        var enPassantTarget = ParseEnPassantSquare(fields[3]);
        var halfMoveClock = int.Parse(fields[4]);
        var moveNumber = int.Parse(fields[5]);
        var bitBoard = ParseBitBoard(pieces);
        var pieceArr = pieces.Select(p => p.PieceCode).ToArray();

        return new ChessBoard()
        {
            BitBoard = bitBoard,
            SquaresOccupants = pieceArr,
            Turn = turn,
            MoveNumber = moveNumber,
            HalfMoveClock = halfMoveClock,
            EnPassantTarget = enPassantTarget,
            CastleRights = castleRights
        };
    }

    internal Piece[] ParsePieces(string fenRanks)
    {
        var ranks = fenRanks.Split('/').Reverse();
        if (ranks.Count() != 8)
        {
            throw new ArgumentException($"FEN must contain 8 ranks, invalid:\n{fenRanks}");
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
                    var colour = char.IsUpper(token) ? C.White : C.Black;
                    var piece = char.ToLower(token) switch
                    {
                        'p' => new Piece(colour, colour == C.White ? PType.WPawn : PType.BPawn),
                        'n' => new Piece(colour, PType.Knight),
                        'b' => new Piece(colour, PType.Bishop),
                        'r' => new Piece(colour, PType.Rook),
                        'q' => new Piece(colour, PType.Queen),
                        'k' => new Piece(colour, PType.King),
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
        var files = new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8' };
        var tokens = fenEnPassant.ToCharArray().Select(c => char.ToLower(c)).ToArray();
        if (!ranks.Contains(tokens[0]) | !files.Contains(tokens[1]))
        {
            throw new ArgumentException($"FEN string for en passant target is not a valid square: {fenEnPassant}");
        }
    }

    internal BitBoard ParseBitBoard(string fenRanks)
    {
        var pieces = ParsePieces(fenRanks);
        return ParseBitBoard(pieces);
    }

    internal BitBoard ParseBitBoard(Piece[] pieces)
    {
        var colours = Enum.GetValues(typeof(C)).Cast<C>();
        var pieceTypes = Enum.GetValues(typeof(PType)).Cast<PType>();

        var pieceBitBoards = colours.SelectMany(colour =>
        {
            var pieceBitBoards = pieceTypes.Select(pieceType =>
            {
                var pieceCode = (byte)colour << 3 | (byte)pieceType;
                var bits = pieces.Select((piece, i) =>
                    piece.Is(colour, pieceType) ? 1UL << i : 0UL
                ).Aggregate((a, b) => a | b);
                return (pieceCode, bits);
            }).ToDictionary(
                tuple => tuple.pieceCode,
                tuple => tuple.bits
            );
            return pieceBitBoards;
        }).ToDictionary(
            pair => pair.Key,
            pair => pair.Value
        );

        return BitBoard.FromDictionary(pieceBitBoards);
    }
}
