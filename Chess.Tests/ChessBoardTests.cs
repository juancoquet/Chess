using Chess.Board;
using Chess.Generics;

public class ChessBoardTests
{
    private readonly ChessBoard _board;

    public ChessBoardTests()
    {
        _board = ChessBoard.FromFen("r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1 w -- - 23 12");
        // r . b k . . . r
        // p . . p B p N p
        // n . . . N . . .
        // . p . N P . . P
        // . . . . . . P .
        // . . . P . . . .
        // P . P . K . . .
        // q . . . . . b .
    }

    [Fact]
    public void BasicIsValidMove()
    {
        _board.IsValidMove(new Move() { From = Square.C2, To = Square.C4, }).Should().BeTrue();
        _board.IsValidMove(new Move() { From = Square.C2, To = Square.C3, }).Should().BeTrue();
        _board.IsValidMove(new Move() { From = Square.C2, To = Square.C5, }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.D5, To = Square.F4, }).Should().BeTrue();
    }

    [Fact]
    public void BlocksAreIlllegal()
    {
        _board.IsValidMove(new Move() { From = Square.E2, To = Square.D3, }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.G7, To = Square.H5, }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.E7, To = Square.G5, }).Should().BeFalse();
        _board.Turn = C.Black;
        _board.IsValidMove(new Move() { From = Square.A1, To = Square.G1, }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.A1, To = Square.H1, }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.G1, To = Square.A7, }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.H8, To = Square.D8, }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.H8, To = Square.D5, }).Should().BeFalse();
    }
}
