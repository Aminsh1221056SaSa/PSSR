using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Management
{
    public class FormDictionaryDescipline
    {
        public long FormDictionaryId { get; private set; }
        public int DesciplineId { get; private set; }

        public Descipline Descipline { get; private set; }
        public FormDictionary FormDictionary { get; private set; }

        public FormDictionaryDescipline()
        {

        }

        public FormDictionaryDescipline(Descipline descipline, FormDictionary formDic)
        {
            this.Descipline = descipline;
            this.FormDictionary = formDic;
        }

        public static IStatusGeneric<FormDictionaryDescipline> CreateFormDicDescipline(long formDicId, int desciplineId)
        {
            var status = new StatusGenericHandler<FormDictionaryDescipline>();

            var newItem = new FormDictionaryDescipline
            {
                DesciplineId = desciplineId,
                FormDictionaryId = formDicId
            };

            status.Result = newItem;
            return status;
        }
    }
}
