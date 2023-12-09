
namespace G_Sharp;

public static class ParsingSupplies
{

    private static readonly Dictionary<SyntaxKind, int> binaryOperatorPrecedence = new()
    {
        [SyntaxKind.AndKeyword]           = 1,
        [SyntaxKind.OrKeyword]            = 1,
        [SyntaxKind.EqualToken]           = 3,
        [SyntaxKind.DifferentToken]       = 3,
        [SyntaxKind.GreaterToken]         = 2,
        [SyntaxKind.LessToken]            = 2,
        [SyntaxKind.GreaterOrEqualToken]  = 2,
        [SyntaxKind.LessOrEqualToken]     = 2,
        [SyntaxKind.MultToken]            = 6,
        [SyntaxKind.DivisionToken]        = 6,
        [SyntaxKind.ModToken]             = 6,
        [SyntaxKind.PlusToken]            = 5,
        [SyntaxKind.MinusToken]           = 5
    };

    private static readonly Dictionary<SyntaxKind, int> unaryOperatorPrecedence = new()
    {
        [SyntaxKind.NotKeyword] = 4,
        [SyntaxKind.PlusToken]  = 7,
        [SyntaxKind.MinusToken] = 7
    };

    
    public static Dictionary<string, Func<SyntaxToken, SyntaxToken, ExpressionSyntax>> GeometricKeywordsEvaluation = new()
    {
        ["point"]   = PointParsing,
        ["line"]    = LineParsing,
        ["segment"] = SegmentParsing,
        ["ray"]     = RayParsing,
        ["measure"] = MeasureParsing,
        ["circle"]  = CircleParsing,
        ["arc"]     = ArcParsing
    };

    public static Dictionary<string, Func<ExpressionSyntax>> RandomsGeometricElements = new()
    {
        ["point"]   = CreateRandomPoint,
        ["line"]    = () => new Line(CreateRandomPoint(), CreateRandomPoint()),
        ["segment"] = () => new Segment(CreateRandomPoint(), CreateRandomPoint()),
        ["ray"]     = () => new Ray(CreateRandomPoint(), CreateRandomPoint()),
        ["measure"] = CreateRandomMeasure,
        ["circle"]  = () => new Circle(CreateRandomPoint(), CreateRandomMeasure()),
        ["arc"]     = () => new Arc(CreateRandomPoint(), CreateRandomPoint(), CreateRandomPoint(), CreateRandomMeasure())
    };

    public static float CreateRandomsCoordinates(int start = 50, int end = 900)
    {
        Random random = new();
        return (float)(random.Next(start, end) + random.NextDouble());
    }
    private static Measure CreateRandomMeasure()
    {
        var points = CreateRandomPoints(2);
        return new Measure(points[0], points[1]);
    }

    public static Points CreateRandomPoint()
    {
        var points = CreateRandomPoints(1);
        return points[0];
    }

    private static Points[] CreateRandomPoints(int quantity)
    {
        Points[] points = new Points[quantity];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Points(CreateRandomsCoordinates(), CreateRandomsCoordinates());
        }

        return points;
    }

    private static ExpressionSyntax PointParsing(SyntaxToken name, SyntaxToken operatorToken)
    {
        var point = CreateRandomPoints(1);
        return new ConstantAssignmentSyntax(name, operatorToken, point[0]);
    }

    private static ExpressionSyntax SegmentParsing(SyntaxToken name, SyntaxToken operatorToken)
    {   
        var points = CreateRandomPoints(2);
        var segment = new Segment(points[0], points[1]);
        return new ConstantAssignmentSyntax(name, operatorToken, segment);
    }

    private static ExpressionSyntax LineParsing(SyntaxToken name, SyntaxToken operatorToken)
    {   
        var points = CreateRandomPoints(2);
        var segment = new Line(points[0], points[1]);
        return new ConstantAssignmentSyntax(name, operatorToken, segment);
    }

    private static ExpressionSyntax RayParsing(SyntaxToken name, SyntaxToken operatorToken)
    {   
        var points = CreateRandomPoints(2);
        var segment = new Ray(points[0], points[1]);
        return new ConstantAssignmentSyntax(name, operatorToken, segment);
    }

    private static ExpressionSyntax MeasureParsing(SyntaxToken name, SyntaxToken operatorToken)
    {
        var points = CreateRandomPoints(2);
        var measure = new Measure(points[0], points[1]);
        return new ConstantAssignmentSyntax(name, operatorToken, measure);
    }

    private static ExpressionSyntax CircleParsing(SyntaxToken name, SyntaxToken operatorToken)
    {   
        var points = CreateRandomPoints(2);
        var measure = new Measure(points[0], points[1]);
        var circle = new Circle(points[0], measure);

        return new ConstantAssignmentSyntax(name, operatorToken, circle);
    }

    private static ExpressionSyntax ArcParsing(SyntaxToken name, SyntaxToken operatorToken)
    {   
        var points = CreateRandomPoints(4);
        var measure = new Measure(points[0], points[3]);
        var arc = new Arc(points[0], points[1], points[2], measure);

        return new ConstantAssignmentSyntax(name, operatorToken, arc);
    }

    public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
    {
        if (binaryOperatorPrecedence.TryGetValue(kind, out int value))
            return value;

        return 0;
    }

    public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
    {
        if (unaryOperatorPrecedence.TryGetValue(kind, out int value))
            return value;

        return 0;
    }

    // Metodo que evalua los slash en el string
    public static string BackSlashEval(string text, int line)
    {
        // Secuencias de escape permitidas
        char[] scapes = { 'n', 'r', 't', 'a', 'f', 'b', 'v', '"', '\'', '\\' };
        char[] scapeSequency = { '\n', '\r', '\t', '\a', '\f', '\b', '\v' };

        int backSlashIndex = text.IndexOf("\\");

        // Se itera mientras tenga slash
        while (backSlashIndex != -1)
        {
            int count = 0;

            // Se cuentan cuantos seguidos hay
            for (int i = backSlashIndex; i < text.Length; i++)
            {
                if (text[i] != '\\') break;
                count++;
            }

            // Se remueve la mitad de los slash
            text = text.Remove(backSlashIndex, count / 2);

            // Si no es par, se inserta el caracter de escape deseado
            if (count % 2 != 0)
            {
                int scapeIndex = Array.IndexOf(scapes, text[backSlashIndex + count - count / 2]);
                text = text.Remove(backSlashIndex, 1);

                if (scapeIndex < 0)
                {
                    Error.SetError("SYNTAX", $"Line '{line}' : Invalid scape character in string");
                    return "";
                }

                if (!(scapes[scapeIndex] == '"' || scapes[scapeIndex] == '\'' ||
                    scapes[scapeIndex] == '\\'))
                {
                    text = text.Remove(backSlashIndex + count - count / 2 - 1, 1);
                    text = text.Insert(backSlashIndex + count - count / 2 - 1, scapeSequency[scapeIndex].ToString());
                }
            }

            backSlashIndex = text.IndexOf("\\", backSlashIndex + count / 2);
        }

        return text;
    }

}