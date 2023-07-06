namespace Prologue.Parsing;

/// <summary>
/// A parser diagnostic.
/// </summary>
/// <param name="Message">The diagnostic message.</param>
/// <param name="Range">The range of the diagnostic in the source.</param>
internal sealed record Diagnostic(string Message, Range Range)
{
    /// <summary>
    /// Returns a string representation of the diagnostic.
    /// </summary>
    public override string ToString() => $"{Range}: {Message}.";
}