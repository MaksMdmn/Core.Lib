using Core.Lib.Backend.Common.Abstract;
using System;

namespace Core.Lib.Backend.Storage.Interfaces
{
    public interface IDBFilter<T> where T: DtoBase, new()
    {
        Func<T, bool> EntitiesLookup { get; }
    }
}
