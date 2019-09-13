using BskaGenericCoreLib;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Management
{
    public class FormDictionary : IAuditTracker
    {
        public long Id { get; private set; }
        public string Description { get; private set; }
        public string Code { get; private set; }
        public string ActivityName { get; private set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }
        public FormDictionaryType Type { get; private set; }
        public string FileName { get; private set; }
        public int Priority { get; private set; }
        public float ManHours { get; private set; }
        //-----------------------------------------
        //Relationships
        public int WorkPackageId { get; private set; }
        public ICollection<FormDictionaryDescipline> DesciplineLink { get; private set; }
        public WorkPackage WorkPackage { get; private set; }
        
        public ICollection<Activity> Activityes { get; private set; }

        private FormDictionary()
        {
            this.Activityes = new List<Activity>();
            this.DesciplineLink = new List<FormDictionaryDescipline>();
        }

        public FormDictionary(FormDictionaryType type,
            string description,string code,string activityName,string fileName
            ,int workPackageId,int priority,float mh)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code));
            
            this.Description = description;
            this.Code = code;
            this.ActivityName = activityName;
            this.Type = type;
            this.FileName = fileName;
            this.WorkPackageId = workPackageId;
            this.Priority = priority;
            this.ManHours = mh;

            this.DesciplineLink = new List<FormDictionaryDescipline>();
            this.Activityes = new List<Activity>();
        }

        public static IStatusGeneric<FormDictionary> CreateFormDicFactory(FormDictionaryType type,
            string description, string code, string activityName, string fileName
            , int workPackageId, int[] desciplineId,int priority,float mh)
        {
            var status = new StatusGenericHandler<FormDictionary>();
            var formDic = new FormDictionary
            {
                Description = description,
                Code = code,
                ActivityName = activityName,
                Type = type,
                FileName = fileName,
                WorkPackageId = workPackageId,
                Priority = priority,
                ManHours=mh
            };

            formDic.DesciplineLink = new List<FormDictionaryDescipline>();
            foreach (var ids in desciplineId)
            {
                formDic.DesciplineLink.Add(FormDictionaryDescipline.CreateFormDicDescipline(0, ids).Result);
            }

            status.Result = formDic;
            return status;
        }

        public IStatusGeneric UpdateFormDictioanry(FormDictionaryType type,
            string description, string code, string activityName, int workPackageId,int priority, string fileName
            ,float mh)
        {
            var status = new StatusGenericHandler();
            //All Ok
            //this.Code = code;
            this.Description = description;
            this.ActivityName = activityName;
            this.Priority = priority;
            this.FileName = fileName;
            this.ManHours = mh;

            return status;
        }

        public void AddDescipline(int desciplineId)
        {
            if(this.DesciplineLink!=null)
            this.DesciplineLink.Add(FormDictionaryDescipline.CreateFormDicDescipline(0, desciplineId).Result);
        }
    }
}
