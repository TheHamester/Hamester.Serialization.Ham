namespace Hamester.Serialization.Ham.Parsing.Exceptions;

internal class HamInvalidSymbolException : Exception
{
    public HamInvalidSymbolException(char symbol) : base($"Symbol {symbol} is invalid") { }
}
