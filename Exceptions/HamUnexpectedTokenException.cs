using Hamester.Serialization.Ham.Parsing;

namespace Hamester.Serialization.Ham.Exceptions;

internal class HamUnexpectedTokenException : Exception
{
    public HamUnexpectedTokenException(HamTokenType found, HamTokenType expected)
        : base($"Unexpected token {found}, expected {expected}") { }
}
