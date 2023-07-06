namespace Prologue.Parsing;

/// <summary>
/// The kind of a lexical token.
/// </summary>
internal enum TokenKind
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
/// <param name="Range">The token's source range.</param>
internal sealed record Token(TokenKind Kind, Range Range);