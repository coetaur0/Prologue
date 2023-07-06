namespace Prologue;

/// <summary>
/// A Prolog variable.
/// </summary>
public sealed record Variable(string Name) : Term
{
    public override HashSet<string> Variables => new() { Name };

    /// <summary>
    /// Applies a substitution to the variable.
    /// </summary>
    /// <returns>The value bound to the variable in the substitution, or the variable itself if it is unbound.</returns>
    public override Term Apply(IDictionary<string, Term> substitution) =>
        substitution.TryGetValue(Name, out var value) ? value : this;

    public override bool Unify(Term other, IDictionary<string, Term> substitution)
    {
        if (this == other)
            return true;

        substitution.Add(this.Name, other.Apply(substitution));

        return true;
    }

    /// <summary>
    /// Returns a string representation of the variable.
    /// </summary>
    public override string ToString() => Name;

    /// <summary>
    /// Renames the variable by appending its identifier with some index.
    /// </summary>
    /// <returns>A new renamed variable.</returns>
    internal override Variable Rename(int index) => new($"{Name}_{index}");
}