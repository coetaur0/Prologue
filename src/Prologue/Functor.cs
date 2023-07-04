namespace Prologue;

/// <summary>
/// A Prolog functor.
/// </summary>
public sealed record Functor(string Symbol, int Arity)
{
    /// <summary>
    /// Returns a string representation of the functor.
    /// </summary>
    public override string ToString() => $"{Symbol}/{Arity}";
}