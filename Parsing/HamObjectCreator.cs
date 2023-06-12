using Hamester.Serialization.Ham.Parsing.AST;
using Hamester.Serialization.Ham.Exceptions;
using System.Diagnostics;

namespace Hamester.Serialization.Ham.Parsing;

internal class HamObjectCreator
{
    private readonly HamParser _parser;

    public HamObjectCreator()
        => _parser = new();

    public HamParseResult BuildObject(string hamText)
    {
        HamElementAST ast;
        try
        {
            ast = _parser.Parse(hamText);
        }
        catch(Exception ex) when (ExceptionTypes.IsParsingError(ex)) 
        {
            return new(ex);
        }


        return new(CreateObject((ObjectElementAST)ast)); 
    }

    private HamObject CreateObject(ObjectElementAST objectElement)
    {
        HamObject obj = new();
        foreach (var el in objectElement.Elements)
        {
            if (el is ObjectElementAST objectElement1)
            {
                obj.AddObject(objectElement1.Identifier, CreateObject(objectElement1));
                continue;
            }

            if (el is PrimitiveElementAST primitiveElement)
            {
                AddPrimitiveElement(primitiveElement, obj);
                continue;
            }

            if(el is ArrayElementAST arrayElement)
            {
                AddArrayElement(arrayElement, obj);
                continue;
            }
        }

        return obj;
    }

    private void AddArrayElement(ArrayElementAST arrayElement, HamObject hamObject)
    {
        if(HamTypes.IsPrimitiveType(arrayElement.Type))
        {
            switch (arrayElement.Type)
            {
                case HamType.String:  hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, x => x          ).ToHamArray()); break;
                case HamType.Int64:   hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, Convert.ToInt64 ).ToHamArray()); break;
                case HamType.Int32:   hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, Convert.ToInt32 ).ToHamArray()); break;
                case HamType.Int16:   hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, Convert.ToInt16 ).ToHamArray()); break;
                case HamType.Int8:    hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, Convert.ToSByte ).ToHamArray()); break;
                case HamType.UInt64:  hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, Convert.ToUInt64).ToHamArray()); break;
                case HamType.UInt32:  hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, Convert.ToInt32 ).ToHamArray()); break;
                case HamType.UInt16:  hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, Convert.ToInt16 ).ToHamArray()); break;
                case HamType.UInt8:   hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, Convert.ToByte  ).ToHamArray()); break;
                case HamType.Double:  hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, Convert.ToDouble).ToHamArray()); break;
                case HamType.Float:   hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, Convert.ToSingle).ToHamArray()); break;
                case HamType.Boolean: hamObject.AddArray(arrayElement.Identifier, CollectPrimitiveArrayElements(arrayElement.Elements, x => x is "True").ToHamArray()); break;
                default: throw new UnreachableException();
            }

            return;
        }


        hamObject.AddArray(arrayElement.Identifier, CollectHamObjectArrayElements(arrayElement.Elements).ToHamArray());
    }

    private static void AddPrimitiveElement(PrimitiveElementAST primitiveElement, HamObject hamObject)
    {
        switch (primitiveElement.Type)
        {
            case HamType.String:  AddPrimitive(primitiveElement, hamObject.AddString,  x => x.Trim('"'));                            break;
            case HamType.Int64:   AddPrimitive(primitiveElement, hamObject.AddLong,    Convert.ToInt64);                             break;
            case HamType.Int32:   AddPrimitive(primitiveElement, hamObject.AddInt,     Convert.ToInt32);                             break;
            case HamType.Int16:   AddPrimitive(primitiveElement, hamObject.AddShort,   Convert.ToInt16);                             break;
            case HamType.Int8:    AddPrimitive(primitiveElement, hamObject.AddSByte,   Convert.ToSByte);                             break;
            case HamType.UInt64:  AddPrimitive(primitiveElement, hamObject.AddULong,   Convert.ToUInt64);                            break;
            case HamType.UInt32:  AddPrimitive(primitiveElement, hamObject.AddUInt,    Convert.ToUInt32);                            break;
            case HamType.UInt16:  AddPrimitive(primitiveElement, hamObject.AddUShort,  Convert.ToUInt16);                            break;
            case HamType.UInt8:   AddPrimitive(primitiveElement, hamObject.AddByte,    Convert.ToByte);                              break;
            case HamType.Float:   AddPrimitive(primitiveElement, hamObject.AddFloat,   x => Convert.ToSingle(x.Replace('.', ',')));  break;
            case HamType.Double:  AddPrimitive(primitiveElement, hamObject.AddDouble,  x => Convert.ToDouble(x.Replace('.', ',')));  break;
            case HamType.Boolean: AddPrimitive(primitiveElement, hamObject.AddBoolean, x => x is "True");                            break;
            default: throw new UnreachableException();
        }
    }

    private static void AddPrimitive<T>(PrimitiveElementAST primitiveElement, Func<string, T, HamObject> add, Func<string, T> converter)
        => add(primitiveElement.Identifier, converter(primitiveElement.Value));

    private static List<T> CollectPrimitiveArrayElements<T>(List<HamElementAST> arrayElement, Func<string, T> converter)
    {
        List<T> list = new();
        foreach (var el in arrayElement)
            list.Add(converter(((PrimitiveElementAST)el).Value));

        return list;
    }

    private List<HamObject> CollectHamObjectArrayElements(List<HamElementAST> arrayElement)
    {
        List<HamObject> list = new();
        foreach (var el in arrayElement)
            list.Add(CreateObject((ObjectElementAST)el));

        return list;
    }
}
