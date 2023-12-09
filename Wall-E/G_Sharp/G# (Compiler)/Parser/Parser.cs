
using System.CodeDom;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;

namespace G_Sharp;

internal sealed class Parser
{
    public static List<string> AllImportedDocs = new();

    private List<string> localImportedDocs = new();
    private List<ExpressionSyntax> functionParams = new();
    private int positionAfterFuncParams;
    private readonly List<SyntaxToken> tokens;
    private int position;

    private SyntaxToken Current => LookAhead(0);
    public bool ContainsError { get; }
    private Dictionary<SyntaxKind, Func<ExpressionSyntax>> tokensEvaluation { get; }
    private Dictionary<SyntaxKind, Func<ExpressionSyntax>> assignmentEvaluation { get; }
    public Parser(string text)
    {
        List<SyntaxToken> tokens = new();

        var lexer = new Lexer(text);

        SyntaxToken token;

        do
        {
            token = lexer.Lex();

            if (token.Kind == SyntaxKind.ErrorToken)
            {
                ContainsError = true;
                break;
            }

            else if (token.Kind != SyntaxKind.WhitespaceToken &&
                token.Kind != SyntaxKind.CommentToken)
            { 
                tokens.Add(token); 
            }  
        }

        while (token.Kind != SyntaxKind.EndOfFileToken);

        this.tokens = tokens;

        tokensEvaluation = new()
        {
            [SyntaxKind.RestoreKeyword]        = RestoreParsing,
            [SyntaxKind.ColorKeyword]          = ColorParsing,
            [SyntaxKind.OpenParenthesisToken]  = ParenthesizedExpressionParsing,
            [SyntaxKind.OpenCurlyBracketToken] = SequenceExpressionParsing,
            [SyntaxKind.LetKeyword]            = LetInExpressionParsing,
            [SyntaxKind.IfKeyword]             = ConditionalExpressionParsing,
            [SyntaxKind.IdentifierToken]       = IdentifierParsing,
            [SyntaxKind.MathToken]             = IdentifierParsing,
            [SyntaxKind.UndefinedToken]        = IdentifierParsing,
            [SyntaxKind.StringToken]           = StringParsing,
            [SyntaxKind.NumberToken]           = NumberParsing,
            [SyntaxKind.GeometryKeyword]       = AssignmentGeometryParsing,
            [SyntaxKind.ImportKeyword]         = ImportParsing
        };

        assignmentEvaluation = new()
        {
            [SyntaxKind.DrawKeyword] = DrawParsing,
            [SyntaxKind.IdentifierToken] = AssignmentIdentifierEval,
            [SyntaxKind.MathToken] = MathKeywordParsing,
            [SyntaxKind.UndefinedToken] = MathKeywordParsing
        };
    }

    private SyntaxToken LookAhead(int offset)
    {
        int index = position + offset;
        return (index >= tokens.Count) ? tokens[^1] : tokens[index];
    }

    private SyntaxToken NextToken()
    {
        var current = Current;
        position++;
        return current;
    }

    public SyntaxTree Parse()
    {
        List<ExpressionSyntax> expressions = new();
        var semicolonToken = new SyntaxToken(SyntaxKind.SemicolonToken, 1, 0, ";", "");

        while (true)
        {

            var expression = ParseExpression();
            expressions.Add(expression);

            semicolonToken = MatchToken(SyntaxKind.SemicolonToken, ";");

            if (semicolonToken.Kind == SyntaxKind.ErrorToken)
            {
                expressions.Add(new ErrorExpressionSyntax());
                break;
            }

            if (Current.Kind == SyntaxKind.EndOfFileToken) break;
        }

        return new SyntaxTree(Error.Wrong, expressions, semicolonToken);
    }

    private SyntaxToken MatchToken(SyntaxKind kind, string expectedText)
    {
        if (Current.Kind == kind)
            return NextToken();

        Error.SetError("SYNTAX", $"Line '{Current.Line}' : Unexpected token ' {Current.Text} ', expected ' {expectedText} '");
        kind = SyntaxKind.ErrorToken;
        return new SyntaxToken(kind, 1, Current.Position, "", 0.0);
    }

