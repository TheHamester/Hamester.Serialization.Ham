using Hamester.Serialization.Ham.Exceptions;

namespace Hamester.Serialization.Ham.Parsing;

internal enum HamTokenType
{
    Identifier,
    Colon,
    Type,
    StringLiteral,
    IntegerLiteral,
    FloatLiteral,
    BooleanLiteral,
    LeftSquareBracket,
    RightSquareBracket,
    LeftParenthesy,
    RightParenthesy,
    LeftCurlyBracket,
    RightCurlyBracket,
    EqualSign,
    Comma,
    Semicolon
}

internal class HamLexer
{
    private const string LatinAlphabet = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
    private const string Digits = "0123456789";
    private readonly string[] Types = new[] { "obj", "u8", "i8", "u16", "i16", "u32","i32", "u64", "i64", "f32", "f64", "bool", "str" };

    private readonly List<HamToken> _tokens;
    private int _currentIndex;
    private string _hamText;
    private HamToken? _prevToken;

    public HamLexer() 
        => (_tokens, _currentIndex, _hamText, _prevToken) = (new(), 0, "", null);

    public List<HamToken> CollectTokens(string hamText)
    {
        _currentIndex = 0;
        _hamText = hamText;

        while(_currentIndex < _hamText.Length)
        {
            if (Whitespaces.All.Contains(_hamText[_currentIndex])) 
            {
                _currentIndex++;
                continue;
            }

            if(LatinAlphabet.Contains(_hamText[_currentIndex]))
            {
                _tokens.Add(CollectAlphanumericToken());
                continue;
            }

            if (Digits.Contains(_hamText[_currentIndex]) || _hamText[_currentIndex] is '-')
            {
                _tokens.Add(CollectNumberLiteral());
                continue;
            }

            _tokens.Add(_prevToken = _hamText[_currentIndex] switch
            {
                ':' => CollectFixedLengthToken(HamTokenType.Colon),
                '=' => CollectFixedLengthToken(HamTokenType.EqualSign),
                ',' => CollectFixedLengthToken(HamTokenType.Comma),
                '[' => CollectFixedLengthToken(HamTokenType.LeftSquareBracket),
                '(' => CollectFixedLengthToken(HamTokenType.LeftParenthesy),
                '{' => CollectFixedLengthToken(HamTokenType.LeftCurlyBracket),
                ']' => CollectFixedLengthToken(HamTokenType.RightSquareBracket),
                ')' => CollectFixedLengthToken(HamTokenType.RightParenthesy),
                '}' => CollectFixedLengthToken(HamTokenType.RightCurlyBracket),
                ';' => CollectFixedLengthToken(HamTokenType.Semicolon),
                '"' => CollectStringLiteral(),
                _ => throw new HamInvalidSymbolException(_hamText[_currentIndex])
            });
        }

        return _tokens;
    }

    private HamToken CollectAlphanumericToken()
    {
        int start = _currentIndex;
        while ((LatinAlphabet.Contains(_hamText[_currentIndex]) || Digits.Contains(_hamText[_currentIndex])) && _currentIndex < _hamText.Length)
            _currentIndex++;

        string collected = _hamText[start.._currentIndex];

        if (collected is "True" or "False")
            return new(HamTokenType.BooleanLiteral, collected);

        if (_prevToken is not null && _prevToken.Type is HamTokenType.Colon && Types.Contains(collected))
            return new(HamTokenType.Type, collected);

        return new(HamTokenType.Identifier, collected);
    }

    private HamToken CollectNumberLiteral()
    {
        HamTokenType literalType = HamTokenType.IntegerLiteral;
        int start = _currentIndex;
        bool dotFound = false;

        if (_hamText[_currentIndex] is '-')
            _currentIndex++;

        if (!Digits.Contains(_hamText[_currentIndex]))
            throw new HamInvalidFloatLiteralException(_hamText[start.._currentIndex]);

        while ((Digits.Contains(_hamText[_currentIndex]) || (dotFound = _hamText[_currentIndex] is '.')) && _currentIndex < _hamText.Length)
        {
            _currentIndex++;
            if (dotFound)
            {
                literalType = HamTokenType.FloatLiteral;
                break;
            }
        }

        if (dotFound)
        {
            if (_currentIndex >= _hamText.Length)
                throw new HamUnexpectedEndOfFileException();
            if (!Digits.Contains(_hamText[_currentIndex]))
                throw new HamInvalidFloatLiteralException(_hamText[start.._currentIndex]);

            while (Digits.Contains(_hamText[_currentIndex]) && _currentIndex < _hamText.Length)
                _currentIndex++;
        }

        return new(literalType, _hamText[start.._currentIndex]);
    }

    private HamToken CollectStringLiteral()
    {
        int start = _currentIndex;
        bool foundEof = false;

        _currentIndex++;
        while (_hamText[_currentIndex] is not '"' && !(foundEof = _currentIndex >= _hamText.Length))
            _currentIndex++;
        _currentIndex++;

        if (foundEof)
            throw new HamUnexpectedEndOfFileException();

        return new(HamTokenType.StringLiteral, _hamText[start.._currentIndex]);
    }

    private HamToken CollectFixedLengthToken(HamTokenType type, int tokenLength = 1)
    {
        HamToken token = new(type, tokenLength == 1 ? _hamText[_currentIndex].ToString() : _hamText[_currentIndex..tokenLength]);
        _currentIndex += tokenLength;
        return token;
    }
}
