using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Hamester.Serealization;

public class HamObject : IHamElement
{
    private readonly Dictionary<string, IHamElement> keyValuePairs;
    private bool isTopLevelObject;

    public HamType Type { get => HamType.Object; }

    public HamObject()
        => (keyValuePairs, isTopLevelObject) = (new(), true);

    public override string ToString() => Stringify();

    public bool SaveToFile(string directoryPath, string name, bool prettify)
        => SaveToFile(Path.Combine(directoryPath, name), prettify);

    public bool SaveToFile(string fileName, bool prettify)
    {
        string path = $"{fileName}.ham";
        string content = prettify ? StringifyPretty() : Stringify();

        try
        {
            File.WriteAllText(path, content);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public string Stringify(string? key = null)
    {
        StringBuilder builder = new();

        if (!isTopLevelObject)
        {
            if(key is not null)
                builder.Append($"{key}:obj=");
            builder.Append('{');
        }

        foreach (string k in keyValuePairs.Keys)
            builder.Append(keyValuePairs[k].Stringify(k));

        if (!isTopLevelObject)
        {
            builder.Append('}');
            if (key is not null)
                builder.Append(';');
        }
        

        return builder.ToString();
    }

    public string StringifyPretty(string? key = null)
        => ((IHamElement)this).StringifyPretty(0, key);

    string IHamElement.StringifyPretty(int indentLevel, string? key)
    {
        StringBuilder builder = new();
        string indent = new('\t', indentLevel);
        if (!isTopLevelObject)
        {
            if (key is not null)
                builder.Append($"{indent}{key}:obj = ");
            builder.Append('{');
            builder.Append('\n');
        }

        foreach (string k in keyValuePairs.Keys)
        {
            builder.Append($"{keyValuePairs[k].StringifyPretty(isTopLevelObject ? indentLevel : indentLevel + 1, k)}\n");
        }

        if (!isTopLevelObject)
        {
            builder.Append($"{indent}}}");
            if (key is not null)
                builder.Append(';');
        }

        return builder.ToString();
    }

    public HamObject AddString(string key, string value) => AddPrimitive(key, value);
    public HamObject AddULong(string key, ulong value) => AddPrimitive(key, value);
    public HamObject AddLong(string key, long value) => AddPrimitive(key, value);
    public HamObject AddUInt(string key, uint value) => AddPrimitive(key, value);
    public HamObject AddInt(string key, int value) => AddPrimitive(key, value);
    public HamObject AddUShort(string key, ushort value) => AddPrimitive(key, value);
    public HamObject AddShort(string key, short value) => AddPrimitive(key, value);
    public HamObject AddByte(string key, byte value) => AddPrimitive(key, value);
    public HamObject AddSByte(string key, sbyte value) => AddPrimitive(key, value);
    public HamObject AddFloat(string key, float value) => AddPrimitive(key, value);
    public HamObject AddDouble(string key, double value) => AddPrimitive(key, value);
    public HamObject AddBoolean(string key, bool value) => AddPrimitive(key, value);
    public HamObject AddArray<T>(string key, HamArray<T> array) where T : IHamElement => AddValue(key, array);
    public HamObject AddArray<T>(string key, HamPrimitiveArray<T> array) => AddValue(key, array);
    public HamObject AddObject(string key, HamObject obj) => AddValue(key, obj);

    public bool? GetBoolean(string key) => GetValue<bool>(key)?.Item1;
    public ulong? GetULong(string key) => GetValue<ulong>(key)?.Item1;
    public long? GetLong(string key) => GetValue<long>(key)?.Item1;
    public uint? GetUInt(string key) => GetValue<uint>(key)?.Item1;
    public int? GetInt(string key) => GetValue<int>(key)?.Item1;
    public ushort? GetUShort(string key) => GetValue<ushort>(key)?.Item1;
    public short? GetShort(string key) => GetValue<short>(key)?.Item1;
    public byte? GetByte(string key) => GetValue<byte>(key)?.Item1;
    public sbyte? GetSByte(string key) => GetValue<sbyte>(key)?.Item1;
    public float? GetFloat(string key) => GetValue<float>(key)?.Item1;
    public double? GetDouble(string key) => GetValue<double>(key)?.Item1;
    public string? GetString(string key) => GetValue<string>(key)?.Item1;

    public HamPrimitiveArray<bool>? GetBooleanArray(string key) => GetPrimitiveArray<bool>(key);
    public HamPrimitiveArray<ulong>? GetULongArray(string key) => GetPrimitiveArray<ulong>(key);
    public HamPrimitiveArray<long>? GetLongArray(string key) => GetPrimitiveArray<long>(key);
    public HamPrimitiveArray<uint>? GetUIntArray(string key) => GetPrimitiveArray<uint>(key);
    public HamPrimitiveArray<int>? GetIntArray(string key) => GetPrimitiveArray<int>(key);
    public HamPrimitiveArray<ushort>? GetUShortArray(string key) => GetPrimitiveArray<ushort>(key);
    public HamPrimitiveArray<short>? GetShortArray(string key) => GetPrimitiveArray<short>(key);
    public HamPrimitiveArray<byte>? GetByteArray(string key) => GetPrimitiveArray<byte>(key);
    public HamPrimitiveArray<sbyte>? GetSByteArray(string key) => GetPrimitiveArray<sbyte>(key);
    public HamPrimitiveArray<float>? GetFloatArray(string key) => GetPrimitiveArray<float>(key);
    public HamPrimitiveArray<double>? GetDoubleArray(string key) => GetPrimitiveArray<double>(key);
    public HamPrimitiveArray<string>? GetStringArray(string key) => GetPrimitiveArray<string>(key);
    public HamArray<HamObject>? GetObjectArray(string key) => GetArray<HamObject>(key);
    public HamObject? GetObject(string key)
        => keyValuePairs.TryGetValue(key, out IHamElement? value) &&
            value is HamObject obj ? obj : null;

    internal HamObject SetAsNotTopLevel()
    {
        isTopLevelObject = false;
        return this;
    }

    internal HamArray<T>? GetArray<T>(string key) where T : IHamElement
        => keyValuePairs.TryGetValue(key, out IHamElement? value) &&
            value is HamArray<T> arr ? arr : null;

    internal HamPrimitiveArray<T>? GetPrimitiveArray<T>(string key)
        => keyValuePairs.TryGetValue(key, out IHamElement? value) &&
            value is HamPrimitiveArray<T> arr ? arr : null;

    internal ValueTuple<T>? GetValue<T>(string key)
        => keyValuePairs.TryGetValue(key, out IHamElement? value) &&
            value is HamPrimitive<T> primitive ? new(primitive.Value) : null;

    internal HamObject AddValue(string key, IHamElement value)
    {
        if (value is HamObject obj)
            obj.SetAsNotTopLevel();

        if(keyValuePairs.ContainsKey(key))
        {
            keyValuePairs[key] = value;
            return this;
        }

        keyValuePairs.Add(key, value);
        return this;
    }

    internal HamObject AddPrimitive<T>(string key, T value)
        => AddValue(key, new HamPrimitive<T>(value));
}
