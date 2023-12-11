using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public static class ScopeSupplies
{
    public static Dictionary<int, object> RandomElements = new();
    public static Dictionary<int, object> RandomPoints = new();

    // Relevancia para la función intersect
    private static readonly Dictionary<SyntaxKind, int> relevanceFigures = new()
    {
        [SyntaxKind.PointToken]   = 60,
        [SyntaxKind.LineToken]    = 50,
        [SyntaxKind.SegmentToken] = 40,
        [SyntaxKind.RayToken]     = 30,
        [SyntaxKind.CircleToken]  = 20,
        [SyntaxKind.ArcToken]     = 10,
    };

    #region Funciones Predefinidas

    // Circle()
    public static object CircleFunction(Scope scope, List<ExpressionSyntax> parameters)
    {   
        try
        {
            Points center = (Points)parameters[0].Evaluate(scope);
            var measure = (Measure)parameters[1].Evaluate(scope);

            return new Circle(center, measure);
        }

        catch
        {
            string type1 = SemanticChecker.GetType(parameters[0]);
            string type2 = SemanticChecker.GetType(parameters[1]);

            Error.SetError("SEMANTIC", $"Function 'circle' receives '<point, " +
                            $"measure>', not '<{type1}, {type2}>'");
            return null!;
        }
        
    }

    // Point()
    internal static object PointFunction(Scope scope, List<ExpressionSyntax> parameters)
    {
        try
        {
            var x = (double)parameters[0].Evaluate(scope);
            var y = (double)parameters[1].Evaluate(scope);

            return new Points((float)x, (float)y);
        }

        catch
        {
            string type1 = SemanticChecker.GetType(parameters[0]);
            string type2 = SemanticChecker.GetType(parameters[1]);

            Error.SetError("SEMANTIC", $"Function 'point' receives '<number, " +
                            $"number>', not '<{type1}, {type2}>'");
            return null!;
        }
    }

    // Line()
    public static object LineFunction(Scope scope, List<ExpressionSyntax> parameters)
    {
        try
        {
            Points point1 = (Points)parameters[0].Evaluate(scope);
            Points point2 = (Points)parameters[1].Evaluate(scope);

            return new Line(point1, point2);
        }

        catch
        {
            string type1 = SemanticChecker.GetType(parameters[0]);
            string type2 = SemanticChecker.GetType(parameters[1]);

            Error.SetError("SEMANTIC", $"Function 'line' receives '<point, " +
                            $"point>', not '<{type1}, {type2}>'");
            return null!;
        }
    }

    // Segment()
    public static object SegmentFunction(Scope scope, List <ExpressionSyntax> parameters)
    {
        try
        {
            Points point1 = (Points)parameters[0].Evaluate(scope);
            Points point2 = (Points)parameters[1].Evaluate(scope);

            return new Segment(point1, point2);
        }

        catch
        {
            string type1 = SemanticChecker.GetType(parameters[0]);
            string type2 = SemanticChecker.GetType(parameters[1]);

            Error.SetError("SEMANTIC", $"Function 'segment' receives '<point, " +
                            $"point>', not '<{type1}, {type2}>'");
            return null!;
        }
    }

    // Ray()
    public static object RayFunction(Scope scope, List<ExpressionSyntax> parameters)
    {
        try
        {
            Points point1 = (Points)parameters[0].Evaluate(scope);
            Points point2 = (Points)parameters[1].Evaluate(scope);

            return new Ray(point1, point2);
        }

        catch
        {
            string type1 = SemanticChecker.GetType(parameters[0]);
            string type2 = SemanticChecker.GetType(parameters[1]);

            Error.SetError("SEMANTIC", $"Function 'ray' receives '<point, " +
                            $"point>', not '<{type1}, {type2}>'");
            return null!;
        }
    }

    // arc()
    public static object ArcFunction(Scope scope, List<ExpressionSyntax> parameters)
    {
        try 
        {
            var point1 = (Points)parameters[0].Evaluate(scope);
            var point2 = (Points)parameters[1].Evaluate(scope);
            var point3 = (Points)parameters[2].Evaluate(scope);
            var measure = (Measure)parameters[3].Evaluate(scope);

            return new Arc(point1, point2, point3, measure);
        }

        catch
        {
            string type1 = SemanticChecker.GetType(parameters[0]);
            string type2 = SemanticChecker.GetType(parameters[1]);
            string type3 = SemanticChecker.GetType(parameters[2]);
            string type4 = SemanticChecker.GetType(parameters[3]);

            Error.SetError("SEMANTIC", $"Function 'ray' receives '<point, point, point, measure>', " +
                            $"not '<{type1}, {type2}, {type3}, {type4}>'");
            return null!;
        }
        
    }

    // Measure()
    public static object MeasureFunction(Scope scope, List<ExpressionSyntax> parameters)
    {
        try
        {
            Points point1 = (Points)parameters[0].Evaluate(scope);
            Points point2 = (Points)parameters[1].Evaluate(scope);

            return new Measure(point1, point2);
        }

        catch
        {
            string type1 = SemanticChecker.GetType(parameters[0]);
            string type2 = SemanticChecker.GetType(parameters[1]);

            Error.SetError("SEMANTIC", $"Function 'measure' receives '<point, " +
                            $"point>', not '<{type1}, {type2}>'");
            return null!;
        }
    }

    // Count()
    public static object CountFunction(Scope scope, List<ExpressionSyntax> parameters)
    {
        try
        {
            var sequence = parameters[0].Evaluate(scope);
            var result = ((SequenceExpressionSyntax)sequence).Count;

            if (result > 0)
            {
                var element = ((FiniteSequence<object>)sequence).Elements.Last();
                if (element is null) return null!;
            }

            return result == -1 ? null! : result;
        }

        catch
        {
            string type1 = SemanticChecker.GetType(parameters[0]);

            Error.SetError("SEMANTIC", $"Function 'count' receives '<sequence>', not '<{type1}>'");
            return null!;
        }
    }

    // Randoms()
    public static object RandomsFunction(Scope scope, List<ExpressionSyntax> list)
    {
        static object Randoms()
        {
            Random random = new();
            return random.NextDouble();
        }

        var result = new InfiniteSequence(Randoms, RandomElements);
        result.valuesType = "number";

        return result;
    }

    // Samples()
    internal static object SamplesFunction(Scope scope, List<ExpressionSyntax> list)
    {
        static object Samples()
        {
            var x = ParsingSupplies.CreateRandomsCoordinates(0, 1300);
            var y = ParsingSupplies.CreateRandomsCoordinates(0, 1300);

            return new Points(x, y);
        }

        var result = new InfiniteSequence(Samples, RandomPoints);
        result.valuesType = "point";

        return result;
    }

    // Points()
    internal static object PointsFunction(Scope scope, List<ExpressionSyntax> list)
    {
        try
        {
            var figure = (Figure)scope.Evaluate(list[0]);
            return figure.PointsInFigure();
        }
        
        catch
        {
            string type1 = SemanticChecker.GetType(list[0]);

            Error.SetError("SEMANTIC", $"Function 'points' receives '<sequence>', not '<{type1}>'");
            return null!;
        }
    }

    // Intersect()
    internal static object IntersectFunction(Scope scope, List<ExpressionSyntax> list)
    {
        try
        {
            var firstArgument = (ExpressionSyntax)scope.Evaluate(list[0]);
            var secondArgument = (ExpressionSyntax)scope.Evaluate(list[1]);

            var firstRelevance = relevanceFigures[firstArgument.Kind];
            var secondRelevance = relevanceFigures[secondArgument.Kind];
            var figure1 = (Figure)firstArgument;
            var figure2 = (Figure)secondArgument;
            var mostImportant = (firstRelevance >= secondRelevance) ? figure1 : figure2;
            var leastImportant = (firstRelevance < secondRelevance) ? figure1 : figure2;

            return mostImportant.Intersect(leastImportant);
        }

        catch
        {
            string type1 = SemanticChecker.GetType(list[0]);
            string type2 = SemanticChecker.GetType(list[1]);

            Error.SetError("SEMANTIC", $"Function 'intersect' receives '<figure, figure>', not '<{type1}, {type2}>'");
            return null!;
        }
    }

    #endregion
}

