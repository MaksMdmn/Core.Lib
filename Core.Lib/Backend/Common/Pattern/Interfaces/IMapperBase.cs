using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Common.Pattern.Interfaces
{
    public interface IMapperBase<TTarget, TSource> where TTarget : class
                                                    where TSource : class
    {
        TSource From(TTarget obj);

        TTarget From(TSource obj);
    }
}
