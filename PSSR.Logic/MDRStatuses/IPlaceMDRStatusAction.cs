using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.MDRS;

namespace PSSR.Logic.MDRStatuses
{
    public interface IPlaceMDRStatusAction : IGenericActionWriteDb<MDRStatusDto, MDRStatus> { }
}
