using Prologue.Parsing;

namespace Prologue.Tests.Parsing;

public class ParserTests
{
    [Fact]
    public void ParseValidProgram()
    {
        var source = new Source("a.\nb.\nequal(a, a).\nequal(b, b).\nequal(X, Y) :- equal(X, Z), equal(Z, Y).");
        var parser = new Parser(source);
        var (program, diagnostics) = parser.ParseProgram();

        Assert.Empty(diagnostics);

        Assert.Equal(
            "a.\n\nb.\n\nequal(a, a).\nequal(b, b).\nequal(X, Y) :- equal(X, Z), equal(Z, Y).\n\n",
            program.ToString()
        );

        // Check that the two instances of variable Z in the body of predicate equal's third clause are the same.
        var functor = new Functor("equal", 2);
        Assert.Equal(program[functor][2].Body[0].Arguments[1], program[functor][2].Body[1].Arguments[0]);
    }

    [Fact]
    public void ParseInvalidProgram()
    {
        var source = new Source(".\nok(a, b.\nok(X, Y) :- ok(X, Z), Z.\nok(X, Y) :- ok(X, Z), ok(Z, Y)");
        var parser = new Parser(source);
        var (_, diagnostics) = parser.ParseProgram();

        Assert.Equal(4, diagnostics.Count);

        Assert.Equal("1:1..1:2: expect a symbol.", diagnostics[0].ToString());
        Assert.Equal("2:8..2:9: expect a ')' at the end of a structure's arguments list.", diagnostics[1].ToString());
        Assert.Equal("3:23..3:24: expect a symbol.", diagnostics[2].ToString());
        Assert.Equal("4:31..4:31: expect a '.' at the end of a clause.", diagnostics[3].ToString());
    }

    [Fact]
    public void ParseValidQuery()
    {
        var source = new Source("equal(X, Y), equal(Y, s(a, b)).");
        var parser = new Parser(source);
        var (query, diagnostics) = parser.ParseQuery();

        Assert.Empty(diagnostics);

        Assert.Equal("?- equal(X, Y), equal(Y, s(a, b)).", query.ToString());

        // Check that the two instances of variable Y in the query are the same.
        Assert.Equal(query.Goals[0].Arguments[1], query.Goals[1].Arguments[0]);
    }

    [Fact]
    public void ParseInvalidQuery()
    {
        var source = new Source("X, equal(,Y), s(a, b");
        var parser = new Parser(source);
        var (_, diagnostics) = parser.ParseQuery();

        Assert.Equal(4, diagnostics.Count);

        Assert.Equal("1:1..1:2: expect a symbol.", diagnostics[0].ToString());
        Assert.Equal("1:10..1:11: expect a Prolog term.", diagnostics[1].ToString());
        Assert.Equal("1:21..1:21: expect a ')' at the end of a structure's arguments list.", diagnostics[2].ToString());
        Assert.Equal("1:21..1:21: expect a '.' at the end of a query.", diagnostics[3].ToString());
    }
}