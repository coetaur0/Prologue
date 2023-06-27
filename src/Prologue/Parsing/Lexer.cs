namespace Prologue.Parsing;

/// <summary>
/// A lexical analyser.
/// </summary>
public sealed class Lexer
{
    /// <summary>
    /// The source being lexed.
    /// </summary>
    private readonly Source _source;

    /// <summary>
    /// The start location of the next token in the source.
    /// </summary>
    private Location _tokenStart;

    /// <summary>
    /// The line number of the lexer's current location in the source.
    /// </summary>
    private int _line;

    /// <summary>
    /// The column number of the lexer's current location in the source. 
    /// </summary>
    private int _column;

    /// <summary>
    /// The offset of the lexer's current location in the source.
    /// </summary>
    private int _offset;

    /// <summary>
    /// A boolean flag indicating if the lexer's stream of tokens is depleted (all tokens have been consumed).
    /// </summary>
    private bool _depleted;

    public Lexer(Source source)
    {
        _source = source;
        _tokenStart = new Location(1, 1, 0);
        _line = 1;
        _column = 1;
        _offset = 0;
        _depleted = false;
    }

    /// <summary>
    /// Returns the next token in the source.
    /// </summary>
    public Token? Next()
    {
        if (_depleted)
            return null;

        SkipNonTokens();
        _tokenStart = new Location(_line, _column, _offset);

        if (_offset >= _source.Length)
        {
            _depleted = true;
            return MakeToken(TokenKind.Eof);
        }

        TokenKind tokenKind;
        switch (_source[_offset])
        {
            case var nextChar when char.IsLetter(nextChar) || nextChar == '_':
                tokenKind = char.IsLower(nextChar) ? TokenKind.Symbol : TokenKind.Variable;
                Consume(c => char.IsLetter(c) || char.IsDigit(c) || c == '_');
                break;

            case ':':
                if (_offset + 1 < _source.Length && _source[_offset + 1] == '-')
                {
                    tokenKind = TokenKind.Neck;
                    Advance(2);
                }
                else
                {
                    tokenKind = TokenKind.Unknown;
                    Advance(1);
                }

                break;

            default:
                tokenKind = _source[_offset] switch
                {
                    '(' => TokenKind.LeftParen,
                    ')' => TokenKind.RightParen,
                    ',' => TokenKind.Comma,
                    '.' => TokenKind.Period,
                    _ => TokenKind.Unknown
                };
                Advance(1);
                break;
        }

        return MakeToken(tokenKind);
    }

    /// <summary>
    /// Advances the lexer's position in the source by some offset.
    /// </summary>
    private void Advance(int offset)
    {
        for (var i = 0; i < offset; i++)
            if (_offset >= _source.Length)
                return;
            else
            {
                if (_source[_offset] == '\n')
                {
                    _line++;
                    _column = 1;
                }
                else
                    _column++;

                _offset++;
            }
    }

    /// <summary>
    /// Consumes characters from the source as long as they satisfy some predicate.
    /// </summary>
    private void Consume(Predicate<char> predicate)
    {
        while (_offset < _source.Length && predicate(_source[_offset]))
        {
            if (_source[_offset] == '\n')
            {
                _line++;
                _column = 1;
            }
            else
                _column++;

            _offset++;
        }
    }

    /// <summary>
    /// Returns a new token of a given type.
    /// </summary>
    private Token MakeToken(TokenKind tokenKind) =>
        new(tokenKind, new(_tokenStart, new(_line, _column, _offset)));

    /// <summary>
    /// Skips comments and whitespace in the source.
    /// </summary>
    private void SkipNonTokens()
    {
        Consume(char.IsWhiteSpace);

        while (_offset < _source.Length && _source[_offset] == '%')
        {
            Consume(c => c != '\n');
            Consume(char.IsWhiteSpace);
        }
    }
}