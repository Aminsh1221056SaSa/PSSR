using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DbAccess.MDRDocuments;
using PSSR.DbAccess.MDRStatuses;
using System;
using System.Linq;

namespace PSSR.Logic.MDRDocuments.Concrete
{
    public class UpdateDocumentIssuance : BskaActionStatus, IUpdateDocumentIssuance
    {
        private readonly IUpdateMDRDocumentDbAccess _dbAccess;
        private readonly IUpdateMDRStatusDbAccess _mdrStatusDbAccess;

        public UpdateDocumentIssuance(IUpdateMDRDocumentDbAccess dbAccess, IUpdateMDRStatusDbAccess mdrstatusdbaccess)
        {
            _dbAccess = dbAccess;
            _mdrStatusDbAccess = mdrstatusdbaccess;
        }

        public void BizAction(IssuanceDto inputData)
        {
            var mdr = _dbAccess.GetMDRDocumentWithStatusAndComment(inputData.MdrId);
            if(mdr==null)
            {
                AddError("Could not find the MDR. Someone entering illegal ids?");
            }

            if (inputData.StatusId <= 0)
            {
                AddError("Could not go to next status...!!");
                return;
            }

            var status = _mdrStatusDbAccess.GetMdrStatus(inputData.StatusId);
            if(status==null)
            {
                AddError("Could not find the MDR Status. Someone entering illegal ids?");
            }

            var laststatus = mdr.MDRStatusHistoryies.OrderByDescending(s => s.CreatedDate).First();
            var lastComment = mdr.MDRDocumentComments.OrderByDescending(s => s.CreatedDate).FirstOrDefault();

            string description = inputData.Description;
            int unclearCommentCount = 0;
            if (lastComment != null)
            {
                unclearCommentCount = mdr.MDRDocumentComments.Where(s=>!s.IsClear).Count();
            }

            if (laststatus.MdrStatusId==status.Id)
            {
                var nextStatus = _mdrStatusDbAccess.GetNextStatus(inputData.ProjectId, status);
                if(nextStatus!=null)
                {
                    description = $"Issuance IFR for {status.Name}.{Environment.NewLine}{inputData.Description}";

                    foreach (var co in mdr.MDRDocumentComments.Where(s => !s.IsClear))
                    {
                        co.ClearComment();
                    }

                    var result = MDRStatusHistory.CreateMDRStatus(description, status.Id, true, false,inputData.FolderName);
                    mdr.MDRStatusHistoryies.Add(result.Result);
                }
                else
                {
                    if(unclearCommentCount>0)
                    {
                        description = $"Issuance IFR for {status.Name}.{Environment.NewLine}{inputData.Description}";
                        foreach (var co in mdr.MDRDocumentComments.Where(s => !s.IsClear))
                        {
                            co.ClearComment();
                        }
                    }
                    else
                    {
                        description = $"Confirm {status.Name}.{Environment.NewLine}{inputData.Description}";
                        mdr.UpdateIsCompleted();
                        laststatus.ConfirmContractor();
                    }
                }
            }
            else
            {
                if (mdr.Type == Common.MDRDocumentType.A)
                {
                    if (!inputData.IsConfirmContractor)
                    {
                        AddError("Please Check MDRDocument Confirm By Contractor for go to next status.");
                    }
                    else
                    {
                       var result = MDRStatusHistory.CreateMDRStatus(description, status.Id, false, false,inputData.FolderName);
                        mdr.MDRStatusHistoryies.Add(result.Result);
                        laststatus.ConfirmContractor();
                    }
                }
                else
                {
                    if(mdr.MDRStatusHistoryies.Count<=1)
                    {
                       var result= MDRStatusHistory.CreateMDRStatus(description, status.Id,false,false, inputData.FolderName);
                        mdr.MDRStatusHistoryies.Add(result.Result);
                    }
                    else
                    {
                        var seperateDays = (DateTime.Now - laststatus.CreatedDate).Days;
                        if (seperateDays > 14)
                        {
                            if (unclearCommentCount > 0)
                            {
                                AddError("for go to next status,All comments muste be clear.");
                            }
                            else
                            {
                                var result = MDRStatusHistory.CreateMDRStatus(description, status.Id,false,false, inputData.FolderName);
                                mdr.MDRStatusHistoryies.Add(result.Result);
                                laststatus.ConfirmContractor();
                            }
                        }
                        else
                        {
                            AddError("for go to next status,Minimum seperated days must by 14.");
                        }
                    }
                }
            }
        }
    }
}
