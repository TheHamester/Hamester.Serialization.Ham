namespace Hamester.Serialization.Ham.Parsing.Exceptions;

internal class HamUnexpectedEndOfFileException : Exception
{
    public HamUnexpectedEndOfFileException() : base("Unexpected End of file") { }
}
