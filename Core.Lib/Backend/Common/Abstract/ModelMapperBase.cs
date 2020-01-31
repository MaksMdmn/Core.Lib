using Core.Lib.Backend.Common.Pattern.Interfaces;
using Core.Lib.Backend.Exceptions;

namespace Core.Lib.Backend.Common.Abstract
{
    public class ModelMapperBase : IMapperBase<ModelBase, DtoBase>
    {
        public virtual DtoBase From(ModelBase obj)
        {
            throw new MappingException($"Mapper for such type is not implemented: {obj.GetType()}");
        }

        public virtual ModelBase From(DtoBase obj)
        {
            throw new MappingException($"Mapper for such type is not implemented: {obj.GetType()}");
        }
    }
}
