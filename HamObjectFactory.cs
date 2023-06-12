using Hamester.Serealization.Parsing;

namespace Hamester.Serealization;

public static class HamObjectFactory
{
    private static HamObjectBuilder? _builder = null;

    public static HamObject? FromString(string hamText)
        => GetBuilderInstance().BuildObject(hamText);

    public static HamObject Create() => new();

    public static HamObject? FromFile(string path)
    {
        string hamText;
        try 
        {
            hamText = File.ReadAllText(path);
        }
        catch(IOException)
        {
            return null;
        }

        return GetBuilderInstance().BuildObject(hamText);
    }
    

    private static HamObjectBuilder GetBuilderInstance() 
        => _builder ??= new();
}
