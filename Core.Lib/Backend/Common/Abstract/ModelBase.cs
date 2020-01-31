using System;
using System.Runtime.Serialization;
using Core.Lib.Backend.Common.Abstract.Interfaces;

namespace Core.Lib.Backend.Common.Abstract
{
    [DataContract]
    [Serializable]
    public abstract class ModelBase : IUnique<string>
    {
        [DataMember]
        public virtual string Uid { get; set; }

        protected ModelBase()
        {
            Uid = Guid.NewGuid().ToString();
        }

        protected ModelBase(string uid)
        {
            Uid = uid;
        }

    }
}
