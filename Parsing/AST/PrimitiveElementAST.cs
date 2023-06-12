namespace Hamester.Serealization.Parsing.AST;

internal record PrimitiveElementAST(string Identifier, HamType Type, string Value) : HamElementAST(Identifier, Type);
