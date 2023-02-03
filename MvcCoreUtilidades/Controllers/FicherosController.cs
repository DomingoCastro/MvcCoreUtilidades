using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Helpers;
using System.Globalization;

namespace MvcCoreUtilidades.Controllers
{
    public class FicherosController : Controller
    {
        private HelperPathProvider helper;

        

        public FicherosController(HelperPathProvider helper)
        {
            this.helper= helper;
        }
        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile archivo)
        {
            string fileName = archivo.FileName;
            string path = this.helper.GetMapPath(Folders.Uploads, fileName);
            //UNA VEZ QUE TENEMOS LA RUTA, UN ARCHIVO SE LEE COMO STEAM, QUE ES UN FLUJO DE BYTES
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }
            ViewData["MENSAJE"] = "Fichero subido en: " + path;
            string folder = this.helper.GetNameFolder(Folders.Uploads);
            string hostUrl = this.helper.GetWebHostUrl();
            ViewData["URLFICHERO"] = hostUrl + folder + "/" + fileName;
            return View();
        }
    }
}
