using Chess.Generics;

namespace Chess.Tests;

public class GenericsTests
{
    [Fact]
    public void TestSquareToRank()
    {
        Square.A1.Rank().Should().Be(Ranks.R1);
        Square.A2.Rank().Should().Be(Ranks.R2);
        Square.A3.Rank().Should().Be(Ranks.R3);
        Square.A4.Rank().Should().Be(Ranks.R4);
        Square.A5.Rank().Should().Be(Ranks.R5);
        Square.A6.Rank().Should().Be(Ranks.R6);
        Square.A7.Rank().Should().Be(Ranks.R7);
        Square.A8.Rank().Should().Be(Ranks.R8);

        Square.B1.Rank().Should().Be(Ranks.R1);
        Square.B2.Rank().Should().Be(Ranks.R2);
        Square.B3.Rank().Should().Be(Ranks.R3);
        Square.B4.Rank().Should().Be(Ranks.R4);
        Square.B5.Rank().Should().Be(Ranks.R5);
        Square.B6.Rank().Should().Be(Ranks.R6);
        Square.B7.Rank().Should().Be(Ranks.R7);
        Square.B8.Rank().Should().Be(Ranks.R8);

        Square.C1.Rank().Should().Be(Ranks.R1);
        Square.C2.Rank().Should().Be(Ranks.R2);
        Square.C3.Rank().Should().Be(Ranks.R3);
        Square.C4.Rank().Should().Be(Ranks.R4);
        Square.C5.Rank().Should().Be(Ranks.R5);
        Square.C6.Rank().Should().Be(Ranks.R6);
        Square.C7.Rank().Should().Be(Ranks.R7);
        Square.C8.Rank().Should().Be(Ranks.R8);

        Square.D1.Rank().Should().Be(Ranks.R1);
        Square.D2.Rank().Should().Be(Ranks.R2);
        Square.D3.Rank().Should().Be(Ranks.R3);
        Square.D4.Rank().Should().Be(Ranks.R4);
        Square.D5.Rank().Should().Be(Ranks.R5);
        Square.D6.Rank().Should().Be(Ranks.R6);
        Square.D7.Rank().Should().Be(Ranks.R7);
        Square.D8.Rank().Should().Be(Ranks.R8);

        Square.E1.Rank().Should().Be(Ranks.R1);
        Square.E2.Rank().Should().Be(Ranks.R2);
        Square.E3.Rank().Should().Be(Ranks.R3);
        Square.E4.Rank().Should().Be(Ranks.R4);
        Square.E5.Rank().Should().Be(Ranks.R5);
        Square.E6.Rank().Should().Be(Ranks.R6);
        Square.E7.Rank().Should().Be(Ranks.R7);
        Square.E8.Rank().Should().Be(Ranks.R8);

        Square.F1.Rank().Should().Be(Ranks.R1);
        Square.F2.Rank().Should().Be(Ranks.R2);
        Square.F3.Rank().Should().Be(Ranks.R3);
        Square.F4.Rank().Should().Be(Ranks.R4);
        Square.F5.Rank().Should().Be(Ranks.R5);
        Square.F6.Rank().Should().Be(Ranks.R6);
        Square.F7.Rank().Should().Be(Ranks.R7);
        Square.F8.Rank().Should().Be(Ranks.R8);

        Square.G1.Rank().Should().Be(Ranks.R1);
        Square.G2.Rank().Should().Be(Ranks.R2);
        Square.G3.Rank().Should().Be(Ranks.R3);
        Square.G4.Rank().Should().Be(Ranks.R4);
        Square.G5.Rank().Should().Be(Ranks.R5);
        Square.G6.Rank().Should().Be(Ranks.R6);
        Square.G7.Rank().Should().Be(Ranks.R7);
        Square.G8.Rank().Should().Be(Ranks.R8);

        Square.H1.Rank().Should().Be(Ranks.R1);
        Square.H2.Rank().Should().Be(Ranks.R2);
        Square.H3.Rank().Should().Be(Ranks.R3);
        Square.H4.Rank().Should().Be(Ranks.R4);
        Square.H5.Rank().Should().Be(Ranks.R5);
        Square.H6.Rank().Should().Be(Ranks.R6);
        Square.H7.Rank().Should().Be(Ranks.R7);
        Square.H8.Rank().Should().Be(Ranks.R8);
    }
}
