using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.MDRDocumentServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.MDRDocumentServices.Concrete
{
    public class ListMDRDocumentService
    {
        private readonly EfCoreContext _context;
        private readonly IMapper _mapper;

        public ListMDRDocumentService(EfCoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ListMDRDocumentService(EfCoreContext context) : this(context, null) { }

        public Task<IQueryable<MDRDocumentListDto>> SortFilterPage
           (MDRDocumentSortFilterPageOptions options, Guid projectId)
        {
            return Task.Run(() =>
            {
                var mdrDocsQuery = _context.MDRDocuments.Where(s => s.ProjectId == projectId)
                .AsNoTracking()
                .MapMDRDicToDto()
                .OrderMDRDocumentBy(options.OrderByOptions)
                .FilterMDRDocumentBy(options.FilterBy, options.FilterValue);

                options.SetupRestOfDto(mdrDocsQuery);

                return mdrDocsQuery.Page(options.PageNum - 1,
                                       options.PageSize);
            });
        }

        public async Task<List<MDRSummaryDto>> GetMDRDashbordSummary(Guid projectid)
        {
            var mdrs = await _context.MDRDocuments.Where(s => s.ProjectId == projectid).GroupBy(s=>s.WorkPackageId).ToListAsync();
            var workPackages = await _context.ProjectRoadMaps.ToListAsync();

            var lstItems = new List<MDRSummaryDto>();
            foreach(var md in mdrs)
            {
                var work = workPackages.First(s => s.Id == md.Key);

                lstItems.Add(new MDRSummaryDto
                {
                    WorkPackageId=md.Key,
                    Done=md.Count(s=>s.IsCompleted),
                    Total=md.Count(),
                    Name=work.Name,
                    Link= $"/ProjectManagment/MDRDocument/MDRDocument?OrderByOptions=0&FilterBy=2&FilterValue={md.Key}"
                });
            }
            return lstItems;
        }

        public async Task<MDRDocumentListDto> GetMdrDocument(long id)
        {
            var mdr = await _context.MDRDocuments.Where(s => s.Id == id)
                .Include(s=>s.WorkPackage).Select(p=>new MDRDocumentListDto
            {
                Title = p.Title,
                CreatedDate = p.CreatedDate.ToString("dddd, dd MMMM yyyy"),
                UpdatedDate = p.UpdatedDate.ToString("dddd, dd MMMM yyyy"),
                WorkPackageId = p.WorkPackageId,
                WBSName = p.WorkPackage.Name,
                Description = p.Description,
                Id = p.Id,
                Code=p.Code,
                Type=p.Type,
                IsCompleted=p.IsCompleted
            }).FirstOrDefaultAsync();
            return mdr;
        }

        public bool HasDublicatedCode(Guid  projectid,string codel)
        {
            return _context.MDRDocuments.Any(s => s.ProjectId == projectid && s.Code==codel);
        }

        public async Task<int> GetCommentsCount(long id)
        {
            return await _context.MDRDocuments.Where(s => s.Id == id).CountAsync();
        }

        public async Task<MDRDocumentListDto> GetMDRDocumentDetails(long id)
        {
            return await _context.MDRDocuments.Where(s => s.Id == id)
                .MapMDRDicToDto().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MDRDocumentStatusListModel>> GetMDRDocumentStatusHistory(long id)
        {
            return await _context.MdrStatusHistories.Where(s => s.MDRDocumentId == id)
                .OrderByDescending(s => s.CreatedDate)
               .Select(s => new MDRDocumentStatusListModel
               {
                   CreatedDate = s.CreatedDate.ToString("dddd, dd MMMM yyyy"),
                   Description = s.Description,
                   MdrDocumentId = s.MDRDocumentId,
                   Id = s.Id,
                   UpdatedDate= s.UpdatedDate.ToString("dddd, dd MMMM yyyy"),
                   StatusId = s.MdrStatusId,
                   StatusName=s.HStatus.Name,
                   WF=s.HStatus.Wf,
                   IsContractorConfirm=s.IsContractorConfirm,
                   IsIFR=s.IsIFR,
                   FilePath=s.FolderName
               }).ToListAsync();
        }

        public async Task<IEnumerable<MDRDocumentCommentListDto>> GetMDRDocumentComments(long id)
        {
            return await _context.MDRDocumentComments.Where(s => s.MDRDocumentId == id).OrderByDescending(s => s.CreatedDate)
               .Select(s => new MDRDocumentCommentListDto
               {
                   CreateDate = s.CreatedDate.ToString("dddd, dd MMMM yyyy"),
                   Description = s.Description,
                   MDRDocumentId = s.MDRDocumentId,
                   Id = s.Id,
                   Title = s.Title,
                   IsClear = s.IsClear,
                   HasFilePath=s.FilePath!=null?true:false
               }).ToListAsync();
        }

        public async Task<MDRDocumentCommentListDto> GetCommentDetails(long commentId)
        {
            return await _context.MDRDocumentComments.Where(s => s.Id == commentId)
              .Select(s => new MDRDocumentCommentListDto
              {
                  CreateDate = s.CreatedDate.ToString("dddd, dd MMMM yyyy"),
                  Description = s.Description,
                  MDRDocumentId = s.MDRDocumentId,
                  Id = s.Id,
                  Title = s.Title,
                  IsClear = s.IsClear
              }).FirstOrDefaultAsync();
        }

        public async Task<string> GetCommentFilePath(long commentId)
        {
            return await _context.MDRDocumentComments.Where(s => s.Id == commentId)
              .Select(s =>s.FilePath).SingleOrDefaultAsync();
        }

        public Task<ILookup<string,MDRDocumentCommentListDto>> GetMDRDocumentCommentsTimeLine(long id)
        {
           return Task.Run(() =>
            {
               return _context.MDRDocumentComments.Where(s => s.MDRDocumentId == id).OrderByDescending(s=>s.CreatedDate)
                .Select(s => new MDRDocumentCommentListDto
                {
                    CreateDate = s.CreatedDate.ToString("dddd, dd MMMM yyyy"),
                    Description = s.Description,
                    MDRDocumentId = s.MDRDocumentId,
                    Id = s.Id,
                    Title = s.Title,
                    IsClear=s.IsClear
                }).ToLookup(s => s.CreateDate);
            });
        }

        public async Task<MDRIssuanceDescription> GetIssuanceDescription(long mdrId,Guid projectId)
        {
            var mdr = await _context.MDRDocuments.Where(s => s.Id == mdrId).Include(s => s.MDRStatusHistoryies)
                .Include(s => s.MDRDocumentComments).SingleOrDefaultAsync();
            if (mdr == null) return null;
            var laststatus = mdr.MDRStatusHistoryies.OrderByDescending(s => s.CreatedDate).First();
            var lastComment = mdr.MDRDocumentComments.OrderByDescending(s => s.CreatedDate).FirstOrDefault();

            var allStatus = await _context.MDRStatus.Where(s => s.ProjectId == projectId)
                .OrderBy(s => s.Id).ToListAsync();

            var hStatus = allStatus.Where(s => s.Id == laststatus.MdrStatusId).Single();
            var nextStatus = allStatus.GetNext(hStatus);

            string nextstatusDescription = "";
            int nextStatusId = 0;
            int unclearCommentCount = 0;
            string lastCommentDate = "";


            if (lastComment != null)
            {
                lastCommentDate = lastComment.CreatedDate.ToString("dddd, dd MMMM yyyy");
                unclearCommentCount = mdr.MDRDocumentComments.Where(s => !s.IsClear).Count();
            }


            if (mdr.MDRStatusHistoryies.Count>1)
            {
                if(nextStatus==null)
                {
                    if(unclearCommentCount>0)
                    {
                        nextstatusDescription = $"IFR For {hStatus.Name}";
                        nextStatusId = hStatus.Id;
                    }
                    else
                    {
                        if (laststatus.IsContractorConfirm)
                        {
                            nextstatusDescription = "Document Is Confirmed";
                            nextStatusId = -1;
                        }
                        else
                        {
                            nextstatusDescription = $"Confirm {hStatus.Name}";
                            nextStatusId = hStatus.Id;
                        }
                    }
                }
                else
                {
                    if (mdr.Type == Common.MDRDocumentType.A)
                    {
                        if (unclearCommentCount == 0)
                        {
                            nextstatusDescription = nextStatus.Name;
                            nextStatusId = nextStatus.Id;
                        }
                        else
                        {
                            nextstatusDescription = $"IFR For {hStatus.Name}";
                            nextStatusId = hStatus.Id;
                        }
                    }
                    else
                    {
                        var seperateDays = (DateTime.Now - laststatus.CreatedDate).Days;
                        if (seperateDays > 14)
                        {
                            if (unclearCommentCount == 0)
                            {
                                nextstatusDescription = nextStatus.Name;
                                nextStatusId = nextStatus.Id;
                            }
                            else
                            {
                                nextstatusDescription = $"IFR For {hStatus.Name}";
                                nextStatusId = hStatus.Id;
                            }
                        }
                        else
                        {
                            if (unclearCommentCount == 0)
                            {
                                nextstatusDescription = "Waiting for response from contractor";
                                nextStatusId = -1;
                            }
                            else
                            {
                                nextstatusDescription = $"IFR For {hStatus.Name}";
                                nextStatusId = hStatus.Id;
                            }
                        }
                    }
                }
               
            }
            else
            {
                nextstatusDescription = nextStatus.Name;
                nextStatusId = nextStatus.Id;
            }

            var model = new MDRIssuanceDescription
            {
                Id=mdr.Id,
                LastCommentDate=lastCommentDate,
                LastStatus=hStatus.Name,
                Type=mdr.Type,
                UnclearComment=unclearCommentCount,
                NextStatusDescription= nextstatusDescription,
                NextStatusId=nextStatusId
            };
            return model;
        }

    }
}
