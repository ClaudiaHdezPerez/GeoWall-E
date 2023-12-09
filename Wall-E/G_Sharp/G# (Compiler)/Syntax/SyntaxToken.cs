namespace G_Sharp;

public sealed class SyntaxToken
{
    public SyntaxKind Kind { get; }
    public int Position { get; }
    public int Line { get; }
    public string Text { get; }
    public object Value { get; set; }

    public SyntaxToken(SyntaxKind kind, int line, int position, string text, object value)
    {
        Kind = kind;
        Position = position;
        Line = line;
        Text = text;
        Value = value;
    }
}
