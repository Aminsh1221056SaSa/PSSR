using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers
{
    public interface ImappedFrom<T>
    {
    }

    public class AppMapping: Profile
    {
        public AppMapping()
        {
            var types = CustomExtension.GetApplicationTypes();

            var maps = (from type in types
                        from i in type.GetInterfaces()
                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ImappedFrom<>)
                        && !type.IsAbstract && !type.IsInterface
                        select new
                        {
                            Source = i.GenericTypeArguments[0],
                            Dest = type
                        }).ToArray();

            foreach (var map in maps)
            {
                CreateMap(map.Source, map.Dest);//.ForAllMembers(opt => opt.NullSubstitute("Other Value"));
                CreateMap(map.Dest, map.Source);
            }
        }
    }
}
