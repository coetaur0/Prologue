using Prologue.Parsing;

namespace Prologue;

/// <summary>
/// A Prolog query.
/// </summary>
public sealed record Query
{
    /// <summary>
    /// The query's goals.
    /// </summary>
    public Structure[] Goals { get; }

    /// <summary>
    /// Returns the set of variable names appearing in the query.
    /// </summary>
    public HashSet<string> Variables { get; }

    public Query(Structure[] goals)
    {
        Goals = goals;
        Variables =
            Goals.Aggregate(new HashSet<string>(), (set, structure) => set.Union(structure.Variables).ToHashSet());
    }

    /// <summary>
    /// Loads a query from a string and returns it.
    /// </summary>
    public static Query Load(string input)
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