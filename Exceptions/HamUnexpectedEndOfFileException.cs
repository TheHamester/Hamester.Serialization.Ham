namespace Hamester.Serialization.Ham.Exceptions;

internal class HamUnexpectedEndOfFileException : Exception
{
    public HamUnexpectedEndOfFileException() : base("Unexpected End of file") { }
}
