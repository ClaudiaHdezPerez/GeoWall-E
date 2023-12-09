using System.Runtime.InteropServices;

namespace G_Sharp;

public static class LexingSupplies
{
    private static readonly Dictionary<string, SyntaxKind> keywordKind = new()
    {
        ["and"]       = SyntaxKind.AndKeyword,
        ["or"]        = SyntaxKind.OrKeyword,
        ["not"]       = SyntaxKind.NotKeyword,
        ["let"]       = SyntaxKind.LetKeyword,
        ["in"]        = SyntaxKind.InKeyword,
        ["if"]        = SyntaxKind.IfKeyword,
        ["else"]      = SyntaxKind.ElseKeyword,
        ["then"]      = SyntaxKind.ThenKeyword,
        ["point"]     = SyntaxKind.GeometryKeyword,
        ["segment"]   = SyntaxKind.GeometryKeyword,
        ["ray"]       = SyntaxKind.GeometryKeyword,
        ["line"]      = SyntaxKind.GeometryKeyword,
        ["circle"]    = SyntaxKind.GeometryKeyword,
        ["arc"]       = SyntaxKind.GeometryKeyword,
        ["measure"]   = SyntaxKind.GeometryKeyword,
        ["count"]     = SyntaxKind.GeometryKeyword,
        ["intersect"] = SyntaxKind.GeometryKeyword,
        ["color"]     = SyntaxKind.ColorKeyword,
        ["blue"]      = SyntaxKind.ColorToken,
        ["red"]       = SyntaxKind.ColorToken,
        ["yellow"]    = SyntaxKind.ColorToken,
        ["green"]     = SyntaxKind.ColorToken,
        ["cyan"]      = SyntaxKind.ColorToken,
        ["magenta"]   = SyntaxKind.ColorToken,
        ["white"]     = SyntaxKind.ColorToken,
        ["gray"]      = SyntaxKind.ColorToken,
        ["black"]     = SyntaxKind.ColorToken,
        ["draw"]      = SyntaxKind.DrawKeyword,
        ["restore"]   = SyntaxKind.RestoreKeyword,
        ["sequence"]  = SyntaxKind.SequenceKeyword,
        ["import"]    = SyntaxKind.ImportKeyword,
        ["PI"]        = SyntaxKind.MathToken,
        ["E"]         = SyntaxKind.MathToken,
        ["undefined"] = SyntaxKind.UndefinedToken,
    };


    public static readonly Dictionary<char, Func<int, int, char, (SyntaxToken, int)>> LexMathCharacters = new()
    {
        ['+'] = (position, line, _) => (new SyntaxToken(SyntaxKind.PlusToken, line, position, "+", null!), ++position),
        ['-'] = (position, line, _) => (new SyntaxToken(SyntaxKind.MinusToken, line, position, "-", null!), ++position),
        ['*'] = (position, line, _) => (new SyntaxToken(SyntaxKind.MultToken, line, position, "*", null!), ++position),
        ['/'] = (position, line, _) => (new SyntaxToken(SyntaxKind.DivisionToken, line, position, "/", null!), ++position),
        ['%'] = (position, line, _) => (new SyntaxToken(SyntaxKind.ModToken, line, position, "%", null!), ++position),
        ['('] = (position, line, _) => (new SyntaxToken(SyntaxKind.OpenParenthesisToken, line, position, "(", null!), ++position),
        [')'] = (position, line, _) => (new SyntaxToken(SyntaxKind.ClosedParenthesisToken, line, position, ")", null!), ++position),
        ['{'] = (position, line, _) => (new SyntaxToken(SyntaxKind.OpenCurlyBracketToken, line, position, "{", null!), ++position),
        ['}'] = (position, line, _) => (new SyntaxToken(SyntaxKind.ClosedCurlyBracketToken, line, position, "}", null!), ++position),
        [';'] = (position, line, _) => (new SyntaxToken(SyntaxKind.SemicolonToken, line, position, ";", null!), ++position),
        [','] = (position, line, _) => (new SyntaxToken(SyntaxKind.SeparatorToken, line, position, ",", null!), ++position),
        ['>'] = LexGreaterThanChar,
        ['<'] = LexLessThanChar,
        ['='] = LexEqualsChar,
        ['!'] = LexDifferentChar
    };

    public static SyntaxKind GetKeywordKind(string token)
    {
        if (keywordKind.TryGetValue(token, out SyntaxKind value))
            return value;

        return SyntaxKind.IdentifierToken;
    }

    private static (SyntaxToken, int) LexGreaterThanChar(int pos, int line, char NextCurrent)
    {
        if (NextCurrent == '=')
                return (new SyntaxToken(SyntaxKind.GreaterOrEqualToken, line, pos, ">=", null!), pos + 2);
        return (new SyntaxToken(SyntaxKind.GreaterToken, line, pos, ">", null!), ++pos);
    }

    private static (SyntaxToken, int) LexLessThanChar(int pos, int line, char NextCurrent)
    {
        if (NextCurrent == '=')
                return (new SyntaxToken(SyntaxKind.LessOrEqualToken, line, pos, "<=", null!), pos + 2);
        return (new SyntaxToken(SyntaxKind.LessToken,   line, pos, "<", null!), ++pos);
    }

    private static (SyntaxToken, int) LexEqualsChar(int pos, int line, char NextCurrent)
    {
        if (NextCurrent == '=')
                return (new SyntaxToken(SyntaxKind.EqualToken, line, pos, "==", null!), pos + 2);
        return (new SyntaxToken(SyntaxKind.AssignmentToken, line, pos, "=", null!), ++pos);
    }
    private static (SyntaxToken, int) LexDifferentChar(int pos, int line, char NextCurrent)
    {
        if (NextCurrent == '=')
            return (new SyntaxToken(SyntaxKind.DifferentToken, line, pos, "!=", null!), pos + 2);

        Error.SetError("LEXICAL", $"Line '{line}' : Unexpected character '!'");
        return (new SyntaxToken(SyntaxKind.ErrorToken!, line, pos, "", null!), ++pos);
    }
}