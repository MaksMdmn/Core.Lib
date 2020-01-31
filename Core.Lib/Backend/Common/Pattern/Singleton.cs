using System;
using System.Threading;

namespace Core.Lib.Backend.Common.Pattern
{
    public abstract class Singleton<T> where T: class
    {
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

        public static T Instance => LazyInstance.Value;

        protected Singleton()
        {

        }

        private static T CreateInstance() 
        {
            dynamic instance = Activator.CreateInstance(typeof(T), true);

            try
            {
                instance.OnInitializing();
            }
            catch (Exception)
            {

            }

            return instance as T;
        }

        protected virtual void OnInitializing()
        {

        }
    }
}
