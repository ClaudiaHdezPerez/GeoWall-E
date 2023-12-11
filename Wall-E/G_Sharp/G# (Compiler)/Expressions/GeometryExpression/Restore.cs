using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

#region Restore
public sealed class Restore : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.RestoreKeyword;

    public override string ReturnType => "void expression";

    #region Constructor
    public SyntaxToken RestoreToken { get; }

    public Restore(SyntaxToken restoreToken)
    {
        RestoreToken = restoreToken;
    }

    #endregion


    // Revisión
    public override bool Check(Scope scope)
    {
        if (Colors.ColorDraw!.Count < 1)
        {
            Error.SetError("SYNTAX", $"Line '{RestoreToken.Line}' : Default color can't be restored");
            return false;
        }

        return true;
    }

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        Colors.ColorDraw!.Pop();
        return "";
    }
}

#endregion
