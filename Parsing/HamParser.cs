using Hamester.Serealization.Parsing.AST;
using Hamester.Serealization.Parsing.Exceptions;

namespace Hamester.Serealization.Parsing;


internal class HamParser
{
    private readonly HamLexer _lexer;
    private List<HamToken>? _tokens;
    private int _currentTokenIndex;

    public HamParser()
        => (_lexer, _tokens, _currentTokenIndex) = (new(), null, 0);

    private HamToken Consume(HamTokenType type)
    {
        if (_tokens![_currentTokenIndex].Type != type)
            throw new HamUnexpectedTokenException(_tokens![_currentTokenIndex].Type, type);

        var token = _tokens[_currentTokenIndex];
        _currentTokenIndex++;

        return token;
    }

    private HamToken? ConsumeOptional(HamTokenType type)
    {
        if (_tokens![_currentTokenIndex].Type == type)
        {
            _currentTokenIndex++;
            return _tokens[_currentTokenIndex];
        }

        return null;
    }

    private bool Peek(HamTokenType type)
        => _tokens![_currentTokenIndex].Type == type;

    public HamElementAST Parse(string hamText)
    {
        _tokens = _lexer.CollectTokens(hamText);
        _currentTokenIndex = 0;

        return ParseObject();
    }

    private HamElementAST ParseObject(string identifier = "", bool topLevel = true)
    {
        if (!topLevel)
            Consume(HamTokenType.LeftCurlyBracket);

        List<HamElementAST> objectElements = ParseObjectElements(topLevel);

        if(!topLevel)
            Consume(HamTokenType.RightCurlyBracket);

        return new ObjectElementAST(identifier, objectElements);
    }

    private List<HamElementAST> ParseObjectElements(bool isTopLevel) 
    {
        List<HamElementAST> elements = new();

        while (isTopLevel && _currentTokenIndex < _tokens!.Count || (!isTopLevel && !Peek(HamTokenType.RightCurlyBracket)))
        {
            elements.Add(ParseElement());
            Consume(HamTokenType.Semicolon);
        }

        return elements;
    }

    private HamElementAST ParseElement()
    {
        string identifier = Consume(HamTokenType.Identifier).Text;
        Consume(HamTokenType.Colon);

        HamType elementType = HamTypes.GetTypeFromString(Consume(HamTokenType.Type).Text);

        bool isArray 
            = ConsumeOptional(HamTokenType.LeftSquareBracket) is not null 
            && ConsumeOptional(HamTokenType.RightSquareBracket) is not null;

        Consume(HamTokenType.EqualSign);

        if(isArray)
            return ParseArray(identifier, elementType);
        

        return ParseObjectOrLiteral(identifier, elementType);
    } 

    private HamElementAST ParseObjectOrLiteral(string identifier, HamType elementType)
    {
        if (HamTypes.IsObjectType(elementType))  return ParseObject(identifier, false);
        if (HamTypes.IsIntegerType(elementType)) return ParseLiteralOfType(identifier, elementType, HamTokenType.IntegerLiteral);
        if (HamTypes.IsFloatType(elementType))   return ParseLiteralOfType(identifier, elementType, HamTokenType.FloatLiteral);
        if (HamTypes.IsBooleanType(elementType)) return ParseLiteralOfType(identifier, elementType, HamTokenType.BooleanLiteral);
        if (HamTypes.IsStringType(elementType))  return ParseLiteralOfType(identifier, elementType, HamTokenType.StringLiteral);

        throw new ArgumentException($"Not a valid HamType {elementType}", nameof(elementType));
    }
    
    private HamElementAST ParseLiteralOfType(string identifier, HamType elementType, HamTokenType tokenType) 
    {
        string value = Consume(tokenType).Text;
        return new PrimitiveElementAST(identifier, elementType, value);
    }

    private HamElementAST ParseArray(string identifier, HamType type) 
    {
        Consume(HamTokenType.LeftSquareBracket);
        List<HamElementAST> arrayElements = ParseArrayElements(type);
        Consume(HamTokenType.RightSquareBracket);

        return new ArrayElementAST(identifier, type, arrayElements);
    }

    private List<HamElementAST> ParseArrayElements(HamType type)
    {
        List<HamElementAST> arrayElements = new();

        if (Peek(HamTokenType.RightSquareBracket))
            return arrayElements;

        arrayElements.Add(ParseObjectOrLiteral(string.Empty, type));

        while (!Peek(HamTokenType.RightSquareBracket))
        {
            Consume(HamTokenType.Comma);
            arrayElements.Add(ParseObjectOrLiteral(string.Empty, type));
        }

        return arrayElements;
    }
}
