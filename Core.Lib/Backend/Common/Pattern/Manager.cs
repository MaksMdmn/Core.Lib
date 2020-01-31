using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Core.Lib.Backend.Common.Abstract.Interfaces;
using Core.Lib.Backend.Common.Pattern;
using NLog;

namespace Core.Lib.Management
{
    public abstract class Manager<TManager, TManagedItem> : Singleton<TManager>, 
                                                            IManager<TManagedItem, object>,
                                                            IEnumerable<TManagedItem>
                                                            where TManager : class, new()
                                                            where TManagedItem : IUnique<string>
    {
        //TODO: standart error hadnling here? new entity? how to work?

        protected readonly ILogger Logger = LogManager.GetLogger(typeof(TManager).Name);

        protected readonly ConcurrentDictionary<object, TManagedItem> Items;

        public string Uid => throw new NotImplementedException();

        protected Manager()
        {
            Items = new ConcurrentDictionary<object, TManagedItem>();
        }

        public virtual void Initialize() { }

        public TManagedItem this[object key]
        {
            get
            {
                if (Items.TryGetValue(key, out TManagedItem result))
                {
                    return result;
                }

                return default;
            }
        }

        public virtual object AddItem(TManagedItem item)
        {
            object key = RetrieveItemKey(item);

            Items.TryAdd(key, item);

            return key;
        }

        public virtual void RemoveItem(TManagedItem item)
        {
            object key = RetrieveItemKey(item);

            RemoveItem(key);
        }

        public virtual void RemoveItem(object key)
        {
            Items.TryRemove(key, out TManagedItem item);
        }

        public IEnumerator<TManagedItem> GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }

        public TManagedItem GetItem(object key)
        {
            Items.TryGetValue(key, out TManagedItem item);

            return item;
        }


        protected virtual object RetrieveItemKey(TManagedItem item)
        {
            return item.Uid ?? Guid.NewGuid().ToString();
        }


        protected virtual void OnCatchedExceptionOccurred(
            Exception ex, 
            LogLevel logMessageLevel, 
            string message = null, 
            LogLevel logExceptionLevel = null, 
            bool rethrow = true)
        {
            Logger.Log(logMessageLevel, message);

            if (logExceptionLevel != null)
            {
                Logger.Log(logExceptionLevel, ex.Message);
            }

            if (rethrow)
            {
                throw ex;
            }
        }

    }
}
