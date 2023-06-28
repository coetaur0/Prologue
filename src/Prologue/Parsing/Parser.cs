namespace Prologue.Parsing;

/// <summary>
/// A Prolog parser.
/// </summary>
public sealed class Parser
{
    /// <summary>
    /// The source being parsed.
    /// </summary>
    private readonly Source _source;

    /// <summary>
    /// The lexer used to extract tokens from the source.
    /// </summary>
    private readonly Lexer _lexer;

    /// <summary>
    /// The next token in the source being parsed.
    /// </summary>
    private Token _nextToken;

    /// <summary>
    /// The diagnostics encountered while parsing the source.
    /// </summary>
    private readonly List<Diagnostic> _diagnostics;

    /// <summary>
    /// Indicates if the parser is in panic mode or not. 
    /// </summary>
    private bool _panicMode;

    public Parser(Source source)
    {
        _source = source;
        _lexer = new Lexer(source);
        _nextToken = _lexer.Next()!;
        _diagnostics = new List<Diagnostic>();
        _panicMode = false;
    }

    /// <summary>
    /// Parses a Prolog program.
    /// </summary>
    public (KnowledgeBase, List<Diagnostic>) ParseProgram()
    {
        var program = new KnowledgeBase();

        var syncTokens = new[] { TokenKind.Symbol };
        while (_nextToken.Kind != TokenKind.Eof)
        {
            var clause = ParseClause();
            if (clause is not null)
                program.Add(clause);

            Synchronize(syncTokens);
        }

        return (program, _diagnostics);
    }

    /// <summary>
    /// Parses a Prolog query.
    /// </summary>
    public (Query, List<Diagnostic>) ParseQuery()
    {
        var goals = new List<Structure>();
        ParseList(ref goals, ParseStructure, TokenKind.Period);

        Consume(TokenKind.Period, "expect a '.' at the end of a query");

        return (new Query(goals.ToArray()), _diagnostics);
    }

    /// <summary>
    /// Parses a Prolog clause.
    /// </summary>
    private Clause? ParseClause()
    {
        var head = ParseStructure();
        if (head is null)
            return null;

        var body = new List<Structure>();
        if (_nextToken.Kind == TokenKind.Neck)
        {
            Advance();
            ParseList(ref body, ParseStructure, TokenKind.Period);
        }

        Consume(TokenKind.Period, "expect a '.' at the end of a clause");

        return new Clause(head, body.ToArray());
    }

    /// <summary>
    /// Parses a Prolog term.
    /// </summary>
    private Term? ParseTerm()
    {
        switch (_nextToken.Kind)
        {
            case TokenKind.Symbol:
                return ParseStructure();

            case TokenKind.Variable:
                return ParseVariable();

            default:
                EmitDiagnostic("expect a Prolog term", _nextToken.Range);
                return null;
        }
    }

    /// <summary>
    /// Parses a Prolog structure.
    /// </summary>
    private Structure? ParseStructure()
    {
        var symbol = Consume(TokenKind.Symbol, "expect a symbol");
        if (symbol is null)
            return null;

        var arguments = new List<Term>();
        if (_nextToken.Kind == TokenKind.LeftParen)
        {
            Advance();
            ParseList(ref arguments, ParseTerm, TokenKind.RightParen);
            Consume(TokenKind.RightParen, "expect a ')' at the end of a structure's arguments list");
        }

        return new Structure(_source[symbol.Range], arguments.ToArray());
    }

    /// <summary>
    /// Parses a Prolog variable.
    /// </summary>
    private Variable? ParseVariable()
    {
        var variable = Consume(TokenKind.Variable, "expect a variable");
        return variable is null ? null : new Variable(_source[variable.Range]);
    }

    /// <summary>
    /// Parses a comma separated list of elements ending with some specific terminator token and returns it.
    /// </summary>
    private void ParseList<T>(ref List<T> elements, Func<T?> parseFunction, TokenKind terminator)
    {
        var syncTokens = new[] { TokenKind.Comma, terminator };
        while (_nextToken.Kind != terminator)
        {
            var element = parseFunction();
            if (element is not null)
                elements.Add(element);

            Synchronize(syncTokens);

            if (_nextToken.Kind == TokenKind.Comma)
                Advance();
            else
                break;
        }
    }

    /// <summary>
    /// Advances the parser's position in the source being parsed.
    /// </summary>
    private Token Advance()
    {
        var token = _nextToken;

        if (_nextToken.Kind != TokenKind.Eof)
            _nextToken = _lexer.Next()!;

        return token;
    }

    /// <summary>
    /// Emits a new compiler diagnostic with some given error message and range if the parser isn't in panic mode.
    /// </summary>
    private void EmitDiagnostic(string message, Range range)
    {
        if (_panicMode)
            return;

        _panicMode = true;
        _diagnostics.Add(new Diagnostic(message, range));
    }

    /// <summary>
    /// Consumes the next token in the source if it is of the specified kind, or emits a compiler diagnostic with some
    /// given message otherwise.
    /// </summary>
    private Token? Consume(TokenKind tokenKind, string message)
    {
        if (_nextToken.Kind == tokenKind)
            return Advance();

        EmitDiagnostic(message, _nextToken.Range);
        return null;
    }

    /// <summary>
    /// Synchronizes the parser at the position of the next token with one of the specified kinds.
    /// </summary>
    private void Synchronize(TokenKind[] tokenKinds)
    {
        if (!_panicMode)
            return;

        while (_nextToken.Kind != TokenKind.Eof && !tokenKinds.Contains(_nextToken.Kind))
            Advance();

        _panicMode = false;
    }
}