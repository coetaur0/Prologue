using Prologue.Resolution;

namespace Prologue;

/// <summary>
/// A Prolog term.
/// </summary>
public abstract class Term
{
    /// <summary>
    /// Performs unification between the term and another one and returns their most general unifier if it succeeds, or
    /// null otherwise.
    /// </summary>
    public Substitution? Unify(Term other)
    {
        var substitution = new Substitution();
        return Solver.Unify(this, other, substitution) ? substitution : null;
    }
}