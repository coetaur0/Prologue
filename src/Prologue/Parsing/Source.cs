namespace Prologue.Parsing;

/// <summary>
/// A source of Prolog code.
/// </summary>
internal sealed class Source
{
    /// <summary>
    /// The source's path.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Returns the character at some offset in the source's contents.
    /// </summary>
    public char this[int offset] => _contents[offset];

    /// <summary>
    /// Returns the contents covered by some range in the source.
    /// </summary>
    public string this[Range range] => this[range.Start.Offset, range.End.Offset];

    /// <summary>
    /// Returns the contents between two offsets in the source.
    /// </summary>
    private string this[int start, int end]
    {
        get
        {
            var offset = Math.Max(0, Math.Min(start, _contents.Length));
            var length = Math.Min(end, _contents.Length) - offset;
            return _contents.Substring(offset, length);
        }
    }

    /// <summary>
    /// Returns the source's length.
    /// </summary>
    public int Length => _contents.Length;

    /// <summary>
    /// The source's contents.
    /// </summary>
    private readonly string _contents;

    public Source(string path, string contents)
    {
        Path = path;
        _contents = contents;
    }
}