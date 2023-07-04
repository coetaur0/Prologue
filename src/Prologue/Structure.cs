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
    /// Returns the set of variables in the structure.
    /// </summary>
    public IEnumerable<Variable> Variables => Arguments.OfType<Variable>().ToHashSet();

    /// <summary>
    /// Indicates if the structure is ground or not.
    /// </summary>
    private bool Ground { get; }

    public Structure(string symbol, Term[] arguments)
    {
        Functor = new Functor(symbol, arguments.Length);
        Arguments = arguments;
        Ground = Arguments.All(arg => arg switch { Structure structure => structure.Ground, _ => false });
    }

    public override Term Apply(IDictionary<Variable, Term> substitution) => Ground
        ? this
        : new Structure(Functor.Symbol, Arguments.Select(arg => arg.Apply(substitution)).ToArray());

    /// <summary>
    /// Returns a string representation of the structure.
    /// </summary>
    public override string ToString()
    {
        var args = Arguments.Aggregate("", (args, term) => $"{args}{term}, ");
        return args.Length > 2 ? $"{Functor.Symbol}({args[..^2]})" : Functor.Symbol;
    }

    public override bool Unify(Term other, IDictionary<Variable, Term> substitution)
    {
        if (this == other)
            return true;

        switch (other.Apply(substitution))
        {
            case Structure structure:
                if (Functor != structure.Functor)
                    return false;

                for (var i = 0; i < Arguments.Length; i++)
                    if (!Arguments[i].Apply(substitution)
                            .Unify(structure.Arguments[i].Apply(substitution), substitution))
                        return false;

                foreach (var (variable, term) in substitution)
                    substitution[variable] = term.Apply(substitution);

                break;

            case Variable variable:
                substitution.Add(variable, Apply(substitution));
                break;
        }

        return true;
    }
}