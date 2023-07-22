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

    [Fact]
    public void TestSquareToFile()
    {
        Square.A1.File().Should().Be(Files.A);
        Square.A2.File().Should().Be(Files.A);
        Square.A3.File().Should().Be(Files.A);
        Square.A4.File().Should().Be(Files.A);
        Square.A5.File().Should().Be(Files.A);
        Square.A6.File().Should().Be(Files.A);
        Square.A7.File().Should().Be(Files.A);
        Square.A8.File().Should().Be(Files.A);

        Square.B1.File().Should().Be(Files.B);
        Square.B2.File().Should().Be(Files.B);
        Square.B3.File().Should().Be(Files.B);
        Square.B4.File().Should().Be(Files.B);
        Square.B5.File().Should().Be(Files.B);
        Square.B6.File().Should().Be(Files.B);
        Square.B7.File().Should().Be(Files.B);
        Square.B8.File().Should().Be(Files.B);

        Square.C1.File().Should().Be(Files.C);
        Square.C2.File().Should().Be(Files.C);
        Square.C3.File().Should().Be(Files.C);
        Square.C4.File().Should().Be(Files.C);
        Square.C5.File().Should().Be(Files.C);
        Square.C6.File().Should().Be(Files.C);
        Square.C7.File().Should().Be(Files.C);
        Square.C8.File().Should().Be(Files.C);

        Square.D1.File().Should().Be(Files.D);
        Square.D2.File().Should().Be(Files.D);
        Square.D3.File().Should().Be(Files.D);
        Square.D4.File().Should().Be(Files.D);
        Square.D5.File().Should().Be(Files.D);
        Square.D6.File().Should().Be(Files.D);
        Square.D7.File().Should().Be(Files.D);
        Square.D8.File().Should().Be(Files.D);

        Square.E1.File().Should().Be(Files.E);
        Square.E2.File().Should().Be(Files.E);
        Square.E3.File().Should().Be(Files.E);
        Square.E4.File().Should().Be(Files.E);
        Square.E5.File().Should().Be(Files.E);
        Square.E6.File().Should().Be(Files.E);
        Square.E7.File().Should().Be(Files.E);
        Square.E8.File().Should().Be(Files.E);

        Square.F1.File().Should().Be(Files.F);
        Square.F2.File().Should().Be(Files.F);
        Square.F3.File().Should().Be(Files.F);
        Square.F4.File().Should().Be(Files.F);
        Square.F5.File().Should().Be(Files.F);
        Square.F6.File().Should().Be(Files.F);
        Square.F7.File().Should().Be(Files.F);
        Square.F8.File().Should().Be(Files.F);

        Square.G1.File().Should().Be(Files.G);
        Square.G2.File().Should().Be(Files.G);
        Square.G3.File().Should().Be(Files.G);
        Square.G4.File().Should().Be(Files.G);
        Square.G5.File().Should().Be(Files.G);
        Square.G6.File().Should().Be(Files.G);
        Square.G7.File().Should().Be(Files.G);
        Square.G8.File().Should().Be(Files.G);

        Square.H1.File().Should().Be(Files.H);
        Square.H2.File().Should().Be(Files.H);
        Square.H3.File().Should().Be(Files.H);
        Square.H4.File().Should().Be(Files.H);
        Square.H5.File().Should().Be(Files.H);
        Square.H6.File().Should().Be(Files.H);
        Square.H7.File().Should().Be(Files.H);
        Square.H8.File().Should().Be(Files.H);
    }

    [Fact]
    public void TestRanksIndex()
    {
        Ranks.R1.Index().Should().Be(0);
        Ranks.R2.Index().Should().Be(1);
        Ranks.R3.Index().Should().Be(2);
        Ranks.R4.Index().Should().Be(3);
        Ranks.R5.Index().Should().Be(4);
        Ranks.R6.Index().Should().Be(5);
        Ranks.R7.Index().Should().Be(6);
        Ranks.R8.Index().Should().Be(7);
    }

    [Fact]
    public void TestFilesIndex()
    {
        Files.A.Index().Should().Be(0);
        Files.B.Index().Should().Be(1);
        Files.C.Index().Should().Be(0);
        Files.D.Index().Should().Be(3);
        Files.E.Index().Should().Be(4);
        Files.F.Index().Should().Be(5);
        Files.G.Index().Should().Be(6);
        Files.H.Index().Should().Be(7);
    }
}
