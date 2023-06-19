namespace Prologue;

/// <summary>
/// A Prolog functor.
/// </summary>
/// <param name="Symbol">The functor's predicate symbol.</param>
/// <param name="Arity">The functor's arity.</param>
public sealed record Functor(string Symbol, int Arity)
{
    /// <summary>
    /// Returns a string representation of the functor.
    /// </summary>
    public override string ToString() => $"{Symbol}/{Arity}";
}