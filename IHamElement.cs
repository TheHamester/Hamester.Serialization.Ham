namespace Hamester.Serealization;

public interface IHamElement
{
    public HamType Type { get; }
    public string Stringify(string? key = null);

    public string StringifyPretty(string? key = null);

    public string StringifyPretty(int indentLevel, string? key = null);
}
