using Prologue.Parsing;

namespace Prologue.Tests.Parsing;

public sealed class LexerTests
{
    [Fact]
    public void LexSymbols()
    {
        Check("a b some_symbol someOtherSymbol",
            new[]
            {
                (Token.Kind.Symbol, "a"), (Token.Kind.Symbol, "b"), (Token.Kind.Symbol, "some_symbol"),
                (Token.Kind.Symbol, "someOtherSymbol"), (Token.Kind.Eof, "")
            });
    }

    [Fact]
    public void LexVariables()
    {
        Check("X _1 Some_variable SomeOtherVariable",
            new[]
            {
                (Token.Kind.Variable, "X"), (Token.Kind.Variable, "_1"), (Token.Kind.Variable, "Some_variable"),
                (Token.Kind.Variable, "SomeOtherVariable"), (Token.Kind.Eof, "")
            });
    }

    [Fact]
    public void LexPunctuation()
    {
        Check("() :- , .",
            new[]
            {
                (Token.Kind.LeftParen, "("), (Token.Kind.RightParen, ")"), (Token.Kind.Neck, ":-"),
                (Token.Kind.Comma, ","), (Token.Kind.Period, "."), (Token.Kind.Eof, "")
            });
    }

    /// <summary>
    /// Checks that the lexer produces tokens of some expected kinds with some expected values for a given input.
    /// </summary>
    private void Check(string input, IEnumerable<(Token.Kind, string)> expected)
    {
        var source = new Source(input);
        var lexer = new Lexer(source);
        foreach (var (kind, value) in expected)
        {
            var token = lexer.Next();
            Assert.NotNull(token);
            Assert.Equal(kind, token.Type);
            Assert.Equal(value, source[token.Range]);
        }
    }
}