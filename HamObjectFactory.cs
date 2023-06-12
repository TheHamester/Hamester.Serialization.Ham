using Hamester.Serialization.Ham.Exceptions;
using Hamester.Serialization.Ham.Parsing;

namespace Hamester.Serialization.Ham;


public static class HamObjectFactory
{
    private static volatile HamObjectCreator? _builder = null;
    private static readonly object _sync_lock = new();

    public static HamParseResult FromString(string hamText)
        => GetBuilderInstance().BuildObject(hamText);

    public static HamObject Create() => new();

    public static HamParseResult FromFile(string path)
    {
        string hamText;
        try 
        {
            hamText = File.ReadAllText(path);
        }
        catch(Exception ex) when (ExceptionTypes.IsFileError(ex))
        {
            return new(ex);
        }

        return GetBuilderInstance().BuildObject(hamText);
    }


    private static HamObjectCreator GetBuilderInstance()
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
