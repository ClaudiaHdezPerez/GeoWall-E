namespace G_Sharp;

public static class Error
{
    public static bool Wrong = false;
    public static string Msg = "";
    public static string TypeMsg = "";

    public static void SetError(string type, string msg)
    {
        if (Wrong) return;
        Wrong = true;
        Msg = msg;
        TypeMsg = type;
    }

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