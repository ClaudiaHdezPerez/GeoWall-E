using static System.Runtime.InteropServices.JavaScript.JSType;

namespace G_Sharp;

public sealed class FunctionExpressionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.FunctionExpression;

    public SyntaxToken IdentifierToken { get; }
    public List<ExpressionSyntax> Values { get; }

    private string returnType = "undefined";
    public override string ReturnType => returnType;

    public FunctionExpressionSyntax(SyntaxToken identifierToken, List<ExpressionSyntax> values)
    {
        IdentifierToken = identifierToken;
        Values = values;
    }

    public override object Evaluate(Scope scope)
    {
        var functionToken = IdentifierToken;
        var values = Values;
        string name = functionToken.Text;
        var function = scope.Functions[name];

        if (Scope.DefaultFunctions.TryGetValue(name, out Func<Scope, List<ExpressionSyntax>, object>? func))
            return func(scope, values);

        if (function.NumberOfCalls > 1000000)
        {
            Error.SetError("RUNTIME", $"Stack Overflow produced by function '{name}'");
            returnType = "undefined";
            return null!;
        }

        Dictionary<string, Constant> constants = new();

        for (int i = 0; i < values.Count; i++)
        {
            ConstantExpressionSyntax parameter = (ConstantExpressionSyntax)function!.Parameters[i];
            var value = values[i].Evaluate(scope);
            var identifier = parameter.IdentifierToken;
            constants[identifier.Text] = new Constant(value);
        }

        Scope child = GetChildScope(scope, constants);
        child.Functions[name].NumberOfCalls++; 
        var evaluation = child.Evaluate(function!.Body);

        scope.DrawingObjects.AddRange(child.DrawingObjects);

        return evaluation;
    }

    public override bool Check(Scope scope)
    {
        var functionToken = IdentifierToken;
        string name = functionToken.Text;

        if (!scope.Functions.TryGetValue(name, out Function? function) &&
            !Scope.DefaultFunctions.ContainsKey(name))
        {
            Error.SetError("SEMANTIC", $"Line '{functionToken.Line}' : Function '{name}' is not defined yet");
            return false;
        }

        if (Values.Count != function!.NumberOfParams)
        {
            int expectedNumber = function!.NumberOfParams;
            int givenNumber = Values.Count;
            string pluralParameters = expectedNumber != 1 ? "parameters" : "parameter";

            string wasPlural = givenNumber > 1 ? "were" : "was";
            Error.SetError("SEMANTIC", $"Line '{functionToken.Line}' : Function '{name}' recieves " +
                            $"{expectedNumber} {pluralParameters}, but {givenNumber} {wasPlural} given");
            return false;
        }

        if (Scope.DefaultFunctions.TryGetValue(name, out Func<Scope, List<ExpressionSyntax>, object>? func))
        {
            for (int i = 0; i < Values.Count; i++)
            {
                if (!Values[i].Check(scope)) 
                    return false;
            }

            return true;
        }

        if (function.hasBeenCalled) return true;

        Dictionary<string, Constant> constants = new();

        for (int i = 0; i < Values.Count; i++)
        {
            if (!Values[i].Check(scope)) 
                return false;

            ConstantExpressionSyntax parameter = (ConstantExpressionSyntax)function!.Parameters[i];
            var identifier = parameter.IdentifierToken;
            constants[identifier.Text] = new Constant(Values[i]);
        }

        Scope child = GetChildScope(scope, constants);

        child.Functions[name].hasBeenCalled = true;
        var check = child.Check(function!.Body);
        child.Functions[name].hasBeenCalled = false;

        return check;
    }

    private Scope GetChildScope(Scope scope, Dictionary<string, Constant> constants)
    {
        return new(constants, scope.Functions);
    }
}
