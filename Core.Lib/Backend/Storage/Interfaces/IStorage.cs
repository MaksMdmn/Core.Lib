using Core.Lib.Backend.Common.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Lib.Backend.Storage.Interfaces
{
    public interface IStorage
    { 
        void Save<T>(T item) where T : DtoBase, new();

        T Load<T>(string key) where T : DtoBase, new();

        IEnumerable<T> LoadAll<T>() where T : DtoBase, new();

        void Remove<T>(string key) where T : DtoBase, new();
    }
}
