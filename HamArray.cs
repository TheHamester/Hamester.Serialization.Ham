using System.Text;
using System.Collections;

namespace Hamester.Serealization;

public interface IHamArray { }

public class HamArray<T> : IHamElement, IHamCollection<T, HamArray<T>>, IHamArray, IEnumerable<T>, IEnumerable where T : IHamElement
{
    private readonly List<T> elements;

    public HamType Type { get; }

    internal HamArray()
        : this(new(), HamTypes.FromSystemType(typeof(T))) { }

    internal HamArray(IEnumerable<T> enumerable) 
        : this(enumerable, HamTypes.FromSystemType(typeof(T))) { }

    internal HamArray(IEnumerable<T> enumerable, HamType type) 
        : this(enumerable.ToList(), type) { }

    internal HamArray(List<T> elements, HamType type)
        => (this.elements, Type) = (elements, type);

    public T this[int i]
    {
        get => elements[i];
        set => elements[i] = value;
    }

    public int Count => elements.Count;

    public override string ToString() => Stringify();

    public IEnumerator<T> GetEnumerator() => elements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => elements.GetEnumerator();

    public HamArray<T> Add(T element)
    {
        if (element is HamObject obj)
        {
            elements.Add((T)(object)obj.SetAsNotTopLevel());
            return this;
        }

        elements.Add(element);
        return this;
    }

    public HamArray<T> Remove(T element)
    {
        elements.Remove(element);
        return this;
    }

    public HamArray<T> RemoveAt(int idx)
    {
        elements.RemoveAt(idx);
        return this;
    }

    string IHamElement.StringifyPretty(int indentLevel, string? key)
    {
        StringBuilder builder = new();
        string indent = new('\t', indentLevel);

        if (key is not null)
            builder.Append($"{indent}{key}:{HamTypes.GetStringFromType(Type)}[] = ");

        builder.Append('[');
        StringifyElementsPretty(indentLevel, builder);
        builder.Append($"]{(key is null ? "" : ";")}");
        return builder.ToString();
    }

    public string StringifyPretty(string? key = null)
        => ((IHamElement)this).StringifyPretty(0, key);

    public string Stringify(string? key = null)
    {
        StringBuilder builder = new();
        if (key is not null)
            builder.Append($"{key}:{HamTypes.GetStringFromType(Type)}[]=");

        builder.Append('[');
        StringifyElements(builder);
        builder.Append($"]{(key is null ? "" : ";")}");
        return builder.ToString();
    }

    private string StringifyElementsPretty(int indentLevel, StringBuilder builder)
        => Type is HamType.Object
        ? StringifyObjectElementsPretty(indentLevel, builder)
        : StringifyPrimitiveElementsPretty(builder);

    private string StringifyPrimitiveElementsPretty(StringBuilder builder) 
    {
        if (elements.Count == 0)
            return string.Empty;

        builder.Append(elements[0].Stringify());
        for (int i = 1; i < elements.Count; i++)
        {
            builder.Append(", ");
            builder.Append(elements[i].Stringify());
        }

        return builder.ToString();
    }

    private string StringifyObjectElementsPretty(int indentLevel, StringBuilder builder) 
    {
        if (elements.Count == 0)
            return string.Empty;

        string indent = new('\t', indentLevel+1);

        builder.Append('\n');
        builder.Append(indent);
        builder.Append(elements[0].StringifyPretty(indentLevel + 1));
        for (int i = 1; i < elements.Count; i++)
        {
            builder.Append($", \n");
            builder.Append(indent);
            builder.Append(elements[i].StringifyPretty(indentLevel + 1));
        }

        builder.Append('\n');
        return builder.ToString();
    }

    private string StringifyElements(StringBuilder builder)
    {
        if (elements.Count == 0)
            return string.Empty;


        builder.Append(elements[0].Stringify());
        for (int i = 1; i < elements.Count; i++)
        {
            builder.Append(',');
            builder.Append(elements[i].Stringify());
        }

        return builder.ToString();
    }

    public HamArray<T> Clear()
    {
        elements.Clear();
        return this;
    }

    public bool Contains(T item) => elements.Contains(item);

    public int IndexOf(T item) => elements.IndexOf(item);

    public HamArray<T> Insert(int index, T item)
    {
        elements.Insert(index, item);
        return this;
    }
}
