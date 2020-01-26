namespace Core.Lib.Backend.Common.Abstract.Interfaces
{
    public interface IUnique<TKey>
    {
        TKey Uid { get; }
    }
}