    public ExpressionSyntax ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    public ExpressionSyntax ParseAssignmentExpression()
    {
        if (assignmentEvaluation.TryGetValue(Current.Kind, out Func<ExpressionSyntax>? value))
            return value();

        return ParseBinaryExpression();
    }

    private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
    {
        ExpressionSyntax left;
        var unaryOperatorPrecedent = Current.Kind.GetUnaryOperatorPrecedence();
        
        if (unaryOperatorPrecedent != 0 && unaryOperatorPrecedent >= parentPrecedence) 
        {
            var operatorToken = NextToken();
            var operand = ParseBinaryExpression(unaryOperatorPrecedent);
            left = new UnaryExpressionSyntax(operatorToken, operand);
        }

        else left = ParsePrimaryExpression();

        while (true)
        {
            var precedence = Current.Kind.GetBinaryOperatorPrecedence();

            if (precedence == 0 || precedence <= parentPrecedence) break;

            var operatorToken = NextToken();
            var right = ParseBinaryExpression(precedence);
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }

    private ExpressionSyntax ParsePrimaryExpression()
    {
        if (tokensEvaluation.TryGetValue(Current.Kind, out Func<ExpressionSyntax>? value))
            return value();
            
        var semicolonToken = MatchToken(SyntaxKind.SemicolonToken, ";");

        if (semicolonToken.Kind == SyntaxKind.ErrorToken)
            return new ErrorExpressionSyntax();

        return new LiteralExpressionSyntax(semicolonToken);
    }

    private ExpressionSyntax ImportParsing()
    {
        var import = NextToken();
        var direction = MatchToken(SyntaxKind.StringToken, "path");

        if (direction.Kind == SyntaxKind.ErrorToken)
            return new ErrorExpressionSyntax();

        string path = direction.Text;
        path = path[1..^1];

        if (AllImportedDocs.Contains(path))
        {
            Error.SetError("COMPILE", $"Line '{import.Line}' : Files only can be imported once");
            return new ErrorExpressionSyntax();
        }

        else
        {
            if (localImportedDocs.Contains(path))
                return new VoidExpressionSyntax();

            AllImportedDocs.Add(path);
            localImportedDocs.Add(path);
        }

        try
        {
            string text = File.ReadAllText(path);
            var syntaxTree = SyntaxTree.Parse(text);
            AllImportedDocs.Remove(path);
            return new ImportExpressionSyntax(import, syntaxTree);
        }

        catch
        {
            Error.SetError("COMPILE", $"Line '{import.Line}' : The given path doesn't contain a valid file");
            return new ErrorExpressionSyntax();
        }
        
    }

    private ExpressionSyntax RestoreParsing()
    {
        var restoreToken = NextToken();
        if (Colors.ColorDraw!.Count < 1)
        {
            Error.SetError("SYNTAX", $"Line '{restoreToken.Line}' : Default color can't be restored");
            return new ErrorExpressionSyntax();
        }

        Colors.ColorDraw!.Pop();
        return new VoidExpressionSyntax();
    }

    private ExpressionSyntax ColorParsing()
    {
        NextToken();
        var color = MatchToken(SyntaxKind.ColorToken, "color");
        if (color.Kind == SyntaxKind.ErrorToken)
            return new ErrorExpressionSyntax();
        
        Colors.ColorDraw!.Push(Colors._Colors[color.Value.ToString()!]);
        return new VoidExpressionSyntax();
    }

    private ExpressionSyntax MathKeywordParsing()
    {
        return AssignmentIdentifierEval();
    }

    private ParenthesizedExpressionSyntax ParenthesizedExpressionParsing()
    {
        var left = NextToken();
        var expression = ParseBinaryExpression();
        var right = MatchToken(SyntaxKind.ClosedParenthesisToken, ")");

        return new ParenthesizedExpressionSyntax(left, expression, right);
    }

    private SequenceExpressionSyntax SequenceExpressionParsing()
    {
        if (LookAhead(2).Kind == SyntaxKind.SuspenseToken)
        {
            NextToken();
            var firstNumber = MatchToken(SyntaxKind.NumberToken, "number");

            if (!long.TryParse(firstNumber.Text, out long first))
                Error.SetError("SYNTAX", $"Line '{firstNumber.Line}' : Expected integer number");

            var suspenseToken = NextToken();

            long last = long.MaxValue;
            bool hasEnd = false;
            if (Current.Kind == SyntaxKind.NumberToken)
            {
                hasEnd = true;
                if (!long.TryParse(Current.Text, out last))
                    Error.SetError("SYNTAX", $"Line '{Current.Line}' : Expected integer number");
                NextToken();
            }

            var closedCurlyBracket = MatchToken(SyntaxKind.ClosedCurlyBracketToken, "}");

            if (hasEnd && !Error.Wrong)
            {
                if (last <= first)
                    return new FiniteSequence<ExpressionSyntax>(new List<ExpressionSyntax>());
            }
            
            return new InfiniteIntegerSequence(first, last);
        }

        var elements = GetFunctionParams("sequence");
        NextToken();

        return new FiniteSequence<ExpressionSyntax>(elements);
    }

    private ExpressionSyntax LetInExpressionParsing()
    {
        var letToken = NextToken();
        List<ExpressionSyntax> instructions = new();

        while (Current.Kind != SyntaxKind.InKeyword)
        {
            if (Current.Kind == SyntaxKind.EndOfFileToken)
            {
                MatchToken(SyntaxKind.InKeyword, "in");
                return new ErrorExpressionSyntax();
            }

            var statement = ParseExpression();

            var semicolonToken = MatchToken(SyntaxKind.SemicolonToken, ";");

            if (semicolonToken.Kind == SyntaxKind.ErrorToken)
                return new ErrorExpressionSyntax();

            instructions.Add(statement);
        }

        var inToken = MatchToken(SyntaxKind.InKeyword, "in");

        if (inToken.Kind == SyntaxKind.ErrorToken)
            return new ErrorExpressionSyntax();
        
        if (Current.Kind == SyntaxKind.SemicolonToken)
        {
            Error.SetError("SYNTAX", $"Line '{Current.Line}' : Missing expression after 'in' in 'let-in' expression");
            return new ErrorExpressionSyntax();
        }

        var body = ParseExpression();

        return new LetInExpressionSyntax(letToken, instructions, inToken, body);
    }

    private ExpressionSyntax ConditionalExpressionParsing()
    {
        var ifKeyword = NextToken();
        var condition = ParseBinaryExpression();

        var thenKeyword = MatchToken(SyntaxKind.ThenKeyword, "then");
        var bodyTrue = ParseExpression();

        var elseKeyword = MatchToken(SyntaxKind.ElseKeyword, "else");
        var bodyFalse = ParseExpression();

        return new ConditionalExpressionSyntax(ifKeyword, condition, thenKeyword, bodyTrue, elseKeyword, bodyFalse);
    }

    private ExpressionSyntax IdentifierParsing()
    {
        var identifierToken = NextToken();

        if (Current.Kind == SyntaxKind.OpenParenthesisToken)
        {
            if (functionParams.Count > 0)
            {
                NextToken();

                if (functionParams.Any(x => x is ErrorExpressionSyntax))
                    return new ErrorExpressionSyntax();

                if (Current.Kind != SyntaxKind.ClosedParenthesisToken)
                    position = positionAfterFuncParams;

                ExpressionSyntax[] parameters = new ExpressionSyntax[functionParams.Count];
                Array.Copy(functionParams.ToArray(), parameters, parameters.Length);
                functionParams.Clear();
                return new FunctionExpressionSyntax(identifierToken, parameters.ToList());
            }

            else
            {
                List<ExpressionSyntax> parameters = GetFunctionParams(identifierToken.Text);
                NextToken();
                return new FunctionExpressionSyntax(identifierToken, parameters);
            }
        }

        return new ConstantExpressionSyntax(identifierToken);
    }

    private ExpressionSyntax StringParsing()
    {
        var stringToken = MatchToken(SyntaxKind.StringToken, "string");

        if (stringToken.Kind == SyntaxKind.ErrorToken)
            return new ErrorExpressionSyntax();

        return new LiteralExpressionSyntax(stringToken);
    }

    private ExpressionSyntax NumberParsing()
    {
        var numberToken = MatchToken(SyntaxKind.NumberToken, "number");

        if (numberToken.Kind == SyntaxKind.ErrorToken)
            return new ErrorExpressionSyntax();

        return new LiteralExpressionSyntax(numberToken);
    }

    private ExpressionSyntax DrawParsing()
    {
        var drawToken = NextToken();
        string msg = "";
        ExpressionSyntax value;

        if (Current.Kind == SyntaxKind.SemicolonToken)
        {
            Error.SetError("SYNTAX", $"Line '{Current.Line}': Empty argument " +
                            $"in 'draw' statement");
            return new ErrorExpressionSyntax();
        }

        if (Current.Kind != SyntaxKind.OpenCurlyBracketToken)
            value = ParseAssignmentExpression();

        else
            value = SequenceExpressionParsing();

        if (Current.Kind == SyntaxKind.StringToken)
            msg = NextToken().Value.ToString()!;
        

        return new Draw(drawToken, value, Colors.ColorDraw!.Peek(), msg);  
    }

    private ExpressionSyntax AssignmentGeometryParsing()
    {
        var typeGeometry = NextToken();

        if (Current.Kind == SyntaxKind.OpenParenthesisToken)
        {
            List<ExpressionSyntax> parameters = GetFunctionParams(typeGeometry.Text);
            NextToken();
            return new FunctionExpressionSyntax(typeGeometry, parameters);
        }

        else if (Current.Kind == SyntaxKind.SequenceKeyword)
        {
            Random generateRandomsEelements = new();
            NextToken();
            var id = MatchToken(SyntaxKind.IdentifierToken, "constant");

            if (id.Kind == SyntaxKind.ErrorToken)
                return new ErrorExpressionSyntax();

            int count = generateRandomsEelements.Next(2, 11);
            var operatorToken = new SyntaxToken(SyntaxKind.AssignmentToken, id.Line, id.Position + 1, "=", "");
            List<ExpressionSyntax> elements = new();

            for (int i = 0; i < count; i++)
            {
                var name = new SyntaxToken(SyntaxKind.ConstantExpression, id.Line, id.Position, $"{i}element", "");
                var element = ParsingSupplies.RandomsGeometricElements[typeGeometry.Text]();
                elements.Add(element);
            }

            var sequence = new FiniteSequence<ExpressionSyntax>(elements);
            return new ConstantAssignmentSyntax(id, operatorToken, sequence);
        }

        else 
        {
            var name = MatchToken(SyntaxKind.IdentifierToken, "constant");
            var operatorToken = new SyntaxToken(SyntaxKind.AssignmentToken, 1, 0, "=", "");
            return ParsingSupplies.GeometricKeywordsEvaluation[typeGeometry.Text](name, operatorToken);
        }
    }

    private ExpressionSyntax AssignmentIdentifierEval()
    {
        if (LookAhead(1).Kind == SyntaxKind.OpenParenthesisToken)
        {
            int actualPos = position;
            var identifierToken = NextToken();

            List<ExpressionSyntax> parameters = GetFunctionParams(identifierToken.Text);

            NextToken();

            if (Current.Kind == SyntaxKind.AssignmentToken)
            {
                var operatorToken = NextToken();
                var body = ParseAssignmentExpression();

                if (body.Kind == SyntaxKind.SemicolonToken)
                {
                    Error.SetError("SEMANTIC", $"Line '{Current.Line}' : Missing expression in " +
                                    $"'{identifierToken.Text}' declaration");
                    return new ErrorExpressionSyntax();
                }

                foreach (var item in parameters)
                {
                    if (item.Kind != SyntaxKind.ConstantExpression)
                    {
                        Error.SetError("SYNTAX", $"Line '{operatorToken.Line}' : Expected identifier token in function parameters");
                    }
                }

                return new AssignmentFunctionSyntax(identifierToken, parameters, operatorToken, body);
            }

            functionParams = parameters;
            positionAfterFuncParams = position;
            position = actualPos;
        }

        else if (LookAhead(1).Kind == SyntaxKind.SeparatorToken)
        {
            List<SyntaxToken> identifiers = new()
            {
                NextToken()
            };

            var separatorToken = NextToken();

            while (true)
            {
                if (Current.Kind != SyntaxKind.IdentifierToken &&
                    Current.Kind != SyntaxKind.SeparatorToken
                   )
                {
                    Error.SetError("SYNTAX", $"Line: '{Current.Line}' : Unexpected '{Current.Text}'");
                    return new ErrorExpressionSyntax();
                }

                
                var identifierToken = MatchToken(SyntaxKind.IdentifierToken, "constant");
                if (identifierToken.Kind == SyntaxKind.ErrorToken)
                    return new ErrorExpressionSyntax();

                identifiers.Add(identifierToken);

                if (Current.Kind == SyntaxKind.AssignmentToken) break;
                separatorToken = MatchToken(SyntaxKind.SeparatorToken, ",");

                if (separatorToken.Kind == SyntaxKind.ErrorToken)
                    return new ErrorExpressionSyntax();
            }

            var assigmentToken = NextToken();
            var expression = ParseExpression();

            return new MultipleAssignmentSyntax(identifiers, assigmentToken, expression);
        }

        else if (LookAhead(1).Kind == SyntaxKind.AssignmentToken)
        {
            var identifierToken = NextToken();
            var operatorToken = NextToken();
            var right = ParseAssignmentExpression();

            if (right is LiteralExpressionSyntax literal && 
                literal.LiteralToken.Kind == SyntaxKind.SemicolonToken
               )
            {
                Error.SetError("SEMANTIC", $"Line '{operatorToken.Line}' : Missing expression in " +
                                $"'{identifierToken.Text}' assignment");
                return new ErrorExpressionSyntax();
            }

            return new ConstantAssignmentSyntax(identifierToken, operatorToken, right);
        }

        return ParseBinaryExpression();
    }

    private List<ExpressionSyntax> GetFunctionParams(string name)
    {
        var kind = Current.Kind;
        var endKind = (kind == SyntaxKind.OpenParenthesisToken) ? SyntaxKind.ClosedParenthesisToken :
                                                                  SyntaxKind.ClosedCurlyBracketToken;

        NextToken();
        SyntaxToken token = Current;
        List<ExpressionSyntax> parameters = new();

        if (Current.Kind == endKind)
            return parameters;

        var parameter = ParseBinaryExpression();
        parameters.Add(parameter);

        while (Current.Kind == SyntaxKind.SeparatorToken)
        {
            NextToken();
            token = Current;
            if (Current.Kind == endKind)
            {
                Error.SetError("SEMANTIC", $"Line '{Current.Line}' : Missing argument in '{name}'");
                parameters.Add(new ErrorExpressionSyntax());
                return parameters;
            }

            parameter = ParseBinaryExpression();
            parameters.Add(parameter);
        }

        if (Current.Kind != endKind)
        {
            var text = (kind == SyntaxKind.OpenParenthesisToken) ? "parenthesis" : "curly brackets";
            Error.SetError("SYNTAX", $"Line '{Current.Line}' : Missing closing {text} after '{token.Text}'");
        }

        return parameters;
    }
}