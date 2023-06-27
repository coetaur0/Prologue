namespace Prologue.Parsing;

/// <summary>
/// A lexical token.
/// </summary>
public sealed class Token
{
    /// <summary>
    /// The kind of a token.
    /// </summary>
    public enum Kind
    {
        Symbol,
        Variable,
        LeftParen,
        RightParen,
        Neck,
        Comma,
        Period,
        Eof,
        Unknown
    }

    /// <summary>
    /// The token's type.
    /// </summary>
    public Kind Type { get; }

    /// <summary>
    /// The token's range.
    /// </summary>
    public Range Range { get; }

    public Token(Kind type, Range range)
    {
        Type = type;
        Range = range;
    }
}