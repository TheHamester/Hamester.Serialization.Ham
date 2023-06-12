namespace Hamester.Serialization.Ham.Exceptions;

internal class HamInvalidFloatLiteralException : Exception
{
    public HamInvalidFloatLiteralException(string literal) : base($"{literal} is not a valid float literal") { }
}
