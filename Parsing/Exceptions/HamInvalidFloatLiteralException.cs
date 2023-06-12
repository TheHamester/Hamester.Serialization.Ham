namespace Hamester.Serialization.Ham.Parsing.Exceptions;

internal class HamInvalidFloatLiteralException : Exception
{
    public HamInvalidFloatLiteralException(string literal) : base($"{literal} is not a valid float literal") { }
}
