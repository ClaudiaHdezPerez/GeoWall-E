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

    #region Resetear
    // Método para resetear los campos estáticos 
    public static void Reset()
    {
        Colors.InitializeColor();
        Error.Reset();
    }
    #endregion

    #region Compilar
    // Método para relacionar la UI con el compilador para obtener la lista de objetos a dibujar
    public static (List<Draw>, bool) BlendCompile(string text)
    {
        Reset();

        // Se crean los diccionarios necesarios para crear el scope global
        Dictionary<string, Constant> constants = new();
        Dictionary<string, Function> functions = new();

        Scope global = new(constants, functions);

        if (string.IsNullOrWhiteSpace(text))
            return (new(), true);

        // Se parsea el texto en el textBox y se devuelve en una estructura en forma de árbol
        var syntaxTree = SyntaxTree.Parse(text);

        // Cada "rama" fue clasificada por un tipo de expresión y es chequeada para luego evaluarse
        foreach (var root in syntaxTree.Root)
        {
            var checking = global.Check(root);
            if (!checking) break;

            global.Evaluate(root);
        }


        if (Error.Wrong)
        {
            ErrorMsg = Error.Msg;
            ErrorType = Error.TypeMsg;
            return (new(), false);
        }


        return (global.DrawingObjects, true);
    }
    #endregion
}
