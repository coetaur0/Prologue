namespace Prologue.Tests;

public class ResolutionTests
{
    [Fact]
    public void SolveQueries()
    {
        var knowledgeBase = KnowledgeBase.FromString(
            "parent(tom, lucy).\n" +
            "parent(laura, lucy).\n" +
            "parent(lucy, james).\n" +
            "parent(james, anne).\n" +
            "ancestor(X, Y) :- parent(X, Y).\n" +
            "ancestor(X, Y) :- parent(X, Z), ancestor(Z, Y)."
        );

        var query = Query.Load("ancestor(X, anne).");

        var solutions = knowledgeBase.Solve(query).ToArray();

        Assert.Equal(4, solutions.Length);

        Assert.Equal("james", solutions[0]["X"].ToString());
        Assert.Equal("tom", solutions[1]["X"].ToString());
        Assert.Equal("laura", solutions[2]["X"].ToString());
        Assert.Equal("lucy", solutions[3]["X"].ToString());

        Assert.Equal(9, knowledgeBase.Solve(Query.Load("ancestor(X, Y).")).ToArray().Length);

        Assert.Empty(knowledgeBase.Solve(Query.Load("ancestor(X, tom).")).ToArray());
        Assert.Empty(knowledgeBase.Solve(Query.Load("a.")).ToArray());
    }

    [Fact]
    public void UnifyTerms()
    {
        var w = new Variable("W");
        var z = new Variable("Z");
        // LHS = p(Z, h(Z, W), f(W)).
        var lhs = new Structure(
            "p",
            new Term[] { z, new Structure("h", new Term[] { z, w }), new Structure("f", new Term[] { w }) }
        );

        var x = new Variable("X");
        var y = new Variable("Y");
        // RHS = p(f(X), h(Y, f(a)), Y).
        var rhs = new Structure(
            "p",
            new Term[]
            {
                new Structure("f", new Term[] { x }),
                new Structure("h",
                    new Term[] { y, new Structure("f", new Term[] { new Structure("a", Array.Empty<Term>()) }) }),
                y
            }
        );

        var substitution = new Dictionary<string, Term>();

        Assert.True(lhs.Unify(rhs, substitution));

        Assert.Equal("f(a)", substitution["X"].ToString());
        Assert.Equal("f(f(a))", substitution["Y"].ToString());
        Assert.Equal("f(a)", substitution["W"].ToString());
        Assert.Equal("f(f(a))", substitution["Z"].ToString());
    }

    [Fact]
    public void FailToUnifyTerms()
    {
        var z = new Variable("Z");
        // LHS = p(f(a), Z, Z).
        var lhs = new Structure(
            "p",
            new Term[] { new Structure("f", new Term[] { new Structure("a", Array.Empty<Term>()) }), z, z }
        );

        var x = new Variable("X");
        var y = new Variable("Y");
        // RHS = p(X, h(a, Y), X).
        var rhs = new Structure(
            "p",
            new Term[] { x, new Structure("h", new Term[] { new Structure("a", Array.Empty<Term>()), y }), x }
        );

        var substitution = new Dictionary<string, Term>();

        Assert.False(lhs.Unify(rhs, substitution));
    }
}