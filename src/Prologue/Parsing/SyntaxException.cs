namespace Prologue.Parsing;

/// <summary>
/// An exception thrown when syntax errors are encountered while parsing a source.
/// </summary>
public sealed class SyntaxException : Exception
{
    public SyntaxException()
    {
    }

    public SyntaxException(string message) : base(message)
    {
    }

    public SyntaxException(string message, Exception inner) : base(message, inner)
    {
    }
}