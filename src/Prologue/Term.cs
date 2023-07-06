namespace Prologue;

/// <summary>
/// A Prolog term.
/// </summary>
public abstract record Term
{
    /// <summary>
    /// Returns the set of variables names appearing in the term.
    /// </summary>
    public abstract HashSet<string> Variables { get; }

    /// <summary>
    /// Applies a substitution to the term.
    /// </summary>
    /// <returns>The new term obtained after applying the substitution to its variables.</returns>
    public abstract Term Apply(IDictionary<string, Term> substitution);

    /// <summary>
    /// Unifies the term with another one and extends the input substitution with their most general unifier.
    /// </summary>
    /// <returns>True if the terms unify, false otherwise.</returns>
    public abstract bool Unify(Term other, IDictionary<string, Term> substitution);

    /// <summary>
    /// Renames the variables in the term by appending their names with some index. 
    /// </summary>
    /// <returns>The new term obtained after renaming its variables.</returns>
    internal abstract Term Rename(int index);
}