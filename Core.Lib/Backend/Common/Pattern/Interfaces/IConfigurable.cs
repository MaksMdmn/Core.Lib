using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Common.Pattern.Interfaces
{
    public interface IConfigurable<TConfig> where TConfig : struct, IConfiguration
    {
        TConfig Config { get; }
    }
}
