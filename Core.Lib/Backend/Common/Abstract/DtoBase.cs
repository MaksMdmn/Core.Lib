using System;
using System.Runtime.Serialization;
using Core.Lib.Backend.Common.Abstract.Interfaces;

namespace Core.Lib.Backend.Common.Abstract
{
    [DataContract]
    [Serializable]
    public abstract class DtoBase : ICloneable, IUnique<string>
    {
        [DataMember]
        public virtual string Uid { get; set; }

        public DtoBase()
        {

        }

        public DtoBase(string uid)
        {
            Uid = uid;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
