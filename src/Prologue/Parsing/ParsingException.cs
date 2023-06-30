namespace Prologue.Parsing;

/// <summary>
/// An exception thrown when errors are encountered while parsing a source.
/// </summary>
public sealed class ParsingException : Exception
{
    public ParsingException()
    {
    }

    public ParsingException(string message) : base(message)
    {
    }

    public ParsingException(string message, Exception inner) : base(message, inner)
    {
    }
}