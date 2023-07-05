namespace Prologue;

/// <summary>
/// A Prolog variable.
/// </summary>
public sealed record Variable(string Name) : Term
{
    public override Term Apply(IDictionary<string, Term> substitution) =>
        substitution.TryGetValue(Name, out var value) ? value : this;

    /// <summary>
    /// Returns a string representation of the variable.
    /// </summary>
    public override string ToString() => Name;

    public override bool Unify(Term other, IDictionary<string, Term> substitution)
    {
        if (this == other)
            return true;

        substitution.Add(Name, other.Apply(substitution));

        return true;
    }
}