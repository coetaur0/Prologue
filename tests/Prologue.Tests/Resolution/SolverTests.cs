using Prologue.Resolution;

namespace Prologue.Tests.Resolution;

public class SolverTests
{
    [Fact]
    public void SucceedUnification()
    {
        var w = new Variable("W");
        var z = new Variable("Z");
        var lhs = new Structure(
            "p",
            new Term[] { z, new Structure("h", new Term[] { z, w }), new Structure("f", new Term[] { w }) }
        );

        var x = new Variable("X");
        var y = new Variable("Y");
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

        var substitution = new Substitution();
        Assert.True(Solver.Unify(lhs, rhs, substitution));

        Assert.Equal("f(a)", substitution[x]!.ToString());
        Assert.Equal("f(f(a))", substitution[y]!.ToString());
        Assert.Equal("f(a)", substitution[w]!.ToString());
        Assert.Equal("f(f(a))", substitution[z]!.ToString());
    }

    [Fact]
    public void FailUnification()
    {
        var z = new Variable("Z");
        var lhs = new Structure(
            "p",
            new Term[] { new Structure("f", new Term[] { new Structure("a", Array.Empty<Term>()) }), z, z }
        );

        var x = new Variable("X");
        var y = new Variable("Y");
        var rhs = new Structure(
            "p",
            new Term[] { x, new Structure("h", new Term[] { new Structure("a", Array.Empty<Term>()), y }), x }
        );

        var substitution = new Substitution();
        Assert.False(Solver.Unify(lhs, rhs, substitution));
    }
}