using BskaGenericCoreLib;
using PSSR.DbAccess.MDRDocuments;

namespace PSSR.Logic.MDRDocuments.Concrete
{
    public class UPdateMDRCommentAction : BskaActionStatus, IUpdateMDRCommentAction
    {
        private readonly IUpdateMDRDocumentCommentDbAccess _dbAccess;

        public UPdateMDRCommentAction(IUpdateMDRDocumentCommentDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }
        public void BizAction(MDRDocumentCommentDto inputData)
        {
            var MDRDOC = _dbAccess.GetMDRDocumentCommdent(inputData.Id);
            if (MDRDOC == null)
            {
                AddError("Could not find the MDR Comment. Someone entering illegal ids?");
            }
             
            if(MDRDOC.IsClear)
            {
                AddError("Comment is clear and does not allow for edit!!");
            }

            if(!this.HasErrors)
            {
                var status = MDRDOC.UpdateMDRDocument(inputData.Title, inputData.Description, inputData.MDRDocumentId);
                CombineErrors(status);
                Message = $"MDR Comment is update: {MDRDOC.ToString()}.";
            }
        }
    }
}
