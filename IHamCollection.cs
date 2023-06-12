namespace Hamester.Serealization;

internal interface IHamCollection<T, TSelf> where TSelf : IHamCollection<T, TSelf>
{
    int Count { get; }
    T this[int index] { get; set; }
    TSelf Add(T item);
    TSelf Clear();
    bool Contains(T item);
    TSelf Remove(T item);
    TSelf RemoveAt(int index);
    int IndexOf(T item);
    TSelf Insert(int index, T item);
}
