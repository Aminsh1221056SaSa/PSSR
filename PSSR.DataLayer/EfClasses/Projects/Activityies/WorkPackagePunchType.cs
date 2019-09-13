using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Projects
{
    public class WorkPackagePunchType
    {
        public int PunchTypeId { get; private set; }
        public int WorkPackageId { get; private set; }

        public float Precentage { get; set; }

        public PunchType PunchType { get; private set; }
        public WorkPackage WorkPackage { get; private set; }

        public WorkPackagePunchType()
        {

        }

        public WorkPackagePunchType(PunchType punchType, WorkPackage workPackage)
        {
            this.PunchType = PunchType;
            this.WorkPackage = workPackage;
        }

        public static IStatusGeneric<WorkPackagePunchType> CreateWorkPackagePunchType(float precentage, int ptypeId, int workId)
        {
            var status = new StatusGenericHandler<WorkPackagePunchType>();

            var newItem = new WorkPackagePunchType
            {
                PunchTypeId = ptypeId,
                WorkPackageId = workId,
                Precentage=precentage
            };

            status.Result = newItem;
            return status;
        }
    }
}
