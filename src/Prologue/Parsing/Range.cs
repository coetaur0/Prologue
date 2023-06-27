namespace Prologue.Parsing;

/// <summary>
/// A range between two locations in a source.
/// </summary>
/// <param name="Start">The range's start location.</param>
/// <param name="End">The range's end location.</param>
public sealed record Range(Location Start, Location End)
{
    /// <summary>
    /// Returns a string representation of the range.
    /// </summary>
    public override string ToString() => $"{Start}..{End}";
}