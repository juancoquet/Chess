using Chess.Board;
using Chess.Generics;

public class ChessBoardTests
{
    private readonly ChessBoard _board;

    public ChessBoardTests()
    {
        _board = ChessBoard.FromFen("r1bk3r/p2p1pNp/n2B1n2/3NP2P/2p3P1/3P4/P1P1K3/q5b1 w -- - 23 12");
        // 8  r . b k . . . r
        // 7  p . . p . p N p
        // 6  n . . B . n . .
        // 5  . . . N P . . P
        // 4  . . p . . . P .
        // 3  . . . P . . . .
        // 2  P . P . K . . .
        // 1  q . . . . . b .
        //    A B C D E F G H
    }

    [Fact]
    public void BasicIsValidMove()
    {
        _board.IsValidMove(new Move() { From = Square.C2, To = Square.C3 }).Should().BeTrue();
        _board.IsValidMove(new Move() { From = Square.C2, To = Square.C5 }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.D5, To = Square.F4 }).Should().BeTrue();
    }

    [Fact]
    public void BlocksAreIlllegal()
    {
        _board.IsValidMove(new Move() { From = Square.E2, To = Square.D3 }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.G7, To = Square.H5 }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.D6, To = Square.F4 }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.C2, To = Square.C4 }).Should().BeFalse();
        _board.Turn = C.Black;
        _board.IsValidMove(new Move() { From = Square.A1, To = Square.G1 }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.A1, To = Square.H1 }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.G1, To = Square.A7 }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.H8, To = Square.D8 }).Should().BeFalse();
        _board.IsValidMove(new Move() { From = Square.H8, To = Square.D5 }).Should().BeFalse();
    }

    [Fact]
    public void TakesAreLegal()
    {
        _board.IsValidMove(new Move() { From = Square.E5, To = Square.F6 }).Should().BeTrue();
        _board.IsValidMove(new Move() { From = Square.D5, To = Square.F6 }).Should().BeTrue();
        _board.IsValidMove(new Move() { From = Square.D3, To = Square.C4 }).Should().BeTrue();
        _board.Turn = C.Black;
        _board.IsValidMove(new Move() { From = Square.A1, To = Square.A2 }).Should().BeTrue();
        _board.IsValidMove(new Move() { From = Square.A1, To = Square.E5 }).Should().BeTrue();
        _board.IsValidMove(new Move() { From = Square.F6, To = Square.D5 }).Should().BeTrue();
        _board.IsValidMove(new Move() { From = Square.C4, To = Square.D3 }).Should().BeTrue();
    }
}
