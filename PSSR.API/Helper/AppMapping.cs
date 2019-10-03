using AutoMapper;
using PSSR.Common.CommonModels;
using PSSR.DataLayer.EfClasses.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.API.Helper
{
    public static class CustomExtension
    {
        public static IEnumerable<Type> GetApplicationTypes()
        {
            return typeof(CustomExtension).Assembly.GetTypes().Where(t => !t.IsAbstract).ToList();
        }
    }
    public interface ImappedFrom<T>
    {
    }

    public class AppMapping : Profile
    {
        public AppMapping()
        {
            CreateMap<ValueUnit, ValueUnitModel>();
        }
    }
}
