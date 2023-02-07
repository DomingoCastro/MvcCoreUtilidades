using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Services;

namespace MvcCoreUtilidades.Controllers
{
    public class AzureFilesController : Controller
    {
        private ServiceStorageFiles service;

        public AzureFilesController(ServiceStorageFiles service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index(string fileName)
        {
            //SI NOS ENVIAN EL NOMBRE LO LEEMOS
            if (fileName !=null)
            {
                string data = await this.service.ReadFileAsync(fileName);
                ViewData["DATA"] = data;

            }
            List<string> files = await this.service.GetFilesAsync();
            return View(files);
        }

        public async Task<IActionResult> DeleteFile (string fileName)
        {
            await this.service.DeleteFileAsync(fileName);
            return RedirectToAction("Index");
        }
        public IActionResult UploadFiles()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile fileName)
        {
            //RECUPERAMOS EL NOMBRE DEL FICHERO A SUBIR
            string fileNames = fileName.FileName;
            //LEEMOS EL STREAM DE IformFile Y LO SUBIMOS A AZURE FILES
            using (Stream stream = fileName.OpenReadStream())
            {
                await this.service.UploadFileAsync(fileNames, stream);
            }
            ViewData["MENSAJE"] = "Archivo subido en Azure Files";
            return View();
        }

    }
}
