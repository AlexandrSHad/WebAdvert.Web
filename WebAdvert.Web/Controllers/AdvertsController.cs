using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAdvert.Web.Models.Adverts;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.Controllers
{
    public class AdvertsController : Controller
    {
        private readonly IFileUploader _fileUploader;

        public AdvertsController(IFileUploader fileUploader)
        {
            _fileUploader = fileUploader;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateAdvertModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // TODO: call AvertApi to save advert data and get id
                var id = "123456";

                if (imageFile != null)
                {
                    var fileName = !String.IsNullOrEmpty(imageFile.FileName)
                        ? Path.GetFileName(imageFile.FileName)
                        : id;
                    var filePath = $"{id}/{fileName}";

                    try
                    {
                        using (var readStrem = imageFile.OpenReadStream())
                        {
                            var result = await _fileUploader.UploadFileAsync(filePath, readStrem);

                            if (!result)
                            {
                                throw new Exception("Could not upload the image to file repository.");
                            }
                        }

                        // TODO: call AvertApi to confirm advert

                        return RedirectToAction("Index", "Home"); // TODO: redirect to list
                    }
                    catch (Exception ex)
                    {
                        // TODO: call AvertApi to cancel advert
                        Console.WriteLine(ex);
                    }
                }
            }

            return View(model);
        }
    }
}
