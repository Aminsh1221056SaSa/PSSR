using OfficeOpenXml;
using PSSR.ServiceLayer.ProjectServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.ExcelHelper
{
    public interface IExcelReportHelper
    {
        Task<MemoryStream> WBSProgressReportGroupe(List<WBSExcelDto> items);
        Task<MemoryStream> WBSProgressReportNormal(ExcelGroupedItems items);
        Task<MemoryStream> WBSWeithFactorReportGroupe(List<WBSExcelDto> items);
    }

    public class ExcelReportHelper : IExcelReportHelper
    {
        public async Task<MemoryStream> WBSProgressReportGroupe(List<WBSExcelDto> items)
        {
            await Task.Yield();
            var stream = new MemoryStream();

            using (ExcelPackage pck = new ExcelPackage(stream))
            {
                var worksheet = pck.Workbook.Worksheets.Add("Sheet1");
                var maxParentLevel = items.Max(s => s.ParentLevel) + 4;

                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "WBSCode";
                worksheet.Cells[1, 3].Value = "Activity Count";
                worksheet.Cells[1, 4].Value = "WF";

                worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                worksheet.Cells[1, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                worksheet.Cells[1, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                worksheet.Cells[1, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                for (var j = 5; j <= maxParentLevel; j++)
                {
                    worksheet.Cells[1, j].Value = $"Level {j - 4}";

                    worksheet.Cells[1, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                }

                var counter = items.Count();
                for (var i = 2; i <= counter + 1; i++)
                {
                    worksheet.Cells[i, 1].Value = items[i - 2].Name;
                    worksheet.Cells[i, 2].Value = items[i - 2].WBSCode;
                    worksheet.Cells[i, 3].Value = items[i - 2].ActivityCount;
                    worksheet.Cells[i, 4].Value = items[i - 2].WF;

                    worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    switch(items[i - 2].Type)
                    {
                        case Common.WBSType.Project:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                            break;
                        case Common.WBSType.WorkPackage:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.HotPink);
                            break;
                        case Common.WBSType.Location:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSalmon);
                            break;
                        case Common.WBSType.Descipline:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkMagenta);
                            break;
                        case Common.WBSType.System:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Khaki);
                            break;
                        case Common.WBSType.SubSystem:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.NavajoWhite);
                            break;
                        default:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);
                            break;
                    }
                  

                    worksheet.Cells[i, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[i, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);

                    for (var j = 5; j <= maxParentLevel; j++)
                    {
                        int itemParentLevel = items[i - 2].ParentLevel + 4;
                        float progress = items[i - 2].Progress;
                        worksheet.Cells[i, j].Value = progress;

                        worksheet.Cells[i, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        if (j == itemParentLevel)
                        {
                            if (progress <= 0)
                            {
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                            }
                            else if (progress > 0 && progress <= 25)
                            {
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                            }
                            else if (progress > 25 && progress <= 50)
                            {
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                            }
                            else if (progress > 50 && progress <= 90)
                            {
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            }
                            else
                            {
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);
                            }
                        }
                        else
                        {
                            worksheet.Cells[i, j].Value = 0;
                            worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Silver);
                        }
                    }

                    worksheet.Row(i).OutlineLevel = items[i-2].ParentLevel;
                    if (i != 2)
                        worksheet.Row(i).Collapsed = true;
                }

                //worksheet.Cells["B12"].Formula = "SUM(B2:B11)";
                //worksheet.Cells["C12"].Formula = "SUM(C2:C11)";

                

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.OutLineSummaryBelow = false;
                pck.Save();
            }
            return stream;
        }

        public async Task<MemoryStream> WBSWeithFactorReportGroupe(List<WBSExcelDto> items)
        {
            await Task.Yield();
            var stream = new MemoryStream();

            using (ExcelPackage pck = new ExcelPackage(stream))
            {
                var worksheet = pck.Workbook.Worksheets.Add("Sheet1");
                var maxParentLevel = items.Max(s => s.ParentLevel) + 4;

                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "WBSCode";
                worksheet.Cells[1, 3].Value = "Activity Count";
                //worksheet.Cells[1, 4].Value = "WF";

                worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                worksheet.Cells[1, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                worksheet.Cells[1, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                //worksheet.Cells[1, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //worksheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                for (var j = 4; j <= maxParentLevel; j++)
                {
                    worksheet.Cells[1, j].Value = $"Level {j - 3}";

                    worksheet.Cells[1, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                }

                var counter = items.Count();
                for (var i = 2; i <= counter + 1; i++)
                {
                    string iwbsCode = items[i - 2].WBSCode;
                    worksheet.Cells[i, 1].Value = items[i - 2].Name;
                    worksheet.Cells[i, 2].Value = iwbsCode;
                    worksheet.Cells[i, 3].Value = items[i - 2].ActivityCount;
                    //worksheet.Cells[i, 4].Value = items[i - 2].WF;

                    worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    switch (items[i - 2].Type)
                    {
                        case Common.WBSType.Project:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                            break;
                        case Common.WBSType.WorkPackage:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.HotPink);
                            break;
                        case Common.WBSType.Location:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSalmon);
                            break;
                        case Common.WBSType.Descipline:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkMagenta);
                            break;
                        case Common.WBSType.System:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Khaki);
                            break;
                        case Common.WBSType.SubSystem:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.NavajoWhite);
                            break;
                        default:
                            worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);
                            break;
                    }

                    worksheet.Cells[i, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[i, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);

                    for (var j = 4; j <= maxParentLevel; j++)
                    {
                        int itemParentLevel = items[i - 2].ParentLevel + 3;
                        float progress = items[i - 2].Progress;
                        worksheet.Cells[i, j].Value = progress;
                    
                        if((j-1)>3 && j==itemParentLevel)
                        {
                            var kprogress = progress;
                            var kwbs = iwbsCode;
                            for (int k = 1; k < (j - 3); k++)
                            {
                               var splitItem = kwbs.Split('-').ToList();
                               splitItem.RemoveAt(splitItem.Count - 1);
                                kwbs = string.Join('-', splitItem);
                                var valp = items.FirstOrDefault(s => s.WBSCode == kwbs);
                                if (valp != null)
                                {
                                    worksheet.Cells[i, j - k].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[i, j - k].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkTurquoise);
                                    kprogress = (kprogress * valp.Progress) / 100;
                                    worksheet.Cells[i, j - k].Value = kprogress;
                                }
                            }
                        }

                        worksheet.Cells[i, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        if (j == itemParentLevel)
                        {
                            worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        }
                        else
                        {
                            worksheet.Cells[i, j].Value = 0;
                            worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Silver);
                        }
                    }

                    if (i > 2)
                    {
                        worksheet.Row(i).OutlineLevel = items[i - 2].ParentLevel;
                        worksheet.Row(i).Collapsed = true;
                    }
                }
                //worksheet.Cells["B12"].Formula = "SUM(B2:B11)";
                //worksheet.Cells["C12"].Formula = "SUM(C2:C11)";

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.OutLineSummaryBelow = false;
                pck.Save();
            }
            return stream;
        }

        public async Task<MemoryStream> WBSProgressReportNormal(ExcelGroupedItems items)
        {
            await Task.Yield();
            var stream = new MemoryStream();

            using (ExcelPackage pck = new ExcelPackage(stream))
            {
                var worksheet = pck.Workbook.Worksheets.Add("Sheet1");
                var maxParentLevel = items.Groups.Max(s => s.Value.Item2) + 3;

                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "WBSCode";
                worksheet.Cells[1, 3].Value = "Activity Count";

                worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                worksheet.Cells[1, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                worksheet.Cells[1, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                for (var j = 4; j <= maxParentLevel; j++)
                {
                    worksheet.Cells[1, j].Value = $"Level {j - 3}";

                    worksheet.Cells[1, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                }

                var counter = items.Items.Count();
                for (var i = 2; i <= counter + 1; i++)
                {
                    worksheet.Cells[i, 1].Value = items.Items[i - 2].Name;
                    worksheet.Cells[i, 2].Value = items.Items[i - 2].WBSCode;
                    worksheet.Cells[i, 3].Value = items.Items[i - 2].ActivityCount;

                    worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);

                    worksheet.Cells[i, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[i, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);

                    for (var j = 4; j <= maxParentLevel; j++)
                    {
                        int itemParentLevel = items.Items[i - 2].ParentLevel + 3;
                        float progress = items.Items[i - 2].Progress;
                        worksheet.Cells[i, j].Value = progress;

                        worksheet.Cells[i, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        if (j == itemParentLevel)
                        {
                            if (progress <= 0)
                            {
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                            }
                            else if (progress > 0 && progress <= 25)
                            {
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                            }
                            else if (progress > 25 && progress <= 50)
                            {
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                            }
                            else if (progress > 50 && progress <= 90)
                            {
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            }
                            else
                            {
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);
                            }
                        }
                        else
                        {
                            worksheet.Cells[i, j].Value = 0;
                            worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Silver);
                        }
                    }
                }

                //worksheet.Cells["B12"].Formula = "SUM(B2:B11)";
                //worksheet.Cells["C12"].Formula = "SUM(C2:C11)";
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.OutLineSummaryBelow = false;
                pck.Save();
            }
            return stream;
        }
    }
}
