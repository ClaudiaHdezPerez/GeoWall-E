using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using G_Sharp;

namespace WallE;

public static class Blender
{
    public static string ErrorType = "";
    public static string ErrorMsg = "";

    public static void Reset()
    {
        Colors.InitializeColor();
        Error.Reset();
    }
    public static (List<object>, bool) BlendCompile(string text)
    {
        Reset();
        Dictionary<string, Constant> constants = new();
        Dictionary<string, Function> functions = new();

        Scope global = new(constants, functions);

        if (string.IsNullOrWhiteSpace(text))
            return (new(), true);

        var syntaxTree = SyntaxTree.Parse(text);
        List<object> obj = new();

        foreach (var root in syntaxTree.Root)
        {
            var checking = global.Check(root);
            if (!checking) break;

            var result = global.Evaluate(root);

            if (result is List<object> list)
                obj.AddRange(list);

            else obj.Add(result);
        }


        if (Error.Wrong)
        {
            ErrorMsg = Error.Msg;
            ErrorType = Error.TypeMsg;
            return (new(), false);
        }


        return (obj, true);
    }

    public static (List<(ExpressionSyntax, Color, string)>, bool) BlendRun(List<object> result)
    {
        Reset();
        List<(ExpressionSyntax, Color, string)> Geometries = new();

        if (!Error.Wrong)
        {
            foreach (var item in result)
            {
                if (item is Draw geometries)
                    Geometries.Add(geometries.Geometries);
            }

            return (Geometries, true);
        }

        if (Error.Wrong)
        {
            ErrorMsg = Error.Msg;
            ErrorType = Error.TypeMsg;
        }

        return (new(), false);
    }
}
