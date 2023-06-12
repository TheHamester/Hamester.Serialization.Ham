namespace Hamester.Serialization.Ham.Exceptions;

internal class HamInvalidSymbolException : Exception
{
    public HamInvalidSymbolException(char symbol) : base($"Symbol {symbol} is invalid") { }
}
