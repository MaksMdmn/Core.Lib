using Core.Lib.Backend.Common.Pattern.Interfaces;
using Core.Lib.Backend.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Storage.EF
{
    public struct EFConfig : IDBConfiguration
    {
        public string ConnectionString { get; set; }

        public string Name { get; set; }
    }
}
