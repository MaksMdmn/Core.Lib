namespace Core.Lib.Management
{
    public interface IManager<TManagedItem, TKey>
    {
        TManagedItem this[TKey key] { get; }

        TKey AddItem(TManagedItem item);

        void RemoveItem(TKey key);

        void RemoveItem(TManagedItem item);
    }
}
