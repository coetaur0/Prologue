namespace Prologue;

/// <summary>
/// A Prolog knowledge base.
/// </summary>
public sealed class KnowledgeBase
{
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
    /// Adds a new clause to the knowledge base.
    /// </summary>
    public void Add(Clause clause)
    {
        if (_predicates.TryGetValue(clause.Head.Functor, out var clauses))
            clauses.Add(clause);
        else
            _predicates[clause.Head.Functor] = new List<Clause>() { clause };
    }
}