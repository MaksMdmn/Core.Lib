using Core.Lib.Backend.Common.Abstract;
using Core.Lib.Backend.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Lib.Backend.Storage.EF
{
    public abstract class EFStorage<TDbContext> : IDBStorage<EFConfig> where TDbContext : DbContext
    {
        protected TDbContext Context { get; private set; }

        public EFConfig Config { get; }

        public EFStorage(EFConfig config)
        {
            Config = config;

            Context = InitializeContext(config);
        }

        protected abstract TDbContext InitializeContext(EFConfig config);

        public void Dispose()
        {
            Context.Dispose();
        }

        public T Load<T>(string key) where T : DtoBase, new() => Context.Set<T>().FirstOrDefault(dbObj => dbObj.Uid.Equals(key));

        public IEnumerable<T> LoadAll<T>() where T : DtoBase, new() => Context.Set<T>();

        public void Remove<T>(string key) where T : DtoBase, new()
        {
            T template = new T() { Uid = key };

            if (key != null) { Context.Remove(template); }

            Context.SaveChanges();
        }

        public void Save<T>(T item) where T : DtoBase, new()
        {
            if (item == null)
            {
                return;
            }

            Context.Update(item);

            Context.SaveChanges();
        }

        public void SaveAll<T>(IEnumerable<T> item) where T : DtoBase, new()
        {
            if (item == null)
            {
                return;
            }

            Context.UpdateRange(item);

            Context.SaveChanges();
        }

        public void RemoveAll<T>() where T : DtoBase, new()
        {
            Context.RemoveRange(
                LoadAll<T>()
                );

            Context.SaveChanges();
        }

        public IEnumerable<T> LoadMany<T, TFilter>(TFilter filter)
            where T : DtoBase, new()
            where TFilter : IDBFilter<T>
        {
            return LoadAll<T>()
                .Where(filter.EntitiesLookup);
        }

        public void RemoveMany<T, TFilter>(TFilter filter)
            where T : DtoBase, new()
            where TFilter : IDBFilter<T>
        {
            Context.RemoveRange(
                LoadMany<T, TFilter>(filter)
                );
        }
    }
}
