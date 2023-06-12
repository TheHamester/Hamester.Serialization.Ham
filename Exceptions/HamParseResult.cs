using System.Diagnostics.CodeAnalysis;

namespace Hamester.Serialization.Ham.Exceptions;

public sealed class HamParseResult : HamResult
{
    public HamObject? ParsedObject { get; }

    [MemberNotNullWhen(true, nameof(ParsedObject))]
    public override bool IsSuccess => ParsedObject is not null;

    internal HamParseResult(HamObject obj) : base(null)
        => ParsedObject = obj;

    public HamParseResult(Exception ex) : base(ex)
        => ParsedObject = null;

    public static implicit operator HamObject?(HamParseResult instance)
        => instance.ParsedObject;
}
