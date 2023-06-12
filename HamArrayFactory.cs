namespace Hamester.Serialization.Ham;


public static class HamArrayFactory
{
    public static HamPrimitiveArray<bool> CreateBooleanArray() => new();
    public static HamPrimitiveArray<ulong> CreateULongArray() => new();
    public static HamPrimitiveArray<long> CreateLongArray() => new();
    public static HamPrimitiveArray<uint> CreateUIntArray() => new();
    public static HamPrimitiveArray<int> CreateIntArray() => new();
    public static HamPrimitiveArray<ushort> CreateUShortArray() => new();
    public static HamPrimitiveArray<short> CreateShortArray() => new();
    public static HamPrimitiveArray<byte> CreateByteArray() => new();
    public static HamPrimitiveArray<sbyte> CreateSByteArray() => new();
    public static HamPrimitiveArray<float> CreateFloatArray() => new();
    public static HamPrimitiveArray<double> CreateDoubleArray() => new();
    public static HamPrimitiveArray<string> CreateStringArray() => new();
    public static HamArray<HamObject> CreateObjectArray() => new(new(), HamType.Object);

    public static HamPrimitiveArray<bool> ToHamArray(this IEnumerable<bool> enumerable) => new(enumerable);
    public static HamPrimitiveArray<ulong> ToHamArray(this IEnumerable<ulong> enumerable) => new(enumerable);
    public static HamPrimitiveArray<long> ToHamArray(this IEnumerable<long> enumerable) => new(enumerable);
    public static HamPrimitiveArray<uint> ToHamArray(this IEnumerable<uint> enumerable) => new(enumerable);
    public static HamPrimitiveArray<int> ToHamArray(this IEnumerable<int> enumerable) => new(enumerable);
    public static HamPrimitiveArray<ushort> ToHamArray(this IEnumerable<ushort> enumerable) => new(enumerable);
    public static HamPrimitiveArray<short> ToHamArray(this IEnumerable<short> enumerable) => new(enumerable);
    public static HamPrimitiveArray<byte> ToHamArray(this IEnumerable<byte> enumerable) => new(enumerable);
    public static HamPrimitiveArray<sbyte> ToHamArray(this IEnumerable<sbyte> enumerable) => new(enumerable);
    public static HamPrimitiveArray<float> ToHamArray(this IEnumerable<float> enumerable) => new(enumerable);
    public static HamPrimitiveArray<double> ToHamArray(this IEnumerable<double> enumerable) => new(enumerable);
    public static HamPrimitiveArray<string> ToHamArray(this IEnumerable<string> enumerable) => new(enumerable);
    public static HamArray<HamObject> ToHamArray(this IEnumerable<HamObject> enumerable) => new(enumerable.Select(x => x.SetAsNotTopLevel()), HamType.Object);
}
