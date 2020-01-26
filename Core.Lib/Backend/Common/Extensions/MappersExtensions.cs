using System;
using AutoMapper;
using Core.Lib.Backend.Common.Abstract;
using NLog;

namespace Core.Lib.Backend.Common.Extensions
{
    public static class MappersExtensions
    {
        public static void CreateTwoWaysMap<TModel, TDto>(this IProfileExpression config)
            where TModel : ModelBase
            where TDto : DtoBase
        {
            config.CreateMap<TModel, TDto>();
            config.CreateMap<TDto, TModel>();
        }

        internal static void LogMapperAttempt(this ILogger logger, Type from, Type to)
        {
            logger.LogMappingAttempt(from, to, nameof(ProjectMapper));
        }

        internal static void LogAutoMapperAttempt(this ILogger logger, Type from, Type to)
        {
            logger.LogMappingAttempt(from, to, nameof(Mapper));
        }

        private static void LogMappingAttempt(this ILogger logger, Type from, Type to, string mapperName)
        {
            logger.Trace($"{mapperName} did map {nameof(from)} to {nameof(to)}. Mixed-mapping usage: {ProjectMapper.Mode}");
        }
    }
}
