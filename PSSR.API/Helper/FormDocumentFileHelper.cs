using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NetBarcode;
using OfficeOpenXml;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.API.Helper
{
    public class FormDocumentFileHelper
    {
        public async Task<string> SaveFormDocument(string formCode, IFormFile file, IHostingEnvironment enviroment)
        {
            var filePath = Path.Combine($"{enviroment.ContentRootPath}/wwwroot/exceldocuments");

            string extension = Path.GetExtension(file.FileName);

            var savePathxlsx = Path.Combine(filePath, $"{formCode}{extension}");

            if (File.Exists(savePathxlsx))
            {
                File.Delete(savePathxlsx);
            }

            using (var steam = new FileStream(savePathxlsx, FileMode.Create))
            {
                await file.CopyToAsync(steam);
            }

            return filePath;
        }

        public async Task SaveXlsxAsPdf(long docId, string fileName, IHostingEnvironment enviroment)
        {
            string extension = Path.GetExtension(fileName);
            await Task.Run(() =>
            {
                string xlsFilePath = Path.Combine(Path.Combine($"{enviroment.ContentRootPath}/wwwroot/exceldocuments", $"doc-{docId}{extension}"));
                var filePath = Path.Combine($"{enviroment.ContentRootPath}/wwwroot/pdfdocuements");
                var savePath = Path.Combine(filePath, $"doc-{docId}.pdf");
                if (!File.Exists(savePath))
                {
                    //Workbook workbook = new Workbook();
                    //workbook.LoadFromFile(xlsFilePath);
                    //workbook.SaveToFile(savePath, Spire.Xls.FileFormat.PDF);
                }
            });
        }

        //mdr manipulation
        public async Task<string> SaveMDRDocumentsNative(string mdrCode, string projectId, List<IFormFile> nativeFiles,
            IHostingEnvironment enviroment, string foldername, string statusName)
        {
            var filePath = Path.Combine($"{enviroment.ContentRootPath}/wwwroot/mdrdocuments/{projectId}/MDR-{mdrCode}/{statusName}/{foldername}");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fileNames = "";
            foreach (var file in nativeFiles)
            {
                var savePathxlsx = Path.Combine(filePath, $"{file.FileName}");
                if (File.Exists(savePathxlsx))
                {
                    File.Delete(savePathxlsx);
                }

                using (var steam = new FileStream(savePathxlsx, FileMode.Create))
                {
                    await file.CopyToAsync(steam);
                }
                fileNames += file.FileName + "%";
            }
            return fileNames;
        }

        public async Task<string> SaveMDRDocumentComment(string mdrCode, string projectId, IFormFile file, IHostingEnvironment enviroment)
        {
            var filePath = Path.Combine($"{enviroment.ContentRootPath}/wwwroot/mdrdocuments/{projectId}/MDR-{mdrCode}/Comments");

            var savePathxlsx = Path.Combine(filePath, $"{file.FileName}");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (File.Exists(savePathxlsx))
            {
                File.Delete(savePathxlsx);
            }

            using (var steam = new FileStream(savePathxlsx, FileMode.Create))
            {
                await file.CopyToAsync(steam);
            }

            return savePathxlsx;
        }

        public void renameMDRDocumentsFolder(string mdrCodeOld, string projectId, string newName,
            IHostingEnvironment enviroment)
        {
            var oldPath = Path.Combine($"{enviroment.ContentRootPath}/wwwroot/mdrdocuments/{projectId}/MDR{mdrCodeOld}");
            var newPath = Path.Combine($"{enviroment.ContentRootPath}/wwwroot/mdrdocuments/{projectId}/MDR{newName}");

            if (Directory.Exists(oldPath))
            {
                Directory.Move(oldPath, newPath);
            }
        }

        //activity manipulation
        public string MoveFormDocToActivityFolder(string formCode, long activityId, string projectId, IHostingEnvironment enviroment)
        {
            var formDocPath = Path.Combine($"{enviroment.ContentRootPath}/wwwroot/exceldocuments");
            var fromPathXls = Path.Combine(formDocPath, $"{formCode}.xls");

            bool isxls = true;
            if (!System.IO.File.Exists(fromPathXls))
            {
                fromPathXls = Path.Combine(formDocPath, $"{formCode}.xlsx");
                if (!System.IO.File.Exists(fromPathXls))
                {
                    return null;
                }
                isxls = false;
            }

            var toPath = Path.Combine($"{enviroment.ContentRootPath}/wwwroot/activityDocuemnts/{projectId}/task{activityId}");
            var saveToPath = Path.Combine(toPath, $"acDoc-{activityId}.xlsx");

            if (!Directory.Exists(toPath))
            {
                Directory.CreateDirectory(toPath);
            }

            if (!System.IO.File.Exists(saveToPath))
            {
                if (isxls)
                {
                    Workbook workbook = new Workbook();
                    workbook.LoadFromFile(fromPathXls);

                    workbook.SaveToFile(saveToPath, ExcelVersion.Version2013);
                }
                else
                {
                    File.Copy(fromPathXls, saveToPath);
                }

                string key = $"{activityId}{formCode}";
                CreateActivityBarcode(key, projectId, activityId, enviroment);
            }

            return saveToPath;
        }

        public async Task<string> SaveActivityDocument(long activityId, string projectId, IFormFile file, IHostingEnvironment enviroment)
        {
            var filePath = Path.Combine($"{enviroment.ContentRootPath}/wwwroot/activityDocuemnts/{projectId}/task{activityId}");

            string extension = Path.GetExtension(file.FileName);
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var savePathxlsx = Path.Combine(filePath, $"{fileName}{extension}");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (File.Exists(savePathxlsx))
            {
                File.Delete(savePathxlsx);
            }

            using (var steam = new FileStream(savePathxlsx, FileMode.Create))
            {
                await file.CopyToAsync(steam);
            }

            return savePathxlsx;
        }

        public byte[] ZipFolder(string folderPath)
        {
            byte[] path = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var directory in Directory.GetDirectories(folderPath))
                    {
                        string dirName = new DirectoryInfo($"{directory}").Name;
                        foreach (var file in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories))
                        {
                            byte[] fileByte = System.IO.File.ReadAllBytes(file);
                            var zipArchiveEntry = archive.CreateEntry($"{dirName}/{Path.GetFileName(file)}", System.IO.Compression.CompressionLevel.Optimal);

                            using (var originalFileStream = new MemoryStream(fileByte))
                            {
                                using (var zipEntryStream = zipArchiveEntry.Open())
                                {
                                    originalFileStream.CopyTo(zipEntryStream);
                                }
                            }
                        }
                    }
                }
                path = ms.ToArray();
            }
            return path;
        }

        //private methods
        private void CreateActivityBarcode(string input, string projectId, long activityId, IHostingEnvironment enviroment)
        {
            var barcode = new Barcode(input, NetBarcode.Type.Code128, true); // default: Code128
            var folderPath = $"{enviroment.ContentRootPath}/wwwroot/activityDocuemnts/{projectId}/task{activityId}";
            var filePath = Path.Combine(folderPath, $"acDoc-{activityId}.xlsx");

            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            var savePath = Path.Combine(folderPath, $"br-{activityId}.png");
            barcode.SaveImageFile(savePath, ImageFormat.Png);

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                using (ExcelPackage p = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.FirstOrDefault();

                    if (ws != null)
                    {
                        this.AddPictures(ws, savePath);
                        p.Save();
                    }
                }
            }
        }

        private void AddPictures(ExcelWorksheet ws, string imagePath)
        {
            var image = Image.FromFile(imagePath);
            OfficeOpenXml.Drawing.ExcelPicture pic = ws.Drawings.AddPicture("barcodePic", image);
            pic.SetPosition(-1, -1);
            pic.SetSize(170, 70);
        }
    }
}
