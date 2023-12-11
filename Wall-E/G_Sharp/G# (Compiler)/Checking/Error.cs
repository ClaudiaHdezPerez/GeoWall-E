namespace G_Sharp;

public static class Error
{
    public static bool Wrong = false;
    public static string Msg = "";
    public static string TypeMsg = "";

    // Setear errores
    public static void SetError(string type, string msg)
    {
        if (Wrong) return;
        Wrong = true;
        Msg = msg;
        TypeMsg = type;
    }

    // Resetear programa
    public static void Reset()
    {
        Wrong = false;
        Msg = "";
        TypeMsg = "";
        Parser.AllImportedDocs = new();
        SemanticChecker.canImport = true;
        ScopeSupplies.RandomElements = new();
        ScopeSupplies.RandomPoints = new();
    }
}