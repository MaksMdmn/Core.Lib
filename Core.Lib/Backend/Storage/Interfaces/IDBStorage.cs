using System;
using System.Collections.Generic;
using Core.Lib.Backend.Common.Abstract;
using Core.Lib.Backend.Common.Pattern.Interfaces;
using Core.Lib.Web.Services;

namespace Core.Lib.Backend.Storage.Interfaces
{
    public interface IDBStorage<TDBConfig> : IStorage, IService, IDisposable, IConfigurable<TDBConfig> 
                                            where TDBConfig : struct, IDBConfiguration
    {
        void SaveAll<T>(IEnumerable<T> item) where T : DtoBase, new();
        void RemoveAll<T>() where T : DtoBase, new();

        IEnumerable<T> LoadMany<T, TFilter>(TFilter filter) where T : DtoBase, new() where TFilter : IDBFilter<T>;

        void RemoveMany<T, TFilter>(TFilter filter) where T : DtoBase, new() where TFilter : IDBFilter<T>;
    }
}
