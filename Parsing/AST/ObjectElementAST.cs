namespace Hamester.Serialization.Ham.Parsing.AST;

internal record ObjectElementAST(string Identifier, List<HamElementAST> Elements) : HamElementAST(Identifier, HamType.Object);

