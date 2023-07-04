using Prologue.Parsing;

namespace Prologue.Tests;

public class ParsingTests
{
    [Fact]
    public void LoadValidKnowledgeBase()
    {
        var knowledgeBase =
            KnowledgeBase.Load("a.\nb.\nequal(a, a).\nequal(b, b).\nequal(X, Y) :- equal(X, Z), equal(Z, Y).");

        Assert.Equal(
            "a.\n\nb.\n\nequal(a, a).\nequal(b, b).\nequal(X, Y) :- equal(X, Z), equal(Z, Y).\n\n",
            knowledgeBase.ToString()
        );

        // Check that the two instances of variable Z in the body of predicate equal's third clause are the same.
        var functor = new Functor("equal", 2);
        Assert.Equal(knowledgeBase[functor][2].Body[0].Arguments[1], knowledgeBase[functor][2].Body[1].Arguments[0]);
    }

    [Fact]
    public void LoadInvalidKnowledgeBase()
    {
        var exception = Assert.Throws<SyntaxException>(
            () => KnowledgeBase.Load(".\nok(a, b.\nok(X, Y) :- ok(X, Z), Z.\nok(X, Y) :- ok(X, Z), ok(Z, Y)")
        );

        const string message = "Syntax errors in input:\n" +
                               "\t- 1:1..1:2: expect a symbol.\n" +
                               "\t- 2:8..2:9: expect a ')' at the end of a structure's arguments list.\n" +
                               "\t- 3:23..3:24: expect a symbol.\n" +
                               "\t- 4:31..4:31: expect a '.' at the end of a clause.";

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void LoadValidQuery()
    {
        var query = Query.Load("equal(X, Y), equal(Y, s(a, b)).");

        Assert.Equal("?- equal(X, Y), equal(Y, s(a, b)).", query.ToString());

        // Check that the two instances of variable Y in the query are the same.
        Assert.Equal(query.Goals[0].Arguments[1], query.Goals[1].Arguments[0]);
    }

    [Fact]
    public void LoadInvalidQuery()
    {
        var exception = Assert.Throws<SyntaxException>(() => Query.Load("X, equal(,Y), s(a, b"));

        const string message = "Syntax errors in input:\n" +
                               "\t- 1:1..1:2: expect a symbol.\n" +
                               "\t- 1:10..1:11: expect a Prolog term.\n" +
                               "\t- 1:21..1:21: expect a ')' at the end of a structure's arguments list.\n" +
                               "\t- 1:21..1:21: expect a '.' at the end of a query.";

        Assert.Equal(message, exception.Message);
    }
}