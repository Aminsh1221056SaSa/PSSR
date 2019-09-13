
using PSSR.Common;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ActivityServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSSR.ServiceLayer.ActivityServices.Concrete
{
    public class ActivityFilterDropdownService
    {

        private readonly EfCoreContext _db;

        public ActivityFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(ActivityFilterBy filterBy,Guid projectId)
        {
            switch (filterBy)
            {
                case ActivityFilterBy.NoFilter:
                    return new List<DropdownTuple>();

                //case ActivityFilterBy.ByWorkPackage:

                //    return _db.ProjectRoadMaps
                //         .Select(s => new DropdownTuple
                //         {
                //             Value = s.Id.ToString(),
                //             Text = s.Name
                //         }).ToList();

                case ActivityFilterBy.ByWorkPackageStep:

                    return _db.WorkPackageStep
                         .Select(s => new DropdownTuple
                         {
                             Value = s.Id.ToString(),
                             Text = s.Title
                         }).ToList();

                //case ActivityFilterBy.ByLocation:

                //    return _db.LocationTypes
                //         .Select(s => new DropdownTuple
                //         {
                //             Value = s.Id.ToString(),
                //             Text = s.Title
                //         }).ToList();


                case ActivityFilterBy.ByDescipline:

                   return _db.Desciplines
                        .Select(s => new DropdownTuple
                    {
                       Value=s.Id.ToString(),
                       Text=s.Name
                    }).ToList();

                //case ActivityFilterBy.BySystem:

                //    return _db.ProjectSystems
                //         .Select(s => new DropdownTuple
                //         {
                //             Value = s.Id.ToString(),
                //             Text = $"{s.Code}({s.Description})"
                //         }).ToList();

                //case ActivityFilterBy.BySubSystem:

                //    return _db.ProjectSubSystems
                //         .Select(s => new DropdownTuple
                //         {
                //             Value = s.Id.ToString(),
                //             Text = $"{s.Code}({s.Description})"
                //         }).ToList();

                case ActivityFilterBy.ByFormDictionary:

                    return _db.FormDictionaries
                         .Select(s => new DropdownTuple
                         {
                             Value = s.Id.ToString(),
                             Text = $"{s.Code}({s.Description})"
                         }).ToList();

                case ActivityFilterBy.ByFormType:
                    return Enum.GetValues(typeof(FormDictionaryType))
                        .Cast<FormDictionaryType>().Select(v => new DropdownTuple
                        {
                            Value = v.ToString(),
                            Text = v.ToString()
                        });

                case ActivityFilterBy.ByStatus:
                    return Enum.GetValues(typeof(ActivityStatus))
                        .Cast<ActivityStatus>().Select(v => new DropdownTuple
                        {
                            Value = v.ToString(),
                            Text = v.ToString()
                        });

                case ActivityFilterBy.ByCondition:
                    return Enum.GetValues(typeof(ActivityCondition))
                        .Cast<ActivityCondition>().Select(v => new DropdownTuple
                        {
                            Value = v.ToString(),
                            Text = v.ToString()
                        });

                case ActivityFilterBy.ByProgress:
                    return FormProgressDropDown();
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }

        private static IEnumerable<DropdownTuple> FormProgressDropDown()
        {
            return new[]
            {
                 new DropdownTuple {Value = "90", Text = "90 % and up"},
                  new DropdownTuple {Value = "80", Text = "80 % and up"},
                   new DropdownTuple {Value = "70", Text = "70 % and up"},
                    new DropdownTuple {Value = "60", Text = "60 % and up"},
                     new DropdownTuple {Value = "50", Text = "50 % and up"},
                new DropdownTuple {Value = "40", Text = "40 % and up"},
                new DropdownTuple {Value = "30", Text = "30 % and up"},
                new DropdownTuple {Value = "20", Text = "15 % and up"},
                new DropdownTuple {Value = "5", Text = "5 % and up"},
            };
        }

        //private string getLastChildIds(ProjectWBS parent)
        //{
        //    string lastChildIds=null;

        //    if (parent.Childeren.Any())
        //    {
        //        foreach(var p in parent.Childeren)
        //        {
        //            string s= getLastChildIds(p);
        //            if (!string.IsNullOrWhiteSpace(s))
        //            {
        //                lastChildIds += (s + ",");
        //            }
        //        }
        //        return lastChildIds;
        //    }
        //    else
        //    {
        //        return parent.Id.ToString();
        //    }
        //}
    }
}
