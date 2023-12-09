using System.Runtime.CompilerServices;

namespace G_Sharp;

internal sealed class Lexer
{
    private int line = 1;
    private int position = 0;

    private string Text { get; }
    private Dictionary<char, Func<SyntaxToken>> LexSpecialChars { get; }
    private char Current => LookAhead(0);
    private char NextCurrent => LookAhead(1);

    public Lexer(string text)
    {
        Text = text;

        LexSpecialChars = new()
        {
            ['\r'] = LexEndOfLine,
            ['\n'] = () => new SyntaxToken(SyntaxKind.WhitespaceToken, line, position++, " ", null!),
            ['"']  = LexStrings,
            ['/']  = LexComments,
        };
    }


    private char LookAhead(int offset)
    {
        var index = position + offset;
        if (index >= Text!.Length)
            return '\0';

        return Text[index];
    }

    private void Next()
    {
        position++;
    }

    public SyntaxToken Lex()
    {
        if (position >= Text!.Length)
            return new SyntaxToken(SyntaxKind.EndOfFileToken, line, position, "end of file", null!);

        else if (LexSpecialChars.TryGetValue(Current, out Func<SyntaxToken>? value))
            return value();

        else if (IsGroupOfChars(out SyntaxToken outToken)) 
            return outToken; 

        else if (LexingSupplies.LexMathCharacters.TryGetValue(Current, out Func<int, int, char, (SyntaxToken, int)>? func))
        {
            (SyntaxToken token, int pos) = func(position, line, NextCurrent);

            if (token.Kind != SyntaxKind.ErrorToken)
                position = pos;
                return token;
        }

        Error.SetError("LEXICAL", $"Line '{line}': Unexpected character '{Current}'");
        return new SyntaxToken(SyntaxKind.ErrorToken!, line, position++, Text![position - 1].ToString(), null!);
    }

    private SyntaxToken LexStrings()
    {
        int start = position;
        int backSlashCount = 0;
        Next();

        while (Current != '"' || (Current == '"' && int.IsOddInteger(backSlashCount)))
        {
            if (LookAhead(1) == '\0')
            {
                Error.SetError("SYNTAX", $"Line '{line}': Undetermined string literal");
                return new SyntaxToken(SyntaxKind.ErrorToken!, line, position++, Text![position - 1].ToString(), null!);
            }

            if (Current == '\\')
                backSlashCount++;

            else backSlashCount = 0;

            if (Current == '\n') line++;

            Next();
        }

        Next();
        int length = position - start;
        string token = Text!.Substring(start, length);
        token = ParsingSupplies.BackSlashEval(token, line);

        if (Error.Wrong)
            return new SyntaxToken(SyntaxKind.ErrorToken, line, position++, token, null!);

        string value = token[1..^1];
        return new SyntaxToken(SyntaxKind.StringToken, line, start, token, value);
    }

    private SyntaxToken LexNumbers()
    {
        int start = position;

        while (char.IsDigit(Current) || Current == '.')
        {
            if (Current == '.' && LookAhead(1) == '.' && LookAhead(2) == '.')
                break;

            Next();
        }

        int length = position - start;
        string token = Text!.Substring(start, length);
        if (!double.TryParse(token, out double value))
        {
            Error.SetError("LEXICAL", $"Line '{line}' : Invalid token '.'");
            return new SyntaxToken(SyntaxKind.ErrorToken, line, start, token, value);
        }

        return new SyntaxToken(SyntaxKind.NumberToken, line, start, token, value);
    }

    private SyntaxToken LexSuspensePoints()
    {
        if (LookAhead(1) == '.' && LookAhead(2) == '.')
            return new SyntaxToken(SyntaxKind.SuspenseToken, line, position += 3, "...", null!);

        Error.SetError("LEXCIAL", $"Line '{line}' : Unexpected character '.'");
        return new SyntaxToken(SyntaxKind.ErrorToken, line, position++, "", null!);
    }

    private SyntaxToken LexComments()
    {
        if (Current == '/' && NextCurrent == '/')
        {
            int start = position;
            while (Current != '\r')
            {
                Next();
                if (Current == '\0') break;
            }

            return new SyntaxToken(SyntaxKind.CommentToken, line, start, "//", null!);
        }
        
        (SyntaxToken token, int pos) = LexingSupplies.LexMathCharacters['/'](position, line, NextCurrent); 
        position = pos;
        return token;
    }

    private SyntaxToken LexIdentifiers()
    {
        int start = position;
        int spaces = 0;

        while (char.IsLetterOrDigit(Current) || Current == '_')
            Next();

        while (Current == ' ')
        {
            Next();
            spaces++;
        }

        int length = position - start - spaces;
        string token = Text!.Substring(start, length);
        var kind = LexingSupplies.GetKeywordKind(token);

        if (kind == SyntaxKind.ColorToken)
            return new SyntaxToken(kind, line, start, token, token);

        return new SyntaxToken(kind, line, start, token, null!);
    }

    private SyntaxToken LexWhitesSpace()
    {
        int start = position;

        while (char.IsWhiteSpace(Current))
            Next();

        int length = position - start;
        string token = Text!.Substring(start, length);
        return new SyntaxToken(SyntaxKind.WhitespaceToken, line, start, token, null!);
    }

    private SyntaxToken LexEndOfLine()
    {
        line++;
        return new SyntaxToken(SyntaxKind.WhitespaceToken, line, position++, " ", null!);
    }

    private bool IsGroupOfChars(out SyntaxToken token)
    {   
        token = null!;

        if (char.IsDigit(Current))
            token = LexNumbers();

        else if (Current == '.')
            token = LexSuspensePoints();

        else if (char.IsLetterOrDigit(Current) || Current == '_')
            token = LexIdentifiers();

        else if (char.IsWhiteSpace(Current))
            token = LexWhitesSpace();
        
        else return false;

        return true;
    }
}