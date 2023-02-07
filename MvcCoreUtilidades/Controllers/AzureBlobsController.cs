using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using MvcCoreUtilidades.Models;
using MvcCoreUtilidades.Services;

namespace MvcCoreUtilidades.Controllers
{
    public class AzureBlobsController : Controller
    {
        private ServiceStorageBlobs service;

        public AzureBlobsController(ServiceStorageBlobs service)
        {
            this.service = service;
        }
        public async Task<IActionResult> ListContainers()
        {
            List<string> containers = await this.service.GetContainersAsync();
            return View(containers);
        }

        public IActionResult CreateContainer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateContainer(string containername)
        {
            await this.service.CreateContainerAsync(containername);
            return RedirectToAction("ListContainers");
        }

        public async Task <IActionResult> DeleteContainer(string containername)
        {
            await this.service.DeleteContainerAsync(containername);
            return RedirectToAction("ListContainers");
        }

        //ACTIONRESULT PARA LOS BLOBS
        public async Task<IActionResult> ListBlobs(string containername)
        {
            List<BlobModel> models = await this.service.GetBlobsAsync(containername);
            ViewData["CONTAINERNAME"] = containername;
            return View(models);
        }
        public async Task<IActionResult> DeleteBlob(string containername, string blobName)
        {
            await this.service.DeleteBlobAsync(containername, blobName);
            return RedirectToAction("ListBlobs", new {containername = containername});
        }

        public IActionResult UploadBlob()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob (string containername, IFormFile file)
        {
            //RECUPERAMOS EL NOMBRE DEL FICHERO A SUBIR
            string fileNames = file.FileName;
            //LEEMOS EL STREAM DE IformFile Y LO SUBIMOS A AZURE FILES
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadBlobAsync(containername, fileNames, stream);
            }
            ViewData["MENSAJE"] = "Blob Subido";
            return View();
        }
    }
}
