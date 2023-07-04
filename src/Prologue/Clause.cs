namespace Prologue;

/// <summary>
/// A Prolog definite clause.
/// </summary>
public sealed record Clause(Structure Head, Structure[] Body)
{
    /// <summary>
    /// Returns a string representation of the clause.
    /// </summary>
    public override string ToString()
    {
        var body = Body.Aggregate("", (body, structure) => $"{body}{structure}, ");
        return body.Length > 2 ? $"{Head} :- {body[..^2]}." : $"{Head}.";
    }
}