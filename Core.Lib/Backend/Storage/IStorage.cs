using Core.Lib.Backend.Common.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Lib.Backend.Storage
{
    public interface IStorage
    { 
        void Save<T>(T item) where T : DtoBase;

        T Load<T>(string key) where T : DtoBase;

        IEnumerable<T> LoadAll<T>() where T : DtoBase;

        void Remove<T>(string key) where T : DtoBase;
    }
}
