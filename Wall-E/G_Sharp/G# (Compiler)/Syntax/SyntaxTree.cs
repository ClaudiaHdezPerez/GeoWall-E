namespace G_Sharp;
public sealed class SyntaxTree
{
    public bool Error { get; }
    public List<ExpressionSyntax> Root { get; }
    public SyntaxToken EndOfFileToken { get; }

    public SyntaxTree(bool error, List<ExpressionSyntax> root, SyntaxToken endOfFileToken)
    {
        Error = error;
        Root = root;
        EndOfFileToken = endOfFileToken;
    }

    public static SyntaxTree Parse(string text)
    {
        var parser = new Parser(text);
        var endOfFileToken = new SyntaxToken(SyntaxKind.EndOfFileToken, 0, 0, "", "");

        if (parser.ContainsError)
            return new SyntaxTree(true, new List<ExpressionSyntax> { new ErrorExpressionSyntax() }, endOfFileToken);

        return parser.Parse();
    }
}