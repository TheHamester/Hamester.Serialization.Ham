namespace Hamester.Serealization.Parsing.Exceptions;

internal class HamUnexpectedEndOfFileException : Exception
{
    public HamUnexpectedEndOfFileException() : base("Unexpected End of file") { }
}
