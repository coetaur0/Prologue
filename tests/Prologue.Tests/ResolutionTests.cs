namespace Prologue.Tests;

public class ResolutionTests
{
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

        var substitution = new Dictionary<Variable, Term>();
        lhs.Unify(rhs, substitution);

        Assert.NotNull(substitution);

        Assert.Equal("f(a)", substitution[x].ToString());
        Assert.Equal("f(f(a))", substitution[y].ToString());
        Assert.Equal("f(a)", substitution[w].ToString());
        Assert.Equal("f(f(a))", substitution[z].ToString());
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

        var substitution = new Dictionary<Variable, Term>();

        Assert.False(lhs.Unify(rhs, substitution));
    }
}