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
    /// Returns the set of variables in the query.
    /// </summary>
    public IEnumerable<Variable> Variables { get; }

    public Query(Structure[] goals)
    {
        Goals = goals;
        Variables = goals.SelectMany(goal => goal.Variables).ToHashSet();
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