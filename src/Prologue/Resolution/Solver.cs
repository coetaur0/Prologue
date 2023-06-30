namespace Prologue.Resolution;

/// <summary>
/// A Prolog query solver.
/// </summary>
internal sealed class Solver
{
    /// <summary>
    /// Performs unification on a pair of Prolog terms.
    /// </summary>
    public static bool Unify(Term lhs, Term rhs, Substitution substitution)
    {
        if (lhs == rhs)
            return true;

        switch (substitution.Apply(lhs), substitution.Apply(rhs))
        {
            case (Structure leftStruct, Structure rightStruct):
                if (leftStruct.Functor != rightStruct.Functor)
                    return false;

                for (var i = 0; i < leftStruct.Arguments.Length; i++)
                    if (!Unify(
                            substitution.Apply(leftStruct.Arguments[i]),
                            substitution.Apply(rightStruct.Arguments[i]),
                            substitution
                        ))
                        return false;

                substitution.Update(substitution);
                break;

            case (Variable leftVar, _):
                substitution.Add(leftVar, substitution.Apply(rhs));
                break;

            case (_, Variable rightVar):
                substitution.Add(rightVar, substitution.Apply(lhs));
                break;
        }

        return true;
    }
}