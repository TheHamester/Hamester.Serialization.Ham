namespace Hamester.Serialization.Ham.Parsing.AST;

internal record ArrayElementAST(string Identifier, HamType Type, List<HamElementAST> Elements) : HamElementAST(Identifier, Type);

