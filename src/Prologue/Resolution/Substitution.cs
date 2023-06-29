using System.Collections;

namespace Prologue.Resolution;

/// <summary>
/// A substitution mapping Prolog variables to terms.
/// </summary>
public sealed class Substitution : IEnumerable<(Variable, Term)>
{
    /// <summary>
    /// Returns the term bound to a variable in the substitution, or null if the variable is unbound.
    /// </summary>
    public Term? this[Variable variable] => _bindings.TryGetValue(variable, out var value) ? value : null;

    /// <summary>
    /// The substitution's bindings.
    /// </summary>
    private readonly Dictionary<Variable, Term> _bindings;

    public Substitution()
    {
        _bindings = new Dictionary<Variable, Term>();
    }

    private Substitution(IDictionary<Variable, Term> bindings)
    {
        _bindings = new Dictionary<Variable, Term>(bindings);
    }

    /// <summary>
    /// Adds a new binding between a variable and a term to the substitution.
    /// </summary>
    public void Add(Variable variable, Term term) => _bindings.Add(variable, term);

    /// <summary>
    /// Returns a new copy of the substitution.
    /// </summary>
    public Substitution Clone() => new(_bindings);

    /// <summary>
    /// Applies the substitution to a term.
    /// </summary>
    public Term Apply(Term term) => term switch
    {
        Structure structure => structure.Ground
            ? structure
            : new Structure(structure.Functor.Symbol, structure.Arguments.Select(Apply).ToArray()),
        Variable variable => _bindings.TryGetValue(variable, out var value) ? value : variable,
        _ => term
    };

    /// <summary>
    /// Applies some substitution to every term in the substitution.
    /// </summary>
    public void Update(Substitution substitution)
    {
        foreach (var (variable, term) in _bindings)
            _bindings[variable] = substitution.Apply(term);
    }

    public IEnumerator<(Variable, Term)> GetEnumerator()
    {
        foreach (var (variable, term) in _bindings)
            yield return (variable, term);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}