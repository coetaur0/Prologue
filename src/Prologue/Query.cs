using Prologue.Parsing;

namespace Prologue;

/// <summary>
/// A Prolog query.
/// </summary>
/// <param name="Goals">The query's goals.</param>
public sealed record Query(Structure[] Goals)
{
    /// <summary>
    /// Loads a query from a string and returns it.
    /// </summary>
    public static Query FromString(string input)
    {
        var source = new Source("input", input);
        var parser = new Parser(source);
        return parser.ParseQuery();
    }

    /// <summary>
    /// Returns a string representation of the query.
    /// </summary>
    public override string ToString()
    {
        var goals = Goals.Aggregate("", (goals, structure) => $"{goals}{structure}, ");
        return goals.Length > 2 ? $"?- {goals[..^2]}." : "";
    }
}