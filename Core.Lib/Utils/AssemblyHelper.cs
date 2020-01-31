using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Core.Lib.Utils
{
    public static class AssemblyHelper
    {
        private const string _supportedExtension = ".dll";

        public static Assembly LoadAssemblySafe(string assemblyFullPath)
        {

            //TODO: do this method more flexible, as it could be different format of paths (localization, OS etc...)

            string directoryPath = assemblyFullPath.Substring(0, assemblyFullPath.LastIndexOf(@"\"));

            if (!assemblyFullPath.Contains(_supportedExtension))
                return null;

            return Directory.Exists(directoryPath)
                ? Assembly.LoadFile(assemblyFullPath)
                : null;
        }

        public static IEnumerable<Type> SelectTypesByBase<TBase>(Assembly assembly) where TBase : class
        {
            return assembly.DefinedTypes
                 .Where(type => type.IsClass && type.IsPublic
                             && !type.IsInterface && !type.IsGenericType && !type.IsImport)
                 .Where(selectedType => typeof(TBase).IsAssignableFrom(selectedType));
        }

    }
}