using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.ProjectSystmes
{
    public interface IPlaceSystemAction : IGenericActionWriteDb<ProjectSystemDto, ProjectSystem> { }
}
