using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DbAccess.MDRDocuments;
using PSSR.DbAccess.MDRStatuses;

namespace PSSR.Logic.MDRDocuments.Concrete
{
    public class PlaceMDRDocumentAction : BskaActionStatus, IPlaceMDRDocumentAction
    {
        private readonly IPlaceMDRDocumentDbAccess _dbAccess;
        private readonly IUpdateMDRStatusDbAccess _dbStatusAccess;

        public PlaceMDRDocumentAction(IPlaceMDRDocumentDbAccess dbAccess
            , IUpdateMDRStatusDbAccess dbStatusAccess)
        {
            _dbAccess = dbAccess;
            _dbStatusAccess = dbStatusAccess;
        }

        public MDRDocument BizAction(MDRDocumentDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Title))
            {
                AddError("title is Required.");
            }
            if (string.IsNullOrWhiteSpace(inputData.FolderName))
            {
                AddError("Folder Name is invalied.");
            }

            var defaultStatus = _dbStatusAccess.GetDefaultStatus(inputData.ProjectId);
            if(defaultStatus==null)
            {
                AddError("Default MDR Status Not Exit.");
            }

            IStatusGeneric<MDRDocument> desStatus = null;

            if (!HasErrors)
            {
                desStatus = MDRDocument.CreateMDRDocument(inputData.Title, inputData.Description,
                  inputData.WorkPackageId, inputData.Code, defaultStatus.Id, inputData.ProjectId, inputData.Type);

                var mdr = desStatus.Result;
                mdr.CreateMDRStatus("CREATE MDR", defaultStatus.Id, inputData.FolderName);

                _dbAccess.Add(desStatus.Result);
                CombineErrors(desStatus);
            }

            return HasErrors ? null : desStatus.Result;
        }
    }
}
