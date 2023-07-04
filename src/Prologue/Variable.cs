namespace Prologue;

/// <summary>
/// A Prolog variable.
/// </summary>
public sealed class Variable : Term
{
    /// <summary>
    /// The variable's name.
    /// </summary>
    public string Name { get; }

    public Variable(string name)
    {
        Name = name;
    }

    public override Term Apply(IDictionary<Variable, Term> substitution) =>
        substitution.TryGetValue(this, out var value) ? value : this;

    /// <summary>
    /// Returns a string representation of the variable.
    /// </summary>
    public override string ToString() => Name;

    public override bool Unify(Term other, IDictionary<Variable, Term> substitution)
    {
        if (this == other)
            return true;

        substitution.Add(this, other.Apply(substitution));

        return true;
    }
}