
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.Logic.Activityes;
using PSSR.Logic.FormDictionaries;
using PSSR.Logic.ProjectSubSystems;
using PSSR.Logic.ProjectSystmes;
using PSSR.Logic.Punches;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers
{
    public class ExcelFileConverterHelper
    {
        public async Task<Tuple<string, List<FormDictionary>>> ParseFormDictionaryExcel(IFormFile excelFile,
            Dictionary<string, int> desciplines, Dictionary<string, int> workPackages)
        {
            var masterValueList = new List<FormDictionary>();
            string errorMsg = "";
            string xlsFilePath = "/wwwroot/exceldocuments";

            using (var memoryStream = new MemoryStream())
            {
                // Get MemoryStream from Excel file
                await excelFile.CopyToAsync(memoryStream);
                // Create a ExcelPackage object from MemoryStream
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    // Get the first Excel sheet from the Workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
                    int rowCount = worksheet.Dimension.Rows+1;
                    for (int row = 3; row <= rowCount; row++)
                    {
                        int desciplineId = 0;
                        var masterDataValue = new FormDictionaryDto();

                        if(worksheet.Cells[row, 4].Value!=null)
                        {
                            string desTem = worksheet.Cells[row, 4].Value.ToString().ToUpper();
                            if (!desciplines.ContainsKey(desTem))
                            {
                                errorMsg += $"Descipline not valid--row{row}--column{3}" + "<br/>"; ;
                            }
                            else
                            {
                                desciplineId = desciplines[desTem];
                            }

                            string pCode = worksheet.Cells[row, 5].Value.ToString();
                            if (string.IsNullOrWhiteSpace(pCode))
                            {
                                errorMsg += $"Descipline not valid--row{row}--column{3}" + "<br/>"; ;
                            }
                            else
                            {
                                masterDataValue.Code = pCode;
                            }

                            string workItem = worksheet.Cells[row, 3].Value.ToString().ToUpper();
                            if (!workPackages.ContainsKey(workItem))
                            {
                                errorMsg += $"WorkPackage not valid--row{row}--column{2}" + "<br/>"; ;
                            }
                            else
                            {
                                masterDataValue.WorkPackageId = workPackages[workItem];
                            }

                            string type = worksheet.Cells[row, 6].Value.ToString().Trim();

                            if (string.Equals(type, "CheckSheet", StringComparison.InvariantCultureIgnoreCase))
                            {
                                masterDataValue.Type = FormDictionaryType.Check;
                            }
                            else if (string.Equals(type, "TestSheet", StringComparison.InvariantCultureIgnoreCase))
                            {
                                masterDataValue.Type = FormDictionaryType.Test;
                            }
                            else
                            {
                                errorMsg += $"Form Type not valid--row{row}--column{5}" + "<br/>"; ;
                            }

                            masterDataValue.Description = worksheet.Cells[row, 7].Value.ToString();

                            string priority = worksheet.Cells[row, 8].Value.ToString();
                            int priorityParse = 0;
                            if (!int.TryParse(priority, out priorityParse))
                            {
                                errorMsg += $"priority not valid--row{row}--column{2}" + "<br/>"; ;
                            }
                            else
                            {
                                masterDataValue.Priority = priorityParse;
                            }

                            string mh = worksheet.Cells[row, 9].Value.ToString();
                            float mhparse = 0;
                            if (!float.TryParse(mh, out mhparse))
                            {
                                errorMsg += $"Man Hours not valid--row{row}--column{2}" + "<br/>"; ;
                            }
                            else
                            {
                                masterDataValue.Mh = priorityParse;
                            }
                            //optional
                            masterDataValue.ActivityName = "Default";

                            if (masterValueList.Any(x => string.Equals(masterDataValue.Code, x.Code, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                var oldForm = masterValueList.Single(s => s.Code == masterDataValue.Code);
                                if (!oldForm.DesciplineLink.Any(s => s.DesciplineId == desciplineId))
                                {
                                    oldForm.AddDescipline(desciplineId);
                                }
                                else
                                {
                                    errorMsg += $"Descipline is duplicated for Form {oldForm.Code}--row{row}--column{5}" + "<br/>";
                                }
                            }
                            else
                            {
                                var filePath = Path.Combine(Path.Combine(xlsFilePath, $"{masterDataValue.Code}"));
                                masterValueList.Add(FormDictionary.CreateFormDicFactory(masterDataValue.Type, masterDataValue.Description,
                                  masterDataValue.Code, masterDataValue.ActivityName, filePath,
                                  masterDataValue.WorkPackageId, new int[] { desciplineId }, masterDataValue.Priority, masterDataValue.Mh).Result);
                            }
                        }
                    }
                }
            }
            return new Tuple<string, List<FormDictionary>>(errorMsg, masterValueList);
        }

        public async Task<Tuple<string, List<ProjectSystem>>> ParseSystemExcel(IFormFile excelFile,Guid projectId)
        {
            var masterValueList = new List<ProjectSystem>();
            string errorMsg = "";

            using (var memoryStream = new MemoryStream())
            {
                await excelFile.CopyToAsync(memoryStream);
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows+1;
                    for (int row = 3; row <= rowCount; row++)
                    {
                        var masterDataValue = new ProjectSystemDto();

                        if(worksheet.Cells[row, 3].Value!=null)
                        {
                            string pCode = worksheet.Cells[row, 3].Value.ToString();
                            if (string.IsNullOrWhiteSpace(pCode))
                            {
                                errorMsg += $"System code not valid--row{row}--column{3}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.Code = pCode;
                            }

                            masterDataValue.Description = worksheet.Cells[row, 4].Value.ToString();

                            if (masterValueList.Any(s => s.Code == masterDataValue.Code))
                            {
                                errorMsg += $"System code is duplicated--row{row}--column{3}" + "<br/>";
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(errorMsg))
                                {
                                    masterValueList.Add(ProjectSystem.CreateProjectSystem(masterDataValue.Code, masterDataValue.Description,
                                        SystemType.Process, projectId).Result);
                                }
                            }
                        }
                    }
                }
            }
            return new Tuple<string, List<ProjectSystem>>(errorMsg, masterValueList);
        }

        public async Task<Tuple<string, List<ProjectSubSystem>>> ParseSubSystemExcel(IFormFile excelFile, Dictionary<string, int> systems, Guid projectId)
        {
            var masterValueList = new List<ProjectSubSystem>();
            string errorMsg = "";

            using (var memoryStream = new MemoryStream())
            {
                await excelFile.CopyToAsync(memoryStream);
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows+1;
                    for (int row = 3; row <= rowCount; row++)
                    {
                        var masterDataValue = new ProjectSubSystemDto();

                        if (worksheet.Cells[row, 3].Value != null)
                        {
                            string system = worksheet.Cells[row, 3].Value.ToString();
                            if (!systems.ContainsKey(system))
                            {
                                errorMsg += $"system not valid--row{row}--column{2}" + Environment.NewLine;
                            }
                            else
                            {
                                masterDataValue.ProjectSystemId = systems[system];
                            }

                            string pCode = worksheet.Cells[row, 4].Value.ToString();
                            if (string.IsNullOrWhiteSpace(pCode))
                            {
                                errorMsg += $"sub System code not valid--row{row}--column{3}" + Environment.NewLine;
                            }
                            else
                            {
                                masterDataValue.Code = pCode;
                            }

                            masterDataValue.Description = worksheet.Cells[row, 5].Value.ToString();

                            if (masterValueList.Any(s => s.Code == masterDataValue.Code))
                            {
                                errorMsg += $"subsystem code is duplicated--row{row}--column{3}" + "<br/>";
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(errorMsg))
                                {
                                    masterValueList.Add(ProjectSubSystem.CreateProjectSubSystem(masterDataValue.Code, masterDataValue.Description,
                                      masterDataValue.ProjectSystemId, 1, null).Result);
                                }
                            }
                        }
                    }
                }
            }
            return new Tuple<string, List<ProjectSubSystem>>(errorMsg, masterValueList);
        }

        public Tuple<string, List<Activity>> ParseActivityExcel(IFormFile excelFile, Dictionary<string, int> valueUnits, Dictionary<string, int> workPackages
            , Dictionary<string, int> locations, Dictionary<string, IEnumerable<Tuple<long, string, int>>> desciplines,Dictionary<string, long> systems
            , Dictionary<string, Tuple<long,int>> subsytems,Dictionary<string, int> workSteps)
        {
            var masterValueList = new List<Activity>();

            string errorMsg = "";

            using (var memoryStream = new MemoryStream())
            {
                excelFile.CopyTo(memoryStream);
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[3];
                    int rowCount = worksheet.Dimension.Rows + 1;
                    for(int row=3;row<=rowCount;row++)
                    {
                        var masterDataValue = new ActivityDto();

                        if (worksheet.Cells[row, 1].Value != null)
                        {
                            string accode = worksheet.Cells[row, 1].Value?.ToString();
                            if (string.IsNullOrWhiteSpace(accode))
                            {
                                errorMsg += $"Activity Code not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                if (masterValueList.Any(s => s.ActivityCode == accode))
                                {
                                    errorMsg += $"Duplicated Punch Code--row{row}" + "<br/>";
                                }
                                else
                                {
                                    masterDataValue.ActivityCode = accode;
                                }
                            }

                            string workPackage = worksheet.Cells[row, 2].Value?.ToString().ToUpper();
                            if (!workPackages.ContainsKey(workPackage))
                            {
                                errorMsg += $"workpackage not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.WorkPackageId = workPackages[workPackage];
                            }

                            string workPackageStep = worksheet.Cells[row, 13].Value?.ToString().ToUpper();
                            if (!workSteps.ContainsKey(workPackageStep))
                            {
                                errorMsg += $"workpackage step not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.WorkPackageStepId = workSteps[workPackageStep];
                            }

                            string location = worksheet.Cells[row, 3].Value?.ToString().ToUpper();
                            if (!locations.ContainsKey(location))
                            {
                                errorMsg += $"location not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.LocationId = locations[location];
                            }

                            string system = worksheet.Cells[row, 4].Value?.ToString().ToUpper();
                            if (!systems.ContainsKey(system))
                            {
                                errorMsg += $"System not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                long systemId = systems[system];
                                string subSystem = worksheet.Cells[row, 5].Value?.ToString().ToUpper();
                                if (!subsytems.ContainsKey(subSystem))
                                {
                                    errorMsg += $"SubSystem not valid--row{row}" + "<br/>";
                                }
                                else
                                {
                                    if (!(subsytems[subSystem].Item2 == systemId))
                                    {
                                        errorMsg += $"system and subsystem not any related in this project--row{row}" + "<br/>";
                                    }
                                    else
                                    {
                                        masterDataValue.SubsytemId = subsytems[subSystem].Item1;
                                    }
                                }
                            }

                            string descipline = worksheet.Cells[row, 6].Value?.ToString().ToUpper();
                            if (!desciplines.ContainsKey(descipline))
                            {
                                errorMsg += $"Descipline not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                var relatedDescipline = desciplines[descipline];
                                string formCode = worksheet.Cells[row, 9].Value?.ToString().ToUpper();
                                if (!relatedDescipline.Any(s => s.Item2 == formCode))
                                {
                                    errorMsg += $"form dictionary not valid--row{row}" + "<br/>";
                                }
                                else
                                {
                                    long formId = relatedDescipline.First(s => s.Item2.ToUpper() == formCode).Item1;
                                    masterDataValue.DesciplineId = relatedDescipline.First(s => s.Item2.ToUpper() == formCode).Item3;
                                    masterDataValue.FormDictionaryId = formId;
                                }
                            }

                            string tagNumber = worksheet.Cells[row, 7].Value?.ToString();
                            if (string.IsNullOrWhiteSpace(tagNumber))
                            {
                                errorMsg += $"tagNumber not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.TagNumber = tagNumber;
                            }

                            string mh = worksheet.Cells[row, 12].Value?.ToString();
                            float mhint = 0;
                            if (!float.TryParse(mh, out mhint))
                            {
                                errorMsg += $"M/h not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.EstimateMh = (int)mhint;
                            }

                            masterDataValue.TagDescription = worksheet.Cells[row, 8].Value?.ToString();

                            string vUnit = worksheet.Cells[row, 14].Value.ToString().ToUpper();
                            if (!valueUnits.ContainsKey(vUnit))
                            {
                                errorMsg += $"ValueUnit not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.ValueUnitId = valueUnits[vUnit];
                            }

                            string vUnitNum = worksheet.Cells[row, 15].Value?.ToString();
                            float vintNum = 0;

                            if (!float.TryParse(vUnitNum, out vintNum))
                            {
                                errorMsg += $"ValueUnit Num not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.ValueUnitNum = vintNum;
                            }

                            string vStatus = worksheet.Cells[row, 16].Value?.ToString();
                            ActivityStatus vsta = ActivityStatus.NotStarted;

                            if (!Enum.TryParse(vStatus, out vsta))
                            {
                                errorMsg += $"Status not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.Status = vsta;
                                if (vsta == ActivityStatus.Done)
                                {
                                    masterDataValue.Progress = 100;
                                }
                            }

                            string vCondition = worksheet.Cells[row, 17].Value?.ToString();
                            ActivityCondition vco = ActivityCondition.Normal;

                            if (!Enum.TryParse(vCondition, out vco))
                            {
                                errorMsg += $"Condition not valid--row{row}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.Condition = vco;
                            }

                            if (string.IsNullOrWhiteSpace(errorMsg))
                            {
                                masterValueList.Add(Activity.CreateActivity
                                  (masterDataValue.TagNumber, masterDataValue.TagDescription, masterDataValue.Progress, 0, masterDataValue.ValueUnitNum
                                  , masterDataValue.EstimateMh, 0, masterDataValue.Status, null, null, null, null, masterDataValue.FormDictionaryId, masterDataValue.ValueUnitId
                                  , masterDataValue.WorkPackageId, masterDataValue.LocationId, masterDataValue.SubsytemId,
                                  masterDataValue.Condition, masterDataValue.ActivityCode
                                  , masterDataValue.DesciplineId, masterDataValue.WorkPackageStepId).Result);
                            }
                        }
                    }
                }
            }
            return new Tuple<string, List<Activity>>(errorMsg, masterValueList);
        }

        public async Task<Tuple<string, List<Punch>>> ParsePunchExcel(IFormFile excelFile, Dictionary<string,long> activityies,
            Dictionary<int,string> types, Dictionary<int, string> categories)
        {
            var masterValueList = new List<Punch>();
            string errorMsg = "";

            using (var memoryStream = new MemoryStream())
            {
                await excelFile.CopyToAsync(memoryStream);
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[4];
                    int rowCount = worksheet.Dimension.Rows + 1;

                    for (int row = 3; row <= rowCount; row++)
                    {
                        var masterDataValue = new PunchDto();

                        if (worksheet.Cells[row, 2].Value != null)
                        {
                            string code = worksheet.Cells[row, 4].Value?.ToString();
                            if (string.IsNullOrWhiteSpace(code))
                            {
                                errorMsg += $"code not valid--row{row}--column{2}" + "<br/>";
                            }
                            else
                            {
                                if (masterValueList.Any(s => s.Code == code))
                                {
                                    errorMsg += $"Duplicated Punch Code--row{row}" + "<br/>";
                                }
                                else
                                {
                                    masterDataValue.Code = code;
                                }
                            }

                            string activityCode = worksheet.Cells[row, 2].Value?.ToString().ToUpper();
                            if (!activityies.ContainsKey(activityCode))
                            {
                                errorMsg += $"Task not valid--row{row}--column{2}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.ActivityId = activityies[activityCode];
                            }

                            string ptype = worksheet.Cells[row, 3].Value?.ToString();
                            if (!types.ContainsValue(ptype))
                            {
                                errorMsg += $"type not valid--row{row}--column{2}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.PunchTypeId = types.First(s => s.Value == ptype).Key;
                            }

                            string orginatedBy = worksheet.Cells[row, 5].Value?.ToString();
                            if (string.IsNullOrWhiteSpace(orginatedBy))
                            {
                                errorMsg += $"CreatedBy By not valid--row{row}--column{2}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.OrginatedBy = orginatedBy;
                            }

                            string checkBy = worksheet.Cells[row, 6].Value?.ToString();
                            masterDataValue.CheckBy = checkBy;

                            string approveBy = worksheet.Cells[row, 7].Value?.ToString();
                            masterDataValue.ApproveBy = approveBy;

                            string acmh = worksheet.Cells[row, 8].Value?.ToString();
                            if (!int.TryParse(acmh, out var acmhint))
                            {
                                errorMsg += $"Actual Mh not valid--row{row}--column{2}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.ActualMh = acmhint;
                            }

                            string esmh = worksheet.Cells[row, 9].Value?.ToString();
                            if (!int.TryParse(esmh, out var esmhint))
                            {
                                errorMsg += $"Estimate Mh not valid--row{row}--column{2}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.EstimateMh = esmhint;
                            }

                            //
                            string clearPlan = worksheet.Cells[row, 10].Value?.ToString();
                            masterDataValue.ClearPlan = clearPlan;

                            string vendor = worksheet.Cells[row, 11].Value?.ToString();
                            masterDataValue.VendorName = vendor;

                            string correctActivation = worksheet.Cells[row, 12].Value?.ToString();
                            masterDataValue.CorectiveAction = correctActivation;

                            string vRequired = worksheet.Cells[row, 13].Value?.ToString();
                            masterDataValue.VendorRequired = string.Equals(vRequired, "1");

                            string mRequired = worksheet.Cells[row, 14].Value?.ToString();
                            masterDataValue.MaterialRequired = string.Equals(mRequired, "1");

                            string eRequired = worksheet.Cells[row, 15].Value?.ToString();
                            masterDataValue.EnginerigRequired = string.Equals(eRequired, "1");

                            string defDesc = worksheet.Cells[row, 16].Value?.ToString();
                            masterDataValue.DefectDescription = defDesc;

                            string orginatedDate = worksheet.Cells[row, 17].Value?.ToString();
                            if (!DateTime.TryParse(orginatedDate, out var dateItem))
                            {
                                errorMsg += $"orginated date not valid--row{row}--column{2}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.OrginatedDate = dateItem;
                            }

                            string checkDatestring = worksheet.Cells[row, 18].Value?.ToString();
                            if (!string.IsNullOrWhiteSpace(checkDatestring))
                            {
                                if (!DateTime.TryParse(checkDatestring, out var checkDate))
                                {
                                    errorMsg += $"CheckDate date not valid--row{row}--column{2}" + "<br/>";
                                }
                                else
                                {
                                    masterDataValue.CheckDate = checkDate;
                                }
                            }

                            string approveDatestring = worksheet.Cells[row, 19].Value?.ToString();
                            if (!string.IsNullOrWhiteSpace(approveDatestring))
                            {
                                if (!DateTime.TryParse(approveDatestring, out var aprovkDate))
                                {
                                    errorMsg += $"CheckDate date not valid--row{row}--column{2}" + "<br/>";
                                }
                                else
                                {
                                    masterDataValue.ClearDate = aprovkDate;
                                }
                            }

                            string cgh = worksheet.Cells[row, 20].Value?.ToString();
                            if (!categories.ContainsValue(cgh))
                            {
                                errorMsg += $"category not valid--row{row}--column{2}" + "<br/>";
                            }
                            else
                            {
                                masterDataValue.CategoryId = categories.First(s => s.Value == cgh).Key;
                            }

                            if (string.IsNullOrWhiteSpace(errorMsg))
                            {
                                masterValueList.Add(Punch.CreatePunch(masterDataValue.Code, masterDataValue.DefectDescription, masterDataValue.OrginatedBy
                                   , masterDataValue.CreatedBy, masterDataValue.CheckBy, masterDataValue.ApproveBy, masterDataValue.OrginatedDate, masterDataValue.CheckDate, masterDataValue.ClearDate, masterDataValue.EstimateMh, masterDataValue.ActualMh,
                                   masterDataValue.VendorRequired, masterDataValue.VendorName, masterDataValue.MaterialRequired, masterDataValue.EnginerigRequired
                                   , masterDataValue.ClearPlan, masterDataValue.CorectiveAction, masterDataValue.PunchTypeId, masterDataValue.ActivityId,masterDataValue.CategoryId).Result);
                            }
                        }
                    }
                }
                return new Tuple<string, List<Punch>>(errorMsg, masterValueList);
            }
        }
    }
}
