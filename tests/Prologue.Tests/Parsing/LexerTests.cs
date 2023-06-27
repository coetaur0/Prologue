using Prologue.Parsing;

namespace Prologue.Tests.Parsing;

public sealed class LexerTests
{
    [Fact]
    public void SkipComments()
    {
        Check("% This is a comment\n% And another one", new[] { (TokenKind.Eof, "") });
    }

    [Fact]
    public void LexSymbols()
    {
        Check("a b some_symbol someOtherSymbol",
            new[]
            {
                (TokenKind.Symbol, "a"), (TokenKind.Symbol, "b"), (TokenKind.Symbol, "some_symbol"),
                (TokenKind.Symbol, "someOtherSymbol"), (TokenKind.Eof, "")
            });
    }

    [Fact]
    public void LexVariables()
    {
        Check("X _1 Some_variable SomeOtherVariable",
            new[]
            {
                (TokenKind.Variable, "X"), (TokenKind.Variable, "_1"), (TokenKind.Variable, "Some_variable"),
                (TokenKind.Variable, "SomeOtherVariable"), (TokenKind.Eof, "")
            });
    }

    [Fact]
    public void LexPunctuation()
    {
        Check("() :- , . $",
            new[]
            {
                (TokenKind.LeftParen, "("), (TokenKind.RightParen, ")"), (TokenKind.Neck, ":-"),
                (TokenKind.Comma, ","), (TokenKind.Period, "."), (TokenKind.Unknown, "$"), (TokenKind.Eof, "")
            });
    }

    /// <summary>
    /// Checks that the lexer produces tokens with the expected kinds and values for some given input.
    /// </summary>
    private void Check(string input, IEnumerable<(TokenKind, string)> expected)
    {
        var source = new Source(input);
        var lexer = new Lexer(source);
        foreach (var (kind, value) in expected)
        {
            var token = lexer.Next();
            Assert.NotNull(token);
            Assert.Equal(kind, token.Kind);
            Assert.Equal(value, source[token.Range]);
        }
    }
}