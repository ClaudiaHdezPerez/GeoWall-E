using System.Drawing;

namespace G_Sharp;

#region Colores
public sealed class Colors : ExpressionSyntax
{
    // Colores que pueden usarse para dibujar
    public static Dictionary<string, Color> _Colors = new()
    {
        ["blue"]  = Color.DodgerBlue,  ["red"]  = Color.Red,  ["yellow"]  = Color.Yellow,
        ["green"] = Color.Green,       ["cyan"] = Color.Cyan, ["magenta"] = Color.Magenta,
        ["white"] = Color.White,       ["gray"] = Color.Gray, ["black"]   = Color.Black
    };

    public static Stack<Color>? ColorDraw;

    public override SyntaxKind Kind => SyntaxKind.ColorKeyword;

    public override string ReturnType => "void expression";

    #region Constructor
    public Color ActualColor { get; }


    public Colors(Color actualColor)
    {
        ActualColor = actualColor;
    }

    #endregion

    public static void InitializeColor()
    {
        ColorDraw = new();
        ColorDraw.Push(Color.DodgerBlue);
    }


    // Revisión
    public override bool Check(Scope scope)
    {
        return true;
    }

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        ColorDraw!.Push(ActualColor);
        return "";
    }
}

#endregion