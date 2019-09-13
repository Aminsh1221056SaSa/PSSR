using BskaGenericCoreLib;
using PSSR.DbAccess.MDRDocuments;

namespace PSSR.Logic.MDRDocuments.Concrete
{
    public class UpdateMDRDocumentAction : BskaActionStatus, IUpdateMDRDocumentAction
    {
        private readonly IUpdateMDRDocumentDbAccess _dbAccess;

        public UpdateMDRDocumentAction(IUpdateMDRDocumentDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(MDRDocumentDto inputData)
        {
            var MDRDOC = _dbAccess.GetMDRDocument(inputData.Id);
            if (MDRDOC == null)
            {
                AddError("Could not find the MDR. Someone entering illegal ids?");
                return;
            }

            var status = MDRDOC.UpdateMDRDocument(inputData.Title,
                inputData.Description,inputData.WorkPackageId,inputData.Code,inputData.Type);

            CombineErrors(status);

            Message = $"MDR is update: {MDRDOC.ToString()}.";
        }
    }
}
