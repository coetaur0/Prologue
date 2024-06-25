using System.Text;

namespace Prologue;

/// <summary>
/// A Prolog definite clause.
/// </summary>
public sealed record Clause(Structure Head, Structure[] Body)
{
    /// <summary>
    /// Returns a string representation of the clause.
    /// </summary>
    public override string ToString()
    {
        var str = new StringBuilder($"{Head}");

        if (Body.Length == 0)
        {
            str.Append('.');
            return str.ToString();
        }

        str.Append(" :- ");
        foreach (var structure in Body)
        {
            str.Append($"{structure}, ");
        }

        str.Remove(str.Length - 2, 2);
        str.Append('.');
        return str.ToString();
    }

    /// <summary>
    /// Renames all the variables in the clause by appending their names with some index.
    /// </summary>
    /// <returns>A new clause where all the variables have been renamed.</returns>
    internal Clause Rename(int index) => new(Head.Rename(index), Body.Select(goal => goal.Rename(index)).ToArray());
}