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

    private readonly Dictionary<Functor, List<Clause>> _predicates = new();

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
    /// Loads a knowledge base from a file and returns it.
    /// </summary>
    public static KnowledgeBase FromFile(string path)
    {
        var input = System.IO.File.ReadAllText(path);
        var source = new Source(path, input);
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
    /// Solves a query, given the knowledge base's predicates.
    /// </summary>
    /// <returns>The sequence of solution substitutions for the query's variables.</returns>
    public IEnumerable<IDictionary<string, Term>> Solve(Query query)
    {
        if (query.Goals.Length == 0)
            yield break;

        var resolvent = query.Goals.Select(goal => goal.Rename(0)).ToArray();

        foreach (var substitution in Prove(resolvent, new Dictionary<string, Term>(), 1))
        {
            var solution =
                query.Variables.ToDictionary(variable => variable, variable => substitution[$"{variable}_0"]);

            yield return solution;
        }
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

    /// <summary>
    /// Proves a sequence of goals (the resolvent) given the knowledge base's predicates and a substitution.
    /// </summary>
    /// <returns>The sequence of solution substitutions for the resolvent.</returns>
    private IEnumerable<IDictionary<string, Term>> Prove(
        IReadOnlyCollection<Structure> resolvent,
        IDictionary<string, Term> substitution,
        int level
    )
    {
        if (resolvent.Count == 0)
        {
            yield return substitution;
            yield break;
        }

        var goal = resolvent.First();
        if (!_predicates.TryGetValue(goal.Functor, out var clauses))
            yield break;

        foreach (var clause in clauses.Select(clause => clause.Rename(level)))
        {
            var newSubstitution = new Dictionary<string, Term>(substitution);

            if (!goal.Unify(clause.Head, newSubstitution))
                continue;

            var newResolvent = clause.Body.Concat(resolvent.Skip(1)).ToArray();
            foreach (var solution in Prove(newResolvent, newSubstitution, level + 1))
                yield return solution;
        }
    }
}