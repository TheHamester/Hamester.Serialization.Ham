namespace Hamester.Serialization.Ham.Exceptions;

internal static class ExceptionTypes
{
    public static bool IsFileError(Exception ex) =>
        ex is ArgumentException
        or PathTooLongException
        or IOException
        or UnauthorizedAccessException
        or NotSupportedException;

    public static bool IsParsingError(Exception ex) =>
        ex is HamInvalidFloatLiteralException
        or HamInvalidSymbolException
        or HamUnexpectedEndOfFileException
        or HamUnexpectedTokenException;
}
