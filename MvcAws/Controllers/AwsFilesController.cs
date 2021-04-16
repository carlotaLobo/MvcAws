using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcAws.Helpers;
using MvcAws.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAws.Controllers
{
    public class AwsFilesController : Controller
    {
        private UploadHelper upload;
        public ServiceAWS3 service;

        public AwsFilesController(UploadHelper upload, ServiceAWS3 service)
        {
            this.upload = upload;
            this.service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ListFilesAws()
        {
            return View(await this.service.GetNameFiles());
        }
        public IActionResult UploadFileAws()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFileAws(IFormFile file)
        {
            String pathhelper = await this.upload.UploadLocal(file, Folder.Images);
            using (FileStream stream = new FileStream(pathhelper, FileMode.Open, FileAccess.Read))
            {
                bool respuesta = await this.service.UploadFile(stream, file.FileName);
                ViewBag.mensaje = "Archivo subido en AWS Bucket: " + respuesta;
            }
            return View();
        }
        public async Task<IActionResult> FileAws(String file)
        {
            return File(await this.service.GetFile(file), "image/png");
        }
        public async Task<IActionResult>DeleteAws(String file)
        {
            await this.service.DeleteFile(file);
            return RedirectToAction("ListFilesAws");
        }
    }
}
