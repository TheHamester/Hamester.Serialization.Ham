namespace Hamester.Serialization.Ham.Parsing.Exceptions;

internal class HamUnexpectedTokenException : Exception
{
    public HamUnexpectedTokenException(HamTokenType found, HamTokenType expected)
        : base($"Unexpected token {found}, expected {expected}") { }
}
