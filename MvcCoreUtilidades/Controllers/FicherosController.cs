using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Helpers;
using System.Globalization;

namespace MvcCoreUtilidades.Controllers
{
    public class FicherosController : Controller
    {
        private HelperPathProvider helper;

        //ESTA VARIABLE ES PARA RECUPERAR EL HOST DE MI WEB
        private string HostUrl;
        

        public FicherosController(HelperPathProvider helper,
            IHttpContextAccessor accessor)
        {
            this.helper= helper;
            //RECUPERAMOS LA URL DE NUESTRO HOST
            this.HostUrl = "https://" + accessor.HttpContext.Request.Host.Value + "/";
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
            ViewData["URLFICHERO"] = this.HostUrl + folder + "/" + fileName;
            return View();
        }
    }
}
