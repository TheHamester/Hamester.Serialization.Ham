using Hamester.Serealization.Parsing;

namespace Hamester.Serealization;

public static class HamObjectFactory
{
    private static volatile HamObjectBuilder? _builder = null;
    private static readonly object _sync_lock = new();

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
    {
        if (_builder is null)
        {
            lock (_sync_lock)
            {
                _builder ??= new();
            }
        }

        return _builder;
    }
}
