﻿namespace Hamester.Serialization.Ham.Parsing;

internal class Whitespaces
{
    /// <summary>All unicode characters where <c>White_Space=yes</c>, and are line breaks.</summary>
    public const string Breaking = "\n\v\f\r\u0085\u2028\u2029";

    /// <summary>All unicode characters where <c>White_Space=yes</c>, and are not a line break.</summary>
    public const string NonBreaking =
        "\u0009\u0020\u00A0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u202F\u205F\u3000";

    /// <summary>All unicode characters where <c>White_Space=no</c>, but appears to be whitespace.</summary>
    public const string Related = "\u180E\u200B\u200C\u200D\u2060\uFEFF";

    /// <summary>All unicode characters where <c>White_Space=yes</c>.</summary>
    public const string Unicode = $"{Breaking}{NonBreaking}";

    /// <summary>All unicode characters that appear to be whitespace.</summary>
    public const string All = $"{Unicode}{Related}";
}
