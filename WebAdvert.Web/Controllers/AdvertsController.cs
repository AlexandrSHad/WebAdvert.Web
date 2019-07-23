using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAdvert.Web.Models.Adverts;
using WebAdvert.Web.ServiceClients;
using WebAdvert.Web.ServiceClients.Models;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.Controllers
{
    public class AdvertsController : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly IAdvertApiClient _advertApiClient;
        private readonly IMapper _mapper;

        public AdvertsController(IFileUploader fileUploader, IAdvertApiClient advertApiClient, IMapper mapper)
        {
            _fileUploader = fileUploader;
            _advertApiClient = advertApiClient;
            _mapper = mapper;
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
                var advertModel = _mapper.Map<AdvertModel>(model);

                var response = await _advertApiClient.CreateAsync(advertModel);
                // TODO: check for successful response
                var id = response.Id;

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

                        var confirmed = await _advertApiClient.ConfirmAsync(new ConfirmAdvertModel
                        {
                            Id = id,
                            FilePath = filePath,
                            Status = AdvertStatus.Active
                        });

                        if (!confirmed)
                        {
                            throw new Exception($"Could not confirm advert of id = {id}");
                        }

                        return RedirectToAction("Index", "Home"); // TODO: redirect to list
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        await _advertApiClient.ConfirmAsync(new ConfirmAdvertModel
                        {
                            Id = id,
                            Status = AdvertStatus.Pending // TODO: use deleted status
                        });

                        // TODO: redirect to error page
                        throw ex;
                    }
                }
            }

            return View(model);
        }
    }
}
