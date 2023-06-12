using System.Collections;

namespace Hamester.Serealization;

public class HamPrimitiveArray<T> : IHamElement, IHamCollection<T, HamPrimitiveArray<T>>, IHamArray, IEnumerable<T>, IEnumerable
{
    private readonly HamArray<HamPrimitive<T>> primitiveArray;

    public HamType Type { get => primitiveArray.Type; }

    public int Count => primitiveArray.Count;

    internal HamPrimitiveArray()
        => primitiveArray = new(new(), HamTypes.FromSystemType(typeof(T)));

    internal HamPrimitiveArray(IEnumerable<T> enumerable)
        => primitiveArray = new(enumerable.Select(val => new HamPrimitive<T>(val)), HamTypes.FromSystemType(typeof(T)));

    public T this[int idx]
    {
        get => primitiveArray[idx].Value;
        set => primitiveArray[idx] = value;
    }

    public override string ToString() => Stringify();


    public string Stringify(string? key = null) => primitiveArray.Stringify(key);

    public string StringifyPretty(string? key = null)
        => primitiveArray.StringifyPretty(key);

    string IHamElement.StringifyPretty(int indentLevel, string? key) 
        => ((IHamElement)primitiveArray).StringifyPretty(indentLevel, key);


    public HamPrimitiveArray<T> Add(T value)
    {
        primitiveArray.Add(value);
        return this;
    }

    public IEnumerator<T> GetEnumerator() => new Enumerator(primitiveArray);
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(primitiveArray);

    public HamPrimitiveArray<T> Clear()
    {
        primitiveArray.Clear();
        return this;
    }

    public bool Contains(T item) => primitiveArray.Contains(item);

    public HamPrimitiveArray<T> Remove(T item)
    {
        primitiveArray.Remove(item);
        return this;
    }

    public HamPrimitiveArray<T> RemoveAt(int index)
    {
        primitiveArray.RemoveAt(index);
        return this;
    }

    public int IndexOf(T item)
        => primitiveArray.IndexOf(item);

    public HamPrimitiveArray<T> Insert(int index, T item)
    {
        primitiveArray.Insert(index, item);
        return this;
    }

    public readonly struct Enumerator : IEnumerator, IEnumerator<T>
    {
        private readonly IEnumerator _primitiveArrayEnumerator;

        public object Current => (_primitiveArrayEnumerator.Current as HamPrimitive<T>?)!.Value.Value!;

        T IEnumerator<T>.Current => (_primitiveArrayEnumerator.Current as HamPrimitive<T>?)!.Value.Value!;

        internal Enumerator(HamArray<HamPrimitive<T>> primitiveArray)
            => _primitiveArrayEnumerator = primitiveArray.GetEnumerator();

        public bool MoveNext() 
            => _primitiveArrayEnumerator.MoveNext();

        public void Reset()
            => _primitiveArrayEnumerator.Reset();

        public void Dispose() { }
    }
}
