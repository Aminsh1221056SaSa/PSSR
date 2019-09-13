using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DbAccess.MDRDocuments;

namespace PSSR.Logic.MDRDocuments.Concrete
{
    public class PlaceMDRCommentAction : BskaActionStatus, IPlaceMDRCommentAction
    {
        private readonly IPlaceMDRDocumentCommentDbAccess _dbAccess;
        private readonly IUpdateMDRDocumentDbAccess _mdrDbAccess;

        public PlaceMDRCommentAction(IPlaceMDRDocumentCommentDbAccess dbAccess, IUpdateMDRDocumentDbAccess mdrDbAccess)
        {
            _dbAccess = dbAccess;
            _mdrDbAccess = mdrDbAccess;
        }

        public MDRDocumentComment BizAction(MDRDocumentCommentDto inputData)
        {
            var mdr = _mdrDbAccess.GetMDRDocument(inputData.MDRDocumentId);
            if (mdr == null)
            {
                AddError("MDR Document Not valid.");
            }

            if(_mdrDbAccess.HasDefaultStatus(inputData.MDRDocumentId))
            {
                AddError("Add Comment on default status not allowed.Please create next issuance status for mdrDocument and then try to add Comment.");
            }

            if (string.IsNullOrWhiteSpace(inputData.Title))
            {
                AddError("title is Required.");
            }
          
            var desStatus = MDRDocumentComment.CreateMDRDocument(inputData.Title,inputData.Description,inputData.MDRDocumentId,inputData.FilePath);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
