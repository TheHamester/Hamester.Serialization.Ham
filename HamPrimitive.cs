using System.Globalization;

namespace Hamester.Serealization;

internal readonly struct HamPrimitive<T> : IHamElement
{
    public T Value { get; init; }
    public HamType Type { get; }

    internal HamPrimitive(T value)
    {
        Value = value;
        Type = HamTypes.FromSystemType(typeof(T));
    }

    public string Stringify(string? key = null)
    {
        string formattedValue = Type switch
        {
            HamType.Double => (Value as double?)!.Value.ToString("G", CultureInfo.InvariantCulture),
            HamType.Float => (Value as float?)!.Value.ToString("G", CultureInfo.InvariantCulture),
            HamType.String => $"\"{Value}\"",
            _ => Value!.ToString()!
        };

        return key is null ? formattedValue : $"{key}:{HamTypes.GetStringFromType(Type)}={formattedValue};";
    }

    string IHamElement.StringifyPretty(int indentLevel, string? key)
    {
        string formattedValue = Type switch
        {
            HamType.Double => (Value as double?)!.Value.ToString("G", CultureInfo.InvariantCulture),
            HamType.Float => (Value as float?)!.Value.ToString("G", CultureInfo.InvariantCulture),
            HamType.String => $"\"{Value}\"",
            _ => Value!.ToString()!
        };

        return key is null ? formattedValue : $"{new string('\t', indentLevel)}{key}:{HamTypes.GetStringFromType(Type)} = {formattedValue};";
    }

    public string StringifyPretty(string? key = null)
        => ((IHamElement)this).StringifyPretty(0, key);

    public static implicit operator HamPrimitive<T>(T value) => new(value);

    public override string ToString() => Stringify();
}
