﻿namespace Hamester.Serealization.Parsing.Exceptions;

internal class HamInvalidFloatLiteralException : Exception
{
    public HamInvalidFloatLiteralException(string literal) : base($"{literal} is not a valid float literal") { }
}
