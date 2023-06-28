namespace Prologue.Parsing;

/// <summary>
/// A compiler diagnostic.
/// </summary>
/// <param name="Message">The diagnostic's message.</param>
/// <param name="Range">The range covered by the diagnostic in the source.</param>
public sealed record Diagnostic(string Message, Range Range)
{
    /// <summary>
    /// Returns a string representation of the diagnostic.
    /// </summary>
    public override string ToString() => $"{Range}: {Message}.";
}