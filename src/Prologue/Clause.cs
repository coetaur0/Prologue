namespace Prologue;

/// <summary>
/// A Prolog definite clause.
/// </summary>
public sealed class Clause
{
    /// <summary>
    /// The clause's head.
    /// </summary>
    public Structure Head { get; }

    /// <summary>
    /// The clause's body.
    /// </summary>
    public Structure[] Body { get; }

    public Clause(Structure head, Structure[] body)
    {
        Head = head;
        Body = body;
    }

    /// <summary>
    /// Returns a string representation of the clause.
    /// </summary>
    public override string ToString()
    {
        var body = Body.Aggregate("", (body, structure) => $"{body}{structure}, ");
        return body.Length > 2 ? $"{Head} :- {body[..^2]}." : $"{Head}.";
    }
}