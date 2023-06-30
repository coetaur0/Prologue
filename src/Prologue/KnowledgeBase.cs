using Prologue.Parsing;

namespace Prologue;

/// <summary>
/// A Prolog knowledge base.
/// </summary>
public sealed class KnowledgeBase
{
    /// <summary>
    /// Returns the clauses associated with a given functor in the knowledge base.
    /// </summary>
    public List<Clause> this[Functor functor] => _predicates[functor];

    /// <summary>
    /// The knowledge base's predicates (or procedures).
    /// </summary>
    private readonly Dictionary<Functor, List<Clause>> _predicates;

    public KnowledgeBase()
    {
        _predicates = new Dictionary<Functor, List<Clause>>();
    }

    /// <summary>
    /// Loads a knowledge base from a string and returns it.
    /// </summary>
    public static KnowledgeBase FromString(string input)
    {
        var source = new Source("input", input);
        var parser = new Parser(source);
        return parser.ParseKnowledgeBase();
    }

    /// <summary>
    /// Adds a new clause to the knowledge base.
    /// </summary>
    public void Add(Clause clause)
    {
        if (_predicates.TryGetValue(clause.Head.Functor, out var clauses))
            clauses.Add(clause);
        else
            _predicates[clause.Head.Functor] = new List<Clause> { clause };
    }

    /// <summary>
    /// Returns a string representation of the knowledge base.
    /// </summary>
    public override string ToString()
    {
        var predicates = "";

        foreach (var (_, clauses) in _predicates)
            predicates = $"{predicates}{clauses.Aggregate("", (predicate, clause) => $"{predicate}{clause}\n")}\n";

        return predicates;
    }
}