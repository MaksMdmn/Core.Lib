﻿using Core.Lib.Backend.Common.Pattern.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.Backend.Storage
{
    public interface IDBConfiguration : IConfiguration
    {
        string ConnectionString { get; set; }
    }
}
