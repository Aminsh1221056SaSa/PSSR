using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PSSR.Common
{
    public static class GlobalClass
    {
    }

    public enum ProjectType
    {
        [Display(Name = "Oil Platform")]
         OilPlatform = 1001,
        [Display(Name = " Gas Platform")]
         GasPlatform =1002,
        [Display(Name = "Refinery")]
         Refinery=1003,
        [Display(Name = "Others")]
        Others = 1004
    }

    public enum SystemType
    {
        Process=1001,
        Utility
    }
    
    public enum UnitMathType
    {
        None = 0,
        Multiple,
        Divide,
    }

    public enum FormDictionaryType
    {
        Test=1,
        Check
    }

    public enum ActivityStatusColor
    {
        FF530D=1,
        E8EC26,
        A3db08,
        DE1515,
    }

    public enum ActivityStatus
    {
        NotStarted=1,
        Ongoing=2,
        Done=3,
        Reject=4,
        Delete=5
    }

    public enum ActivityCondition
    {
        Normal=3000,
        Hold=3002,
        Front=3003
    }

    public enum ActivityHolBy
    {
        NoHold=0,
        HoldMaterial=1,
        HoldSequence=2,
        HoldOther=3,
    }

    public enum ManHouresType
    {
        ByForm=1001,
        ByActivity
    }

    public enum WBSType
    {
        Project=1001,
        WorkPackage=1002,
        Location=1003,
        Descipline = 1004,
        System =1005,
        SubSystem=1006,
        Activity=1007
    }

    public enum WfCalculationType
    {
        Automatic=1,
        Manual
    }
    public enum MDRDocumentType
    {
        A=1,
        B=2
    }
}
