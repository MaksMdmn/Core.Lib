using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Core.Lib.Backend.Common.Abstract;
using Core.Lib.Backend.Common.Extensions;
using NLog;
using Core.Lib.Backend.Exceptions;
using Utils;

namespace Core.Lib.Backend.Common
{
    public enum MapperModes
    {
        OnlyMapperDefault,          // only mapper will be used
        OnlyMapperOrAutoMapper,     // try mapper, if fails - AutoMapper
        MixAutoMapperThenMapper     // do automapper, then mapper - always!
    }

    public static class ProjectMapper
    {
        private static readonly Dictionary<Type, List<ModelMapperBase>> projectMappers;
        private static Mapper _autoMapper;
        private static ILogger _logger;

        public static MapperModes Mode { get; set; }

        static ProjectMapper()
        {
            Mode = MapperModes.OnlyMapperDefault;

            _logger = LogManager.GetCurrentClassLogger();

            projectMappers = new Dictionary<Type, List<ModelMapperBase>>();

            List<Type> selectedMappers = AssemblyHelper.SelectTypesByBase<ModelMapperBase>(Assembly.GetEntryAssembly()).ToList();

            foreach (var mapperType in selectedMappers)
            {
                List<Type> modelConvertableTypes = mapperType
                    .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                    .Where(method => method.Name.Equals(nameof(ModelMapperBase.From)))
                    .Select(suitableMethod => suitableMethod.ReturnType)
                    .Distinct()
                    .ToList();

                foreach (var modelType in modelConvertableTypes)
                {
                    if (!projectMappers.ContainsKey(modelType))
                    {
                        projectMappers[modelType] = new List<ModelMapperBase>();
                    }

                    ModelMapperBase mapperInstance = Activator.CreateInstance(mapperType) as ModelMapperBase;

                    if (mapperInstance == null)
                    {
                        throw new MappingException($"Unable to create mapper: {mapperType}, for instance type: {modelType}.");
                    }

                    projectMappers[modelType].Add(mapperInstance);
                }
            }
        }

        public static IEnumerable<TDestination> MapAll<TDestination>(IEnumerable<dynamic> fromObjects) where TDestination : class
            => fromObjects.Select(obj => Map<TDestination>(obj) as TDestination);


        public static TDestination Map<TDestination>(dynamic fromObject) where TDestination : class
        {
            //TODO: check this approach to return null in each case below
            Type sourceType = typeof(TDestination);

            if (fromObject == null)
            {
                return null;
            }

            return Mode == MapperModes.MixAutoMapperThenMapper 
                ? MixedMap<TDestination>(fromObject, sourceType) 
                : StraightMap<TDestination>(fromObject, sourceType);
        }

        public static void ConfigureAutoMapping(params Profile[] profiles)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));

            _autoMapper = new Mapper(config);
        }

        private static TDestination StraightMap<TDestination>(dynamic fromObject, Type sourceType) where TDestination : class
        {
            TDestination result = null;

            foreach (var mapper in projectMappers[sourceType])
            {
                try
                {
                    result = (mapper as dynamic).From(fromObject) as TDestination;

                    _logger.LogMapperAttempt(sourceType, typeof(TDestination));

                    return result;
                }
                catch (MappingException)
                {
                    //NOP
                }
            }

            try
            {
                if (Mode == MapperModes.OnlyMapperOrAutoMapper
                    && _autoMapper != null)
                {
                    result = _autoMapper.Map<TDestination>(fromObject);

                    _logger.LogAutoMapperAttempt(sourceType, typeof(TDestination));

                    return result;
                }
            }
            catch (Exception)
            {
                //NOP
            }

            throw new MappingException($"Mapper for such instance type: {typeof(TDestination)} not found.");
        }

        private static TDestination MixedMap<TDestination>(dynamic fromObject, Type sourceType) where TDestination : class
        {
            TDestination autoMapperResult = null;
            TDestination mapperChiefResult = null;

            try
            {
                if (_autoMapper != null)
                {
                    autoMapperResult = _autoMapper.Map<TDestination>(fromObject);

                    _logger.LogAutoMapperAttempt(sourceType, typeof(TDestination));
                }
            }
            catch (Exception)
            {
                //NOP
            }

            if (projectMappers.TryGetValue(sourceType, out List<ModelMapperBase> mappers))
            {
                foreach (var mapper in mappers)
                {
                    try
                    {
                        mapperChiefResult = (mapper as dynamic).From(fromObject) as TDestination;

                        _logger.LogMapperAttempt(sourceType, typeof(TDestination));

                        break;
                    }
                    catch (MappingException)
                    {
                        //NOP
                    }
                }
            }

            if (autoMapperResult != null
                && mapperChiefResult != null)
            { 
                autoMapperResult?
                    .GetType()
                    .GetProperties()
                    .ToList()
                    .ForEach(prop =>
                    {
                        var defaultValue = prop.PropertyType.IsValueType
                            ? Activator.CreateInstance(prop.PropertyType)
                            : null;

                        if (prop.GetValue(autoMapperResult) == defaultValue)
                        {
                            prop.SetValue(autoMapperResult, prop.GetValue(mapperChiefResult));
                        }
                    });

                return mapperChiefResult;
            }

            if (autoMapperResult != null)
            {
                return autoMapperResult;
            }

            if (mapperChiefResult != null)
            {
                return mapperChiefResult;
            }

            throw new MappingException($"Mapper for such instance type: {typeof(TDestination)} not found.");
        }

    }
}
