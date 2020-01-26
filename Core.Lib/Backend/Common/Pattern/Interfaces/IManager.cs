using Core.Lib.Common;

namespace Core.Lib.Management
{
    public interface IManager<TManagedItem, TKey> : IInitialize, IReset
    {
        TManagedItem this[TKey key] { get; }

        TKey AddItem(TManagedItem item);

        void RemoveItem(TKey key);

        void RemoveItem(TManagedItem item);
    }
}
