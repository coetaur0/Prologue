namespace Prologue;

/// <summary>
/// A Prolog structure.
/// </summary>
public sealed class Structure : Term
{
    /// <summary>
    /// The structure's functor.
    /// </summary>
    public Functor Functor { get; }

    /// <summary>
    /// The structure's arguments.
    /// </summary>
    public Term[] Arguments { get; }

    /// <summary>
    /// Indicates whether the structure is a ground term or not.
    /// </summary>
    public bool Ground { get; }

    public Structure(string symbol, Term[] arguments)
    {
        Functor = new Functor(symbol, arguments.Length);
        Arguments = arguments;
        Ground = Arguments.All(arg => arg switch { Structure structure => structure.Ground, _ => false });
    }

    /// <summary>
    /// Returns a string representation of the structure.
    /// </summary>
    public override string ToString()
    {
        var args = Arguments.Aggregate("", (args, term) => $"{args}{term}, ");
        return args.Length > 2 ? $"{Functor.Symbol}({args[..^2]})" : Functor.Symbol;
    }
}