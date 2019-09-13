using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Management
{
    public class LocationType
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public ICollection<Activity> Activityes { get; private set; }

        private LocationType()
        {
            Activityes = new List<Activity>();
        }

        public LocationType(string title)
        {
            this.Title = title;
            Activityes = new List<Activity>();
        }

        public static IStatusGeneric<LocationType> CreateLocationType(string title)
        {
            var status = new StatusGenericHandler<LocationType>();
            var location = new LocationType
            {
                Title=title
            };
            
            status.Result = location;
            return status;
        }

        public IStatusGeneric UpdateLocationType(string title)
        {
            var status = new StatusGenericHandler();
            if (string.IsNullOrWhiteSpace(title))
            {
                status.AddError("I'm sorry, but title is empty.");
                return status;
            }
            
            //All Ok
            this.Title = title;
            return status;
        }
    }
}
