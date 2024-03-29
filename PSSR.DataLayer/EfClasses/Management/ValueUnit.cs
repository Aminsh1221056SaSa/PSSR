﻿using BskaGenericCoreLib;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Management
{
    public class ValueUnit
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public UnitMathType MathType { get; set; }
        public Int32 MathNum { get; private set; }

        //-----------------------------------------
        //Relationships
        public Int32? ParentId { get; private set; }
        public ValueUnit Parent { get; private set; }
        
        public ICollection<ValueUnit> Childeren { get; private set; }
       // public ICollection<ValueUnitDescipline> DesciplineLink { get; private set; }
        public ICollection<Activity> Activityes { get; private set; }

        public ValueUnit()
        {
            this.Childeren = new List<ValueUnit>();
           // this.DesciplineLink = new List<ValueUnitDescipline>();
            this.Activityes = new List<Activity>();
        }

        public ValueUnit(string name,UnitMathType type,int mathNum,int? parentId)
        {
            this.Name = name;
            this.MathType = type;
            this.MathNum = mathNum;
            this.ParentId = parentId;

            this.Childeren = new List<ValueUnit>();
            //this.DesciplineLink = new List<ValueUnitDescipline>();
            this.Activityes = new List<Activity>();
        }

        public static IStatusGeneric<ValueUnit> CreateValueUnit(string name, UnitMathType type, int mathNum, int? parentId)
        {
            var status = new StatusGenericHandler<ValueUnit>();
            var valueUnit = new ValueUnit
            {
                Name = name,
                MathType = type,
                MathNum = mathNum,
                ParentId = parentId
            };

            //valueUnit.DesciplineLink = new List<ValueUnitDescipline>();
            //foreach (var ids in desciplineId)
            //{
            //    valueUnit.DesciplineLink.Add(ValueUnitDescipline.CreateValueUnitDescipline(0,ids).Result);
            //}

            status.Result = valueUnit;
            return status;
        }

        public IStatusGeneric UpdateValueUnit(string name, UnitMathType type, int mathNum)
        {
            var status = new StatusGenericHandler();
            if (string.IsNullOrWhiteSpace(name))
            {
                status.AddError("I'm sorry, but name is empty.");
                return status;
            }

            //All Ok
            this.Name = name;
            this.MathType = MathType;
            this.MathNum = mathNum;
            return status;
        }

    }
}
