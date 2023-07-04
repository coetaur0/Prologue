namespace Prologue;

/// <summary>
/// A Prolog term.
/// </summary>
public abstract class Term
{
    /// <summary>
    /// Applies a substitution to the term.
    /// </summary>
    /// <returns>The new term obtained after applying the substitution to the variables in the original term.</returns>
    public abstract Term Apply(IDictionary<Variable, Term> substitution);

    /// <summary>
    /// Unifies the term with another one and records their most general unifier in a substitution.
    /// </summary>
    /// <returns>True if the terms unify, false otherwise.</returns>
    public abstract bool Unify(Term other, IDictionary<Variable, Term> substitution);
}