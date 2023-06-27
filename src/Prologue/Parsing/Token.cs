namespace Prologue.Parsing;

/// <summary>
/// The kind of a lexical token.
/// </summary>
public enum TokenKind
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
/// A lexical token.
/// </summary>
/// <param name="Kind">The token's kind.</param>
/// <param name="Range">The source range covered by the token.</param>
public sealed record Token(TokenKind Kind, Range Range);