namespace Hamester.Serialization.Ham;


public enum HamType
{
    Int64,
    UInt64,
    Int32,
    UInt32,
    Int16,
    UInt16,
    Int8,
    UInt8,
    Float,
    Double,
    String,
    Boolean,
    Object
}

internal static class HamTypes
{
    public static string GetStringFromType(HamType type) => type switch
    {
        HamType.UInt64 => "u64",
        HamType.Int64 => "i64",
        HamType.Float => "f32",
        HamType.Double => "f64",
        HamType.UInt32 => "u32",
        HamType.Int32 => "i32",
        HamType.UInt16 => "u16",
        HamType.Int16 => "i16",
        HamType.UInt8 => "u8",
        HamType.Int8 => "i8",
        HamType.Boolean => "bool",
        HamType.String => "str",
        HamType.Object => "obj",
        _ => throw new ArgumentException($"Invalid HamType {type}", nameof(type))
    };

    public static HamType GetTypeFromString(string str) => str switch
    {
        "u64" => HamType.UInt64,
        "i64" => HamType.Int64,
        "f32" => HamType.Float,
        "f64" => HamType.Double,
        "u32" => HamType.UInt32,
        "i32" => HamType.Int32,
        "u16" => HamType.UInt16,
        "i16" => HamType.Int16,
        "u8" => HamType.UInt8,
        "i8" => HamType.Int8,
        "bool" => HamType.Boolean,
        "str" => HamType.String,
        "obj" => HamType.Object,
        _ => throw new ArgumentException($"Invalid HamType string representation {str}", nameof(str))
    };

    private static HamType FromTypeCode(TypeCode code) => code switch
    {
        TypeCode.Boolean => HamType.Boolean,
        TypeCode.UInt32 => HamType.UInt32,
        TypeCode.Int32 => HamType.Int32,
        TypeCode.UInt64 => HamType.UInt64,
        TypeCode.Int64 => HamType.Int64,
        TypeCode.Byte => HamType.UInt8,
        TypeCode.SByte => HamType.Int8,
        TypeCode.UInt16 => HamType.UInt16,
        TypeCode.Int16 => HamType.Int16,
        TypeCode.String => HamType.String,
        TypeCode.Single => HamType.Float,
        TypeCode.Double => HamType.Double,
        _ => throw new ArgumentException($"There's no conversion from TypeCode {code} to HamType", nameof(code))
    };

    public static HamType FromSystemType(Type type)
        => FromTypeCode(Type.GetTypeCode(type));

    public static bool IsIntegerType(HamType type)
        => type is HamType.UInt64 or HamType.UInt32 or HamType.UInt16 or HamType.UInt8 
        or HamType.Int64 or HamType.Int32 or HamType.Int16 or HamType.Int8;

    public static bool IsPrimitiveType(HamType type)
        => !IsObjectType(type);

    public static bool IsFloatType(HamType type)
        => type is HamType.Float or HamType.Double;

    public static bool IsBooleanType(HamType type)
        => type is HamType.Boolean;

    public static bool IsStringType(HamType type)
        => type is HamType.String;

    public static bool IsObjectType(HamType type)
        => type is HamType.Object;
}
