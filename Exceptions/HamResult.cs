namespace Hamester.Serialization.Ham.Exceptions;

public class HamResult
{
    public virtual bool IsSuccess => _ex is null;

    public bool IsError => _ex is not null;

    private readonly Exception? _ex;

    internal protected HamResult(Exception? ex)
        => _ex = ex;

    internal protected HamResult()
    => _ex = null;

    public string GetErrorMessage()
        => _ex is null ? "Operation was succesful" : _ex.Message;

    public static bool operator true(HamResult instance)
        => instance.IsSuccess;

    public static bool operator false(HamResult instance)
        => instance.IsError;
}
