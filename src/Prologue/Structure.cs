using System.Text;

namespace Prologue;

/// <summary>
/// A Prolog structure.
/// </summary>
public sealed record Structure : Term
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
    /// Indicates if the structure is ground (it doesn't contain any variables).
    /// </summary>
    public bool Ground { get; }

    public override HashSet<string> Variables { get; }

    public Structure(string symbol, Term[] arguments)
    {
        Functor = new Functor(symbol, arguments.Length);
        Arguments = arguments;
        Variables = Arguments.Aggregate(new HashSet<string>(), (set, term) => set.Union(term.Variables).ToHashSet());
        Ground = !Variables.Any();
    }

    /// <summary>
    /// Applies a substitution to the structure.
    /// </summary>
    /// <returns>The new structure obtained after substituting its variables.</returns>
    public override Structure Apply(IDictionary<string, Term> substitution) => Ground
        ? this
        : new Structure(Functor.Symbol, Arguments.Select(argument => argument.Apply(substitution)).ToArray());

    public override bool Unify(Term other, IDictionary<string, Term> substitution)
    {
        if (this == other)
        {
            return true;
        }

        switch (other)
        {
            case Structure structure:
                if (Functor != structure.Functor)
                {
                    return false;
                }

                if (Arguments.Where((t, i) => !t
                        .Apply(substitution)
                        .Unify(structure.Arguments[i].Apply(substitution), substitution)).Any())
                {
                    return false;
                }

                foreach (var (variable, term) in substitution)
                {
                    substitution[variable] = term.Apply(substitution);
                }

                break;

            case Variable variable:
                substitution[variable.Name] = Apply(substitution);
                break;
        }

        return true;
    }

    /// <summary>
    /// Returns a string representation of the structure.
    /// </summary>
    public override string ToString()
    {
        var str = new StringBuilder($"{Functor.Symbol}");

        if (Arguments.Length == 0)
        {
            return str.ToString();
        }

        str.Append('(');
        foreach (var argument in Arguments)
        {
            str.Append($"{argument}, ");
        }

        str.Remove(str.Length - 2, 2);
        str.Append(')');
        return str.ToString();
    }

    /// <summary>
    /// Renames all the variables in the structure by appending their identifiers with some index.
    /// </summary>
    /// <returns>A new structure where all variables have been renamed.</returns>
    internal override Structure Rename(int index) =>
        new(Functor.Symbol, Arguments.Select(argument => argument.Rename(index)).ToArray());
}