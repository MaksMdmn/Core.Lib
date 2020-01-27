using System;
using System.Collections.Generic;
using Core.Lib.Backend.Common.Abstract;
using Core.Lib.Backend.Common.Pattern.Interfaces;
using Core.Lib.Web.Services;

namespace Core.Lib.Backend.Storage
{
    public interface IDBStorage<TDBConfig> : IStorage, IService, IDisposable, IConfigurable<TDBConfig> 
                                            where TDBConfig : struct, IDBConfiguration
    {
        void SaveAll<T>(IEnumerable<T> item) where T : DtoBase;

        IEnumerable<T> LoadMany<T>(IDBFilter filter) where T : DtoBase;

        IEnumerable<T> RemoveMany<T>(IDBFilter filter) where T : DtoBase;

        void RemoveAll<T>() where T : DtoBase;
    }
}
