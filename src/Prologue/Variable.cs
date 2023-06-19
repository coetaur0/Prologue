namespace Prologue;

/// <summary>
/// A Prolog variable.
/// </summary>
public sealed class Variable : Term
{
    /// <summary>
    /// The variable's name.
    /// </summary>
    public string Name { get; }

    public Variable(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Returns a string representation of the variable.
    /// </summary>
    public override string ToString() => Name;
}