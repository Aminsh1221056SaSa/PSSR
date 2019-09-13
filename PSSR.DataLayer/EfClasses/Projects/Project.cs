using BskaGenericCoreLib;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSSR.DataLayer.EfClasses.Projects
{
    [Serializable]
    public class Project: IAuditTracker
    {
            public Guid Id { get; private set; }
            public string Description { get; private set; }
            public DateTime? StartDate { get; private set; }
            public DateTime? EndDate { get; private set; }
            public DateTime CreatedDate { get; internal set; }
            public DateTime UpdatedDate { get; internal set; }
            public ProjectType Type { get; private set; }

        //-----------------------------------------
        //Relationships
        public int ContractorId { get; private set; }
        public Contractor Contractor { get; private set; }

        public ICollection<ProjectSystem> ProjectSystems { get; private set; }
        public ICollection<MDRStatus> MDRStatus { get; private set; }
        public ICollection<PunchType> PunchTypes { get; private set; }
        public ICollection<PunchCategory> PunchCategoryes { get; private set; }
        public ICollection<ProjectWBS> ProjectWBS { get; private set; }
        public ICollection<MDRDocument> MDRDocuments { get; private set; }
        public ICollection<PersonProject> AgentsLink { get; private set; }

        private Project()
        {
            this.ProjectSystems = new List<ProjectSystem>();
            this.PunchTypes = new List<PunchType>();
            this.ProjectWBS = new List<ProjectWBS>();
            this.MDRDocuments = new List<MDRDocument>();
            this.AgentsLink = new List<PersonProject>();
            this.PunchCategoryes = new List<PunchCategory>();
        }

        public Project(string description,int contractorId,DateTime? startDate,DateTime? endDate, ProjectType type)
        {
            this.ContractorId = contractorId;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Type = type;

            this.ProjectSystems = new List<ProjectSystem>();
            this.PunchTypes = new List<PunchType>();
            this.ProjectWBS = new List<ProjectWBS>();
            this.MDRDocuments = new List<MDRDocument>();
            this.AgentsLink = new List<PersonProject>();
            this.PunchCategoryes = new List<PunchCategory>();
        }

        public static IStatusGeneric<Project> CreateProject(string description,int contractorId,
            DateTime? startDate, DateTime? endDate, ProjectType type)
        {
            var status = new StatusGenericHandler<Project>();
            var project = new Project
            {
                Description = description,
                ContractorId = contractorId,
                EndDate = endDate,
                StartDate = startDate,
                Type=type
            };

            if (!project.ProjectWBS.Any())
            {
                var pWbsItem = Projects.ProjectWBS.CreateProjectWBS(WBSType.Project, 0, 100, "1", Guid.Empty, null, "Project",WfCalculationType.Automatic).Result;
                //var mdrWbsItem = Projects.ProjectWBS.CreateProjectWBS(WBSType.MDR, 0, 10, "11", Guid.Empty, null, "MDR").Result;

                //var ifcMdr= Projects.ProjectWBS.CreateProjectWBS(WBSType.MDR, 0, 40, "111", Guid.Empty, null, "IFC MDR").Result;
                //var ifaMdr = Projects.ProjectWBS.CreateProjectWBS(WBSType.MDR, 0, 30, "112", Guid.Empty, null, "IFA MDR").Result;
                //var afcMdr = Projects.ProjectWBS.CreateProjectWBS(WBSType.MDR, 0, 30, "113", Guid.Empty, null, "AFC MDR").Result;

                //mdrWbsItem.SetParent(pWbsItem);
                //ifcMdr.SetParent(mdrWbsItem);
                //ifaMdr.SetParent(mdrWbsItem);
                //afcMdr.SetParent(mdrWbsItem);

                project.ProjectWBS.Add(pWbsItem);
                //project.ProjectWBS.Add(mdrWbsItem);

                //project.ProjectWBS.Add(ifcMdr);
                //project.ProjectWBS.Add(ifaMdr);
                //project.ProjectWBS.Add(afcMdr);
            }

            status.Result = project;
            return status;
        }

        public IStatusGeneric UpdateProject(string description,
            DateTime? startDate, DateTime? endDate)
        {
            var status = new StatusGenericHandler();

            //All Ok
            this.EndDate = endDate;
            this.Description = description;
            this.StartDate = startDate;

            return status;
        }
        
    }
}
